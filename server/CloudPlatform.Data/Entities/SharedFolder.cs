using System.Collections.Generic;

namespace CloudPlatform.Data.Entities
{
    public class SharedFolder
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public int UserId { get; set; }

        public Folder Folder { get; set; }
        public User User { get; set; }
    }
}