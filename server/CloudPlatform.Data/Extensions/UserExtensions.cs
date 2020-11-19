using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CloudPlatform.Data.Entities;
using CloudPlatform.Core.Extensions;

namespace CloudPlatform.Data.Extensions
{
    public static class UserExtensions
    {
        static IQueryable<User> Search(this IQueryable<User> users, string search) =>
            users.Where(x => x.Username.ToLower().Contains(search.ToLower()));

        public static async Task<List<User>> GetUsers(this AppDbContext db) =>
            await db.Users
                .OrderBy(x => x.Username)
                .ToListAsync();

        public static async Task<List<User>> SearchUsers(this AppDbContext db, string search) =>
            await db.Users
                .Where(x => x.Username.Contains(search.UrlEncode()))
                .OrderBy(x => x.Username)
                .ToListAsync();

        public static async Task<User> GetUser(this AppDbContext db, string username) =>
            await db.Users
                .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

        public static async Task<bool> ValidateUsername(this AppDbContext db, User user)
        {
            var check = await db.Users
                .FirstOrDefaultAsync(x =>
                    x.Id != user.Id &&
                    x.Username == user.Username.UrlEncode()
                );

            return check is null;
        }

        public static async Task<User> AddUser(this AppDbContext db, User user)
        {
            if (await user.Validate(db))
            {
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();

                return user;
            }

            return null;
        }

        public static async Task UpdateUser(this AppDbContext db, User user)
        {
            if (await user.Validate(db))
            {
                db.Users.Update(user);
                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveUser(this AppDbContext db, User user)
        {
            user.RemoveSharedFolders(db);
            user.RemoveSharedNotes(db);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
        }

        static void RemoveSharedFolders(this User user, AppDbContext db)
        {
            var sharedFolders = db.SharedFolders.Where(x => x.UserId == user.Id);
            db.SharedFolders.RemoveRange(sharedFolders);
        }

        static void RemoveSharedNotes(this User user, AppDbContext db)
        {
            var sharedNotes = db.SharedNotes.Where(x => x.UserId == user.Id);
            db.SharedNotes.RemoveRange(sharedNotes);
        }

        static async Task<bool> Validate(this User user, AppDbContext db)
        {
            if (string.IsNullOrEmpty(user.Username))
            {
                throw new Exception("User must have a username");
            }

            if (!await db.ValidateUsername(user))
            {
                throw new Exception($"${user.Username} is already a User");
            }

            return true;
        }
    }
}