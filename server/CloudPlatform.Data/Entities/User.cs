using System.Collections.Generic;

namespace CloudPlatform.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool UseDarkTheme { get; set; }

        // Preferences
        public int EditorPadding { get; set; }
        public int EditorFontSize { get; set; }
        public int EditorTabSpacing { get; set; }
        public string EditorFont { get; set; }
        public string SnippetTheme { get; set; }

        public IEnumerable<Folder> Folders { get; set; }
        public IEnumerable<SharedFolder> SharedFolders { get; set; }
        public IEnumerable<SharedNote> SharedNotes { get; set; }
    }
}