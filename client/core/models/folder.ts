import { Note } from './note';
import { SharedFolder } from './shared-folder';
import { User } from './user';

export interface Folder {
  id: number;
  userId: number;
  name: string;

  user: User;

  notes: Note[];
  sharedFolders: SharedFolder[];
}
