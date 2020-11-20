import { Injectable } from '@angular/core';
import { OverlayContainer } from '@angular/cdk/overlay';
import { UserService } from './api';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  useDarkTheme = false;
  snippetTheme = 'snippet-default';

  toggleTheme = () => {
    this.useDarkTheme = !this.useDarkTheme;
    this.setOverlayContainerTheme();
  }

  constructor(
    private userSvc: UserService,
    private overlay: OverlayContainer
  ) {
    this.setOverlayContainerTheme();
    this.setOverlayContainerSnippetTheme(this.snippetTheme);

    this.userSvc.currentUser$.subscribe(user => {
      if (user) {
        this.useDarkTheme = user.useDarkTheme;
        this.setOverlayContainerTheme();

        const previousTheme = this.snippetTheme;
        this.snippetTheme = user.snippetTheme;
        this.setOverlayContainerSnippetTheme(this.snippetTheme, previousTheme);
      }
    })
  }

  setOverlayContainerSnippetTheme = (current: string, previous: string = null) => {
    if (previous)
      this.overlay.getContainerElement().classList.remove(previous);

    if (current)
      this.overlay.getContainerElement().classList.add(current);
  }

  setOverlayContainerTheme = () => {
    if (this.useDarkTheme) {
      this.overlay.getContainerElement().classList.remove('app-light');
      this.overlay.getContainerElement().classList.add('app-dark');
    } else {
      this.overlay.getContainerElement().classList.remove('app-dark');
      this.overlay.getContainerElement().classList.add('app-light');
    }
  }
}
