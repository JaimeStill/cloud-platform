import {
  Component,
  Input
} from '@angular/core';

import {
  ThemeService,
  User,
  UserService
} from 'core';

@Component({
  selector: 'layout-view',
  templateUrl: 'layout.view.html'
})
export class LayoutView {
  @Input() user: User;

  constructor(
    public themeSvc: ThemeService,
    public userSvc: UserService
  ) { }

  toggleTheme = async () => {
    this.user.useDarkTheme = !this.user.useDarkTheme;
    const res = this.userSvc.updateUser(this.user);
    res && this.userSvc.setCurrentUser(this.user);
  }
}
