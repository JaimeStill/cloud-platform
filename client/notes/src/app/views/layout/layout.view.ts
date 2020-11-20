import {
  Component,
  Input
} from '@angular/core';

import {
  ThemeService,
  User,
  UserService,
  UserDialog
} from 'core';

import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'layout-view',
  templateUrl: 'layout.view.html'
})
export class LayoutView {
  @Input() user: User;

  constructor(
    private dialog: MatDialog,
    public themeSvc: ThemeService,
    public userSvc: UserService
  ) { }

  toggleTheme = async () => {
    this.user.useDarkTheme = !this.user.useDarkTheme;
    const res = this.userSvc.updateUser(this.user);
    res && this.userSvc.setCurrentUser(this.user);
  }

  setPreferences = () => this.dialog.open(UserDialog, {
    data: Object.assign({} as User, this.user),
    autoFocus: false,
    disableClose: true,
    width: '800px'
  });
}
