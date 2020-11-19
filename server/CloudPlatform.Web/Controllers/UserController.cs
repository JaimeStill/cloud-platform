using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using CloudPlatform.Data;
using CloudPlatform.Data.Entities;
using CloudPlatform.Data.Extensions;

namespace CloudPlatform.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private AppDbContext db;

        public UserController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        public async Task<List<User>> GetUsers() => await db.GetUsers();

        [HttpGet("[action]/{search}")]
        public async Task<List<User>> SearchUsers([FromRoute]string search) => await db.SearchUsers(search);

        [HttpGet("[action]/{username}")]
        public async Task<User> GetUser([FromRoute]string userName) => await db.GetUser(userName);

        [HttpPost("[action]")]
        public async Task<bool> ValidateUsername([FromBody]User user) => await db.ValidateUsername(user);

        [HttpPost("[action]")]
        public async Task AddUser([FromBody] User User) => await db.AddUser(User);

        [HttpPost("[action]")]
        public async Task UpdateUser([FromBody] User User) => await db.UpdateUser(User);

        [HttpPost("[action]")]
        public async Task RemoveUser([FromBody] User User) => await db.RemoveUser(User);
    }
}