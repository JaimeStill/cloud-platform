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

        [HttpGet("[action]/{owner}")]
        public async Task<List<Folder>> GetRootFolders([FromRoute]string owner) => await db.GetRootFolders(owner);

        [HttpGet("[action]/{owner}")]
        public async Task<List<Folder>> GetRootSharedFolders([FromRoute]string owner) => await db.GetRootSharedFolders(owner);

        [HttpGet("[action]/{folderId}")]
        public async Task<List<Folder>> GetSubFolders([FromRoute]int folderId) => await db.GetSubFolders(folderId);

        [HttpGet("[action]/{owner}/{path}")]
        public async Task<Folder> GetFolder([FromRoute]string owner, [FromRoute]string path) => await db.GetFolder(owner, path);

        [HttpPost("[action]")]
        public async Task<bool> ValidateFolderName([FromBody]Folder folder) => await db.ValidateFolderName(folder);

        [HttpPost("[action]")]
        public async Task AddFolder([FromBody] Folder Folder) => await db.AddFolder(Folder);

        [HttpPost("[action]")]
        public async Task UpdateFolder([FromBody] Folder Folder) => await db.UpdateFolder(Folder);

        [HttpPost("[action]")]
        public async Task RemoveFolder([FromBody] Folder Folder) => await db.RemoveFolder(Folder);

        [HttpPost("[action]/{folderId}")]
        public async Task ShareFolder([FromRoute]int folderId, [FromBody] List<User> users) =>
            await db.ShareFolder(folderId, users);

        [HttpGet("[action]/{folderId}/{username}")]
        public async Task UnshareFolder([FromRoute]int folderId, [FromRoute]string username) => await db.UnshareFolder(folderId, username);
    }
}