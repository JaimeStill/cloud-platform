import {
  Component,
  OnInit
} from '@angular/core';

import { UserService } from 'core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  constructor(
    public userSvc: UserService
  ) { }

  private initializeUser = async (username: string) => {
    const user = await this.userSvc.getUser(username);
    if (user) {
      this.userSvc.setCachedUser(user);
      this.userSvc.setCurrentUser(user);
    }
  }

  ngOnInit() {
    const username = this.userSvc.getCachedUser();
    username && this.initializeUser(username);
  }
}
