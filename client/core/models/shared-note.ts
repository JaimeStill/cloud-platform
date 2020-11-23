import { Note } from './note';
import { User } from './user';

export interface SharedNote {
  id: number;
  noteId: number;
  username: string;

  note: Note;
  user: User;
}
