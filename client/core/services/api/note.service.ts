import {
  Injectable,
  Optional
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { Note } from '../../models';
import { SnackerService } from '../snacker.service';
import { ServerConfig } from '../../config';

@Injectable()
export class NoteService {
  private notes = new BehaviorSubject<Note[]>(null);
  private note = new BehaviorSubject<Note>(null);

  notes$ = this.notes.asObservable();
  note$ = this.note.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ServerConfig
  ) { }

  getNotes = (folderId: number) => this.http.get<Note[]>(`${this.config.api}/note/getNotes/${folderId}`)
    .subscribe(
      data => this.notes.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    )

  searchNotes = (folderId: number, search: string) =>
    this.http.get<Note[]>(`${this.config.api}note/searchNotes/${folderId}/${search}`)
      .subscribe(
        data => this.notes.next(data),
        err => this.snacker.sendErrorMessage(err.error)
      )

  getNote = (id: number): Promise<Note> => new Promise((resolve) => {
    this.http.get<Note>(`${this.config.api}/note/getNote/${id}`)
      .subscribe(
        data => {
          this.note.next(data);
          resolve(data);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(null);
        }
      );
  })

  addNote = (note: Note): Promise<Note> => new Promise((resolve) => {
    this.http.post<Note>(`${this.config.api}note/addNote`, note)
      .subscribe(
        data => {
          this.snacker.sendSuccessMessage(`${note.title} successfully created`);
          resolve(data);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(null);
        }
      );
  })

  updateNote = (note: Note): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}/note/updateNote`, note)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${note.title} successfully updated`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  })

  removeNote = (note: Note): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}note/removeNote`, note)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${note.title} successfully removed`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  })
}
