using System.Collections.Generic;

namespace CloudPlatform.Data.Entities
{
    public class Folder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public User User { get; set; }

        public IEnumerable<Note> Notes { get; set; }
        public IEnumerable<SharedFolder> SharedFolders { get; set; }
    }
}