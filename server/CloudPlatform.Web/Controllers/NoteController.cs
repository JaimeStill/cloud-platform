using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using CloudPlatform.Data;
using CloudPlatform.Data.Entities;
using CloudPlatform.Data.Extensions;

namespace CloudPlatform.Web.Controllers
{
    [Route("api/[controller]")]
    public class NoteController : Controller
    {
        private AppDbContext db;

        public NoteController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]/{folderId}")]
        public async Task<List<Note>> GetNotes([FromRoute]int folderId) => await db.GetNotes(folderId);

        [HttpGet("[action]/{owner}/{path}")]
        public async Task<Note> GetNote([FromRoute]string owner, [FromRoute]string path) => await db.GetNote(owner, path);

        [HttpPost("[action]")]
        public async Task<bool> ValidateNoteName([FromBody]Note note) => await db.ValidateNoteName(note);

        [HttpPost("[action]")]
        public async Task AddNote([FromBody] Note note) => await db.AddNote(note);

        [HttpPost("[action]")]
        public async Task UpdateNote([FromBody] Note note) => await db.UpdateNote(note);

        [HttpPost("[action]")]
        public async Task RemoveNote([FromBody] Note note) => await db.RemoveNote(note);

        [HttpPost("[action]/{noteId}")]
        public async Task ShareNote([FromRoute]int noteId, [FromBody] List<User> users) =>
            await db.ShareNote(noteId, users);

        [HttpGet("[action]/{noteId}/{username}")]
        public async Task UnshareNote([FromRoute]int noteId, [FromRoute]string username) => await db.UnshareNote(noteId, username);
    }
}