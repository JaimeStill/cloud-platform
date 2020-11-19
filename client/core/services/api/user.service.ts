import {
  Injectable,
  Optional
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { SnackerService } from '../snacker.service';
import { ServerConfig } from '../../config';
import { User } from '../../models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private currentUser = new BehaviorSubject<User>(null);
  private users = new BehaviorSubject<User[]>(null);
  private user = new BehaviorSubject<User>(null);

  currentUser$ = this.currentUser.asObservable();
  users$ = this.users.asObservable();
  user$ = this.user.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ServerConfig
  ) { }

  setCurrentUser = (user: User) => this.currentUser.next(user);

  setCachedUser = (user: User) => localStorage.setItem('cp-current-user', user.username);
  getCachedUser = (): string | null => localStorage.getItem('cp-current-user');

  getUsers = () => this.http.get<User[]>(`${this.config.api}user/getUsers`)
    .subscribe(
      data => this.users.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    )

  searchUsers = (search: string) => this.http.get<User[]>(`${this.config.api}user/searchUsers/${search}`)
    .subscribe(
      data => this.users.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    )

  getUser = (username: string): Promise<User> => new Promise((resolve) => {
    this.http.get<User>(`${this.config.api}user/getUser/${username}`)
      .subscribe(
        data => {
          this.user.next(data);
          resolve(data);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(null);
        }
      );
  })

  validateUsername = (user: User): Promise<boolean> => new Promise((resolve) => {
    this.http.post<boolean>(`${this.config.api}user/validateUsername`, user)
      .subscribe(
        data => resolve(data),
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      )
  })

  addUser = (user: User): Promise<User> => new Promise((resolve) => {
    this.http.post<User>(`${this.config.api}user/addUser`, user)
      .subscribe(
        data => {
          this.snacker.sendSuccessMessage(`${user.username} successfully created`);
          resolve(data);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(null);
        }
      );
  })

  updateUser = (user: User): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}user/updateUser`, user)
      .subscribe(
        () => resolve(true),
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  })

  removeUser = (user: User): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}user/removeUser`, user)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${user.username} successfully removed`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  })
}
