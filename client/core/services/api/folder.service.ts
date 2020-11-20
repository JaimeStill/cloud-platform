import {
  Injectable,
  Optional
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { SnackerService } from '../snacker.service';
import { ServerConfig } from '../../config';
import { Folder } from 'client/core/models';

@Injectable()
export class FolderService {
  private folders = new BehaviorSubject<Folder[]>(null);
  private folder = new BehaviorSubject<Folder>(null);

  folders$ = this.folders.asObservable();
  folder$ = this.folder.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ServerConfig
  ) { }

  getFolders = (userId: number) => this.http.get<Folder[]>(`${this.config.api}folder/getFolders/${userId}`)
    .subscribe(
      data => this.folders.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    )

  searchFolders = (userId: number, search: string) =>
    this.http.get<Folder[]>(`${this.config.api}folder/searchFolders/${userId}/${search}`)
      .subscribe(
        data => this.folders.next(data),
        err => this.snacker.sendErrorMessage(err.error)
      )

  getFolder = (id: number): Promise<Folder> => new Promise((resolve) => {
    this.http.get<Folder>(`${this.config.api}/folder/getFolder/${id}`)
      .subscribe(
        data => {
          this.folder.next(data);
          resolve(data);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(null);
        }
      );
  })

  addFolder = (folder: Folder): Promise<Folder> => new Promise((resolve) => {
    this.http.post<Folder>(`${this.config.api}/folder/addFolder`, folder)
      .subscribe(
        data => {
          this.snacker.sendSuccessMessage(`${folder.name} successfully created`);
          resolve(data);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(null);
        }
      );
  })

  updateFolder = (folder: Folder): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}/folder/updateFolder`, folder)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${folder.name} successfully updated`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  })

  removeFolder = (folder: Folder): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}folder/removeFolder`, folder)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${folder.name} successfully removed`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  })
}
