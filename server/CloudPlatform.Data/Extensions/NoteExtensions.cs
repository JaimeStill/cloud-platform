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
        // public static async Task<List<Note>> SearchNotes(this AppDbContext db)

        public static async Task<List<Note>> GetNotes(this AppDbContext db, int folderId) =>
            await db.Notes
                .OrderBy(x => x.Title)
                .ToListAsync();

        // public static async Task<Note> GetNote(this AppDbContext db, int id) =>
        //     await db.Notes

    }
}