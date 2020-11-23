using System.Collections.Generic;

namespace CloudPlatform.Data.Entities
{
    public class SharedNote
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public string Username { get; set; }

        public Note Note { get; set; }
        public User User { get; set; }
    }
}