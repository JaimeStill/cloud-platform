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

        [HttpGet("[action]/{id}")]
        public async Task<List<Note>> GetNotes([FromRoute]int id) => await db.GetNotes(id);

        [HttpGet("[action]/{id}")]
        public async Task<Note> GetNote([FromRoute]int id) => await db.GetNote(id);

        [HttpPost("[action]/{id}")]
        public async Task<List<Note>> SearchNotes([FromRoute] int id, [FromBody] string search) =>
            await db.SearchNotes(id, search);

        [HttpPost("[action]")]
        public async Task AddNote([FromBody] Note note) => await db.AddNote(note);

        [HttpPost("[action]")]
        public async Task UpdateNote([FromBody] Note note) => await db.UpdateNote(note);

        [HttpPost("[action]")]
        public async Task RemoveNote([FromBody] Note note) => await db.RemoveNote(note);

        [HttpPost("[action]/{id}")]
        public async Task ShareNote([FromRoute]int id, [FromBody] List<User> users) =>
            await db.ShareNote(id, users);

        [HttpPost("[action]")]
        public async Task UnshareNote([FromBody] SharedNote note) => await db.UnshareNote(note);
    }
}