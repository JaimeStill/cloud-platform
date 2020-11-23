import { Note } from './note';
import { SharedFolder } from './shared-folder';
import { User } from './user';

export interface Folder {
  id: number;
  folderId?: number;
  name: string;
  owner: string;
  path: string;

  parent: Folder;
  user: User;

  folders: Folder[];
  notes: Note[];
  sharedFolders: SharedFolder[];
}
