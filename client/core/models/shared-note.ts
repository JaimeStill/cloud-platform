import { Note } from './note';
import { User } from './user';

export interface SharedNote {
  id: number;
  noteId: number;
  userId: number;

  note: Note;
  user: User;
}
