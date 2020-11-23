import { Folder } from './folder';
import { User } from './user';

export interface SharedFolder {
  id: number;
  folderId: number;
  username: string;

  folder: Folder;
  user: User;
}
