import { Folder } from './folder';
import { User } from './user';

export interface SharedFolder {
  id: number;
  folderId: number;
  userId: number;

  folder: Folder;
  user: User;
}
