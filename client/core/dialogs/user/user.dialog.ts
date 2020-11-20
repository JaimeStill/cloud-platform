import {
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';

import {
  Component,
  AfterContentChecked,
  Inject,
  ViewChild,
  ElementRef
} from '@angular/core';

import {
  DomSanitizer,
  SafeHtml
} from '@angular/platform-browser';

import {
  MarkedService,
  SnippetService,
  UserService
} from '../../services';

import { OverlayContainer } from '@angular/cdk/overlay';
import { MatSliderChange } from '@angular/material/slider';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { User } from '../../models';

@Component({
  selector: 'user-dialog',
  templateUrl: 'user.dialog.html'
})
export class UserDialog implements AfterContentChecked {
  @ViewChild('editor') editor: ElementRef;
  example: SafeHtml;

  constructor(
    private dialogRef: MatDialogRef<UserDialog>,
    private markedSvc: MarkedService,
    private overlay: OverlayContainer,
    private sanitizer: DomSanitizer,
    private userSvc: UserService,
    public snippetSvc: SnippetService,
    @Inject(MAT_DIALOG_DATA) public user: User
  ) { }

  ngAfterContentChecked() {
    const md = '```json\n' + JSON.stringify(this.user, null, this.user.editorTabSpacing) + '\n```';
    if (this.editor) this.editor.nativeElement.value = md;
    this.example = this.sanitizer.bypassSecurityTrustHtml(this.markedSvc.convert(md, 'UserDialog'));
  }

  setTheme = (event: MatSlideToggleChange) => {
    this.user.useDarkTheme = event.checked;

    if (this.user.useDarkTheme) {
      this.overlay.getContainerElement().classList.remove('app-light');
      this.overlay.getContainerElement().classList.add('app-dark');
    } else {
      this.overlay.getContainerElement().classList.remove('app-dark');
      this.overlay.getContainerElement().classList.add('app-light');
    }
  }

  setPadding = (event: MatSliderChange) => this.user.editorPadding = event.value;

  setFontSize = (event: MatSliderChange) => this.user.editorFontSize = event.value;

  save = async () => {
    const res = this.userSvc.updateUser(this.user);

    if (res) {
      this.userSvc.setCachedUser(this.user);
      this.userSvc.setCurrentUser(this.user);
      this.dialogRef.close();
    }
  }
}
