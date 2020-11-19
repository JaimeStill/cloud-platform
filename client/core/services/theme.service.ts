import { Injectable } from '@angular/core';
import { OverlayContainer } from '@angular/cdk/overlay';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  useDarkTheme = false;

  toggleTheme = () => {
    this.useDarkTheme = !this.useDarkTheme;
    this.setOverlayContainerTheme();
  }

  constructor(
    private overlay: OverlayContainer
  ) {
    this.setOverlayContainerTheme();
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
