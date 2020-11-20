import { Folder } from './folder';
import { SharedFolder } from './shared-folder';
import { SharedNote } from './shared-note';

export interface User {
  id: number;
  username: string;
  useDarkTheme: boolean;

  editorFontSize: number;
  editorPadding: number;
  editorTabSpacing: number;
  editorFont: string;
  snippetTheme: string;

  folders: Folder[];
  sharedFolders: SharedFolder[];
  sharedNotes: SharedNote[];
}
