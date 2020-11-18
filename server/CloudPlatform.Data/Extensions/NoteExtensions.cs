using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CloudPlatform.Core;
using CloudPlatform.Data.Entities;

namespace CloudPlatform.Data.Extensions
{
    public static class NoteExtensions
    {
        public static async Task<List<Note>> GetNotes(this AppDbContext db, int folderId) =>
            await db.Notes
                .OrderBy(x => x.Title)
                .ToListAsync();

        public static async Task<List<Note>> SearchNotes(this AppDbContext db, int folderId, string search)
        {
            var folderNotes = await db.GetNotes(folderId);

            return folderNotes.Where(x => x.Title.ToLower().Contains(search.ToLower())).ToList();
        }

        public static async Task<Note> GetNote(this AppDbContext db, int id) =>
            await db.Notes
                .FindAsync(id);

        public static async Task AddNote(this AppDbContext db, Note note)
        {
            if (await note.Validate(db))
            {
                await db.Notes.AddAsync(note);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateNote(this AppDbContext db, Note note)
        {
            if (await note.Validate(db))
            {
                db.Notes.Update(note);
                await db.SaveChangesAsync();
            }
        }
        public static async Task ShareNote(this AppDbContext db, int id, List<User> users) // this is for bonus points
        {
            var sharedNotes = users.Select(user => new SharedNote()
            {
                NoteId = id,
                UserId = user.Id
            });

            await db.SharedNotes.AddRangeAsync(sharedNotes);
            await db.SaveChangesAsync();
        }
        public static async Task RemoveNote(this AppDbContext db, Note note)
        {
            if (await note.Validate(db))
            {
                db.Notes.Remove(note);
                await db.SaveChangesAsync();
            }
        }
        public static async Task UnshareNote(this AppDbContext db, SharedNote note)
        {
            db.SharedNotes.Remove(note);
            await db.SaveChangesAsync();
        }

        // possibly need another validate for sharing

        static async Task<bool> Validate(this Note note, AppDbContext db)
        {
            if (string.IsNullOrEmpty(note.Title))
            {
                throw new AppException("Note must have a name", ExceptionType.Validation);
            }

            var check = await db.Notes
                .FirstOrDefaultAsync(x =>
                    x.Id != note.Id &&
                    x.Title.ToLower() == note.Title.ToLower()
                );

            if (check != null)
            {
                throw new AppException($"{note.Title} is already a Note", ExceptionType.Validation);
            }

            return true;
        }
    }
}