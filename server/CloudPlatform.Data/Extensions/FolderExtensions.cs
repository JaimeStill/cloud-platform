using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CloudPlatform.Data.Entities;

namespace CloudPlatform.Data.Extensions
{
    public static class FolderExtensions
    {
        public static async Task<List<Folder>> GetFolders(this AppDbContext db, int userId) =>
            await db.Folders
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        public static async Task<List<Folder>> SearchFolders(this AppDbContext db, int userId, string search)
        {
            var userFolders = await db.GetFolders(userId);

            return userFolders.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
        }
        public static async Task<Folder> GetFolder(this AppDbContext db, int id) =>
            await db.Folders
                .FindAsync(id);

        public static async Task AddFolder(this AppDbContext db, Folder folder)
        {
            if (await folder.Validate(db))
            {
                await db.Folders.AddAsync(folder);
                await db.SaveChangesAsync();
            }
        }
        public static async Task UpdateFolder(this AppDbContext db, Folder folder)
        {
            if (await folder.Validate(db))
            {
                db.Folders.Update(folder);
                await db.SaveChangesAsync();
            }
        }
        public static async Task ShareFolder(this AppDbContext db, int id, List<User> users) // this is also for bonus points
        {
            var sharedFolders = users.Select(user => new SharedFolder()
            {
                FolderId = id,
                UserId = user.Id
            });

            await db.SharedFolders.AddRangeAsync(sharedFolders);
            await db.SaveChangesAsync();
        }
        public static async Task RemoveFolder(this AppDbContext db, Folder folder)
        {
            if (await folder.Validate(db))
            {
                db.Folders.Remove(folder);
                await db.SaveChangesAsync();
            }
        }
        public static async Task UnshareFolder(this AppDbContext db, SharedFolder folder)
        {
            db.SharedFolders.Remove(folder);
            await db.SaveChangesAsync();
        }

        // possibly need another validate for sharing

        static async Task<bool> Validate(this Folder folder, AppDbContext db)
        {
            if (string.IsNullOrEmpty(folder.Name))
            {
                throw new Exception("Folder must have a name");
            }

            var check = await db.Folders
                .FirstOrDefaultAsync(x =>
                    x.Id != folder.Id &&
                    x.Name.ToLower() == folder.Name.ToLower()
                );

            if (check != null)
            {
                throw new Exception($"{folder.Name} is already a Folder");
            }

            return true;
        }
    }
}