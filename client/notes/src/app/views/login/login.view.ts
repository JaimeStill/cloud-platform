import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  ElementRef
} from '@angular/core';

import {
  ConfirmDialog,
  CoreService,
  ThemeService,
  UserService,
  User
} from 'core';

import { Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'login-view',
  templateUrl: 'login.view.html'
})
export class LoginView implements OnInit, OnDestroy {
  private sub: Subscription;
  private initialized = false;
  user = {} as User;
  validName: boolean;

  constructor(
    private core: CoreService,
    private dialog: MatDialog,
    public themeSvc: ThemeService,
    public userSvc: UserService
  ) { }

  @ViewChild('userInput', { static: false })
  set userInput(input: ElementRef) {
    if (input && !this.initialized) {
      this.sub = this.core.generateUrlInputObservable(input)
        .subscribe(async val => {
          if (val?.length > 0) {
            this.user.username = val;
            this.validName = await this.userSvc.validateUsername(this.user);
          } else {
            this.validName = null;
          }
        })
    }
  }

  ngOnInit() {
    this.userSvc.getUsers();
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  checkValid = () => this.validName === false ? true : false;

  addUser = async () => {
    if (this.validName) {
      const res = await this.userSvc.addUser(this.user);

      if (res) {
        this.userSvc.setCachedUser(res);
        this.userSvc.setCurrentUser(res);
      }
    }
  }

  selectUser = (user: User) => {
    this.userSvc.setCachedUser(user);
    this.userSvc.setCurrentUser(user);
  }

  removeUser = (user: User) => this.dialog.open(ConfirmDialog, {
    data: {
      title: `Remove ${user.username}`,
      content: `Are you sure you want to remove ${user.username}?`
    },
    disableClose: true,
    autoFocus: false,
    width: '800px'
  })
    .afterClosed()
    .subscribe(async result => {
      if (result) {
        const res = await this.userSvc.removeUser(user);
        res && this.userSvc.getUsers();
      }
    })
}
