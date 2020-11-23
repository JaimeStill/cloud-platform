using System.Collections.Generic;

namespace CloudPlatform.Data.Entities
{
    public class Folder
    {
        public int Id { get; set; }
        public int? FolderId { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Path { get; set; }

        public Folder Parent { get; set; }
        public User User { get; set; }

        public IEnumerable<Folder> Folders { get; set; }
        public IEnumerable<Note> Notes { get; set; }
        public IEnumerable<SharedFolder> SharedFolders { get; set; }
    }
}