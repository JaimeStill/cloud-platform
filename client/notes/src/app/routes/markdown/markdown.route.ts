import {
  Component,
  OnInit
} from '@angular/core';

import {
  DomSanitizer,
  SafeHtml
} from '@angular/platform-browser';

import {
  MarkedService,
  SnippetService,
  UserService
} from 'core';

@Component({
  selector: 'markdown-route',
  templateUrl: 'markdown.route.html'
})
export class MarkdownRoute implements OnInit {
  content: SafeHtml
  constructor(
    private sanitizer: DomSanitizer,
    public markedSvc: MarkedService,
    public snippetSvc: SnippetService,
    public userSvc: UserService
  ) { }

  ngOnInit() {
    this.userSvc.currentUser$.subscribe(user => user && this.snippetSvc.setSnippet(JSON.stringify(user, null, 2), 'json'));
  }

  syncMarkdown = (md: string) => this.content = this.sanitizer.bypassSecurityTrustHtml(this.markedSvc.convert(md, 'Editor'));
}
