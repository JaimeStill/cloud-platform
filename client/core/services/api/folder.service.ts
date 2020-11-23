import {
  Injectable,
  Optional
} from '@angular/core';

import {
  Folder,
  User
} from '../../models';

import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { SnackerService } from '../snacker.service';
import { ServerConfig } from '../../config';

@Injectable()
export class FolderService {
  private folders = new BehaviorSubject<Folder[]>(null);
  private sharedFolders = new BehaviorSubject<Folder[]>(null);

  folders$ = this.folders.asObservable();
  sharedFolders$ = this.sharedFolders.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ServerConfig
  ) { }

  getRootFolders = (owner: string) => this.http.get<Folder[]>(`${this.config.api}folder/getRootFolders/${owner}`)
    .subscribe(
      data => this.folders.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  getRootSharedFolders = (owner: string) => this.http.get<Folder[]>(`${this.config.api}folder/getRootSharedFolders/${owner}`)
    .subscribe(
      data => this.sharedFolders.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  getSubFolders = (folderId: number) => this.http.get<Folder[]>(`${this.config.api}folder/getSubFolders/${folderId}`)
    .subscribe(
      data => this.folders.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  getFolder = (owner: string, path: string): Promise<Folder> => new Promise((resolve) => {
    this.http.get<Folder>(`${this.config.api}folder/getFolder/${owner}/${path}`)
      .subscribe(
        data => resolve(data),
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(null);
        }
      )
  })

  validateFolderName = (folder: Folder): Promise<boolean> => new Promise((resolve) => {
    this.http.post<boolean>(`${this.config.api}folder/validateFolderName`, folder)
      .subscribe(
        data => resolve(data),
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      )
  })

  addFolder = (folder: Folder): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}folder/addFolder`, folder)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${folder.name} successfully created`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      )
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

  shareFolder = (folder: Folder, users: User[]): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}folder/shareFolder/${folder.id}`, users)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${folder.name} successfully shared`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      )
  })

  unshareFolder = (folder: Folder, username: string): Promise<boolean> => new Promise((resolve) => {
    this.http.get(`${this.config.api}folder/unshareFolder/${folder.id}/${username}`)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${username} removed from ${folder.name}`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      )
  })
}
