import { Folder } from './folder';
import { SharedFolder } from './shared-folder';
import { SharedNote } from './shared-note';

export interface Note {
  id: number;
  folderId: number;
  title: string;
  value: string;
  dateCreated: Date;
  dateModified: Date;

  folder: Folder;

  sharedFolders: SharedFolder[];
  sharedNotes: SharedNote[];
}
