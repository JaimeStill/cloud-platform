using System.Collections.Generic;

namespace CloudPlatform.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Theme { get; set; }

        public IEnumerable<Folder> Folders { get; set; }
        public IEnumerable<SharedFolder> SharedFolders { get; set; }
        public IEnumerable<SharedNote> SharedNotes { get; set; }
    }
}