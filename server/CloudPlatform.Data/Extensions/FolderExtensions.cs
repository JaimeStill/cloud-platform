using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CloudPlatform.Core.Extensions;
using CloudPlatform.Data.Entities;

namespace CloudPlatform.Data.Extensions
{
    public static class FolderExtensions
    {
        public static async Task<string> GetBreadcrumbs(this AppDbContext db, int? folderId)
        {
            if (folderId is not null)
            {
                var folder = await db.Folders.FindAsync(folderId);

                if (folder is not null)
                {
                    // Thank you Aaron
                    var breadcrumbs = new StringBuilder();

                    breadcrumbs.Insert(0, $"/{folder.Name}");
                    breadcrumbs.Insert(0, await db.GetBreadcrumbs(folder.FolderId));

                    return breadcrumbs.ToString();
                }
                else return null;
            }
            else return null;
        }

        public static async Task<List<Folder>> GetRootFolders(this AppDbContext db, string owner) =>
            await db.Folders
                .Where(x =>
                    x.Owner == owner &&
                    x.FolderId == null
                )
                .OrderBy(x => x.Name)
                .ToListAsync();

        public static async Task<List<Folder>> GetRootSharedFolders(this AppDbContext db, string username) =>
            await db.SharedFolders
                .Include(x => x.Folder)
                .Where(x =>
                    x.Username == username &&
                    x.Folder.FolderId == null
                )
                .Select(x => x.Folder)
                .OrderBy(x => x.Name)
                .ToListAsync();

        public static async Task<List<Folder>> GetSubFolders(this AppDbContext db, int folderId)
        {
            var subfolders = await db.Folders
                .Where(x => x.FolderId == folderId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return subfolders;
        }

        public static async Task<Folder> GetFolder(this AppDbContext db, string owner, string path) =>
            await db.Folders
                .FirstOrDefaultAsync(x =>
                    x.Path == path &&
                    x.Owner == owner
                );

        public static async Task<bool> ValidateFolderName(this AppDbContext db, Folder folder)
        {
            var check = await db.Folders
                .FirstOrDefaultAsync(x =>
                    x.FolderId == folder.FolderId &&
                    x.Owner == folder.Owner &&
                    x.Id != folder.Id &&
                    x.Name == folder.Name.UrlEncode()
                );

            return check is null;
        }

        public static async Task AddFolder(this AppDbContext db, Folder folder)
        {
            if (await folder.Validate(db))
            {
                folder.Path = await folder.GeneratePath(db);

                await db.Folders.AddAsync(folder);
                await db.SaveChangesAsync();
            }
        }
        public static async Task UpdateFolder(this AppDbContext db, Folder folder)
        {
            if (await folder.Validate(db))
            {
                folder.Path = await folder.GeneratePath(db);

                db.Folders.Update(folder);
                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveFolder(this AppDbContext db, Folder folder)
        {
            folder.RemoveSharedFolders(db);
            db.Folders.Remove(folder);
            await db.SaveChangesAsync();
        }

        public static async Task ShareFolder(this AppDbContext db, int folderId, List<User> users) // this is also for bonus points
        {
            var names = await db.GetSharedFolderUsers(folderId);

            var sharedFolders = users
                .Where(x => !names.Contains(x.Username))
                .Select(user => new SharedFolder()
                {
                    FolderId = folderId,
                    Username = user.Username
                });

            await db.SharedFolders.AddRangeAsync(sharedFolders);
            await db.SaveChangesAsync();
        }

        public static async Task UnshareFolder(this AppDbContext db, int folderId, string username)
        {
            var folder = await db.SharedFolders
                .FirstOrDefaultAsync(x =>
                    x.FolderId == folderId &&
                    x.Username == username
                );

            if (folder is not null)
            {
                db.SharedFolders.Remove(folder);
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"{username} does not have access to the specified folder");
            }
        }

        static async Task<bool> Validate(this Folder folder, AppDbContext db)
        {
            if (string.IsNullOrEmpty(folder.Name))
            {
                throw new Exception("Folder must have a name");
            }

            if (!await db.ValidateFolderName(folder))
            {
                throw new Exception($"{folder.Name} is already a Folder in this directory");
            }

            return true;
        }

        static async Task<string> GeneratePath(this Folder folder, AppDbContext db) => $"{await db.GetBreadcrumbs(folder.FolderId)}/{folder.Name}";

        static void RemoveSharedFolders(this Folder folder, AppDbContext db) =>
            db.SharedFolders.RemoveRange(
                db.SharedFolders.Where(x => x.FolderId == folder.Id)
            );
    }
}