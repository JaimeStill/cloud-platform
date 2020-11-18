using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CloudPlatform.Core;
using CloudPlatform.Data.Entities;

namespace CloudPlatform.Data.Extensions
{
    public static class UserExtensions
    {
        public static async Task<List<User>> GetUsers(this AppDbContext db) =>
            await db.Users
                .OrderBy(x => x.Username)
                .ToListAsync();
        public static async Task<List<User>> SearchUsers(this AppDbContext db, string search) =>
            await db.Users
                .Where(x => x.Username.Contains(search.ToLower()))
                .ToListAsync();

        public static async Task<User> GetUser(this AppDbContext db, string username) =>
            await db.Users
                .FindAsync(username);
        public static async Task AddUser(this AppDbContext db, User user)
        {
            if (await user.Validate(db))
            {
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
            }
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
            if (await user.Validate(db))
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
        }

        static async Task<bool> Validate(this User user, AppDbContext db)
        {
            if (string.IsNullOrEmpty(user.Username))
            {
                throw new AppException("User must have a Username", ExceptionType.Validation);
            }

            var check = await db.Users
                .FirstOrDefaultAsync(x =>
                    x.Id != user.Id &&
                    x.Username.ToLower() == user.Username.ToLower()
                );

            if (check != null)
            {
                throw new AppException($"{user.Username} is already a User", ExceptionType.Validation);
            }

            return true;
        }
    }
}