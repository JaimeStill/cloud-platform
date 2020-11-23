using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CloudPlatform.Data.Entities;
using CloudPlatform.Core.Extensions;

namespace CloudPlatform.Data.Extensions
{
    public static class NoteExtensions
    {
        public static async Task<List<Note>> GetNotes(this AppDbContext db, int folderId)
        {
            var notes = await db.Notes
                .Where(x => x.FolderId == folderId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return notes;
        }

        public static async Task<Note> GetNote(this AppDbContext db, string owner, string path) =>
            await db.Notes
                .FirstOrDefaultAsync(x =>
                    x.Folder.Owner == owner &&
                    x.Path == path
                );

        public static async Task<bool> ValidateNoteName(this AppDbContext db, Note note)
        {
            var check = await db.Notes
                .FirstOrDefaultAsync(x =>
                    x.FolderId == note.FolderId &&
                    x.Id != note.Id &&
                    x.Name == note.Name.UrlEncode()
                );

            return check is null;
        }

        public static async Task AddNote(this AppDbContext db, Note note)
        {
            if (await note.Validate(db))
            {
                note.Path = await note.GeneratePath(db);

                await db.Notes.AddAsync(note);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateNote(this AppDbContext db, Note note)
        {
            if (await note.Validate(db))
            {
                note.Path = await note.GeneratePath(db);

                db.Notes.Update(note);
                await db.SaveChangesAsync();
            }
        }
        public static async Task RemoveNote(this AppDbContext db, Note note)
        {
            note.RemoveSharedNotes(db);
            db.Notes.Remove(note);
            await db.SaveChangesAsync();
        }

        public static async Task ShareNote(this AppDbContext db, int noteId, List<User> users) // this is for bonus points
        {
            var names = await db.GetSharedNoteUsers(noteId);

            var sharedNotes = users
                .Where(x => !names.Contains(x.Username))
                .Select(user => new SharedNote()
                {
                    NoteId = noteId,
                    Username = user.Username
                });

            await db.SharedNotes.AddRangeAsync(sharedNotes);
            await db.SaveChangesAsync();
        }

        public static async Task UnshareNote(this AppDbContext db, int noteId, string username)
        {
            var note = await db.SharedNotes
                .FirstOrDefaultAsync(x =>
                    x.NoteId == noteId &&
                    x.Username == username
                );

            if (note is not null)
            {
                db.SharedNotes.Remove(note);
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"{username} does not have access to the specified note");
            }
        }

        // possibly need another validate for sharing

        static async Task<bool> Validate(this Note note, AppDbContext db)
        {
            if (string.IsNullOrEmpty(note.Name))
            {
                throw new Exception("Note must have a name");
            }

            if (!await db.ValidateNoteName(note))
            {
                throw new Exception($"{note.Name} is already a Note in this directory");
            }

            return true;
        }

        static async Task<string> GeneratePath(this Note note, AppDbContext db) => $"{await db.GetBreadcrumbs(note.FolderId)}/{note.Name}.md";

        static void RemoveSharedNotes(this Note note, AppDbContext db) =>
            db.SharedNotes.RemoveRange(
                db.SharedNotes.Where(x => x.NoteId == note.Id)
            );
    }
}