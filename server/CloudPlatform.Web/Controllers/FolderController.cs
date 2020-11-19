using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using CloudPlatform.Data;
using CloudPlatform.Data.Entities;
using CloudPlatform.Data.Extensions;

namespace CloudPlatform.Web.Controllers
{
    [Route("api/[controller]")]
    public class FolderController : Controller
    {
        private AppDbContext db;

        public FolderController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]/{id}")]
        public async Task<List<Folder>> GetFolders([FromRoute]int id) => await db.GetFolders(id);

        [HttpGet("[action]/{id}")]
        public async Task<Folder> GetFolder([FromRoute]int id) => await db.GetFolder(id);

        [HttpGet("[action]/{id}/{search}")]
        public async Task<List<Folder>> SearchFolders([FromRoute] int id, [FromRoute] string search) =>
            await db.SearchFolders(id, search);

        [HttpPost("[action]")]
        public async Task AddFolder([FromBody] Folder Folder) => await db.AddFolder(Folder);

        [HttpPost("[action]")]
        public async Task UpdateFolder([FromBody] Folder Folder) => await db.UpdateFolder(Folder);

        [HttpPost("[action]")]
        public async Task RemoveFolder([FromBody] Folder Folder) => await db.RemoveFolder(Folder);

        [HttpPost("[action]/{id}")]
        public async Task ShareFolder([FromRoute]int id, [FromBody] List<User> users) =>
            await db.ShareFolder(id, users);

        [HttpPost("[action]")]
        public async Task UnshareFolder([FromBody] SharedFolder Folder) => await db.UnshareFolder(Folder);
    }
}