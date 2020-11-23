using System;
using System.Collections.Generic;

namespace CloudPlatform.Data.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public Folder Folder { get; set; }

        public IEnumerable<SharedFolder> SharedFolders { get; set; }
        public IEnumerable<SharedNote> SharedNotes { get; set; }

    }
}