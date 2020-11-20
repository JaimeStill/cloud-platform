import {
  Injectable,
  Optional
} from '@angular/core';

import { BehaviorSubject } from 'rxjs';
import { SnippetService } from './snippet.service';
import { ServerConfig } from '../../config';

import * as marked from 'marked';

@Injectable({
  providedIn: 'root'
})
export class MarkedService {
  private fonts = new BehaviorSubject<string[]>([
    'Andale Mono',
    'Cascadia Code',
    'Consolas',
    'Courier',
    'Courier New',
    'Fira Code Retina',
    'Lucida Console',
    'Lucida Sans Typewriter',
    'Monaco'
  ]);
  fonts$ = this.fonts.asObservable();

  private renderer = new marked.Renderer();
  private parser = marked;

  constructor(
    @Optional() private config: ServerConfig,
    private snippetSvc: SnippetService
  ) {
    this.renderer.code = function (code, lang) {
      code = this.options.highlight(code, lang);

      return lang
        ? `<pre class="language-${lang}"><code class="language-${lang}">${code}</code></pre>`
        : `<pre><code>${code}</code></pre>`;
    }

    this.parser.setOptions({
      baseUrl: this.config.server,
      renderer: this.renderer,
      highlight: (code, lang) => this.snippetSvc.highlight(code, lang || 'js'),
      gfm: true,
      smartLists: true
    });
  }

  private checkAnchor = (href: string): boolean => href.charAt(0) === '#';

  private checkRelative = (href: string): boolean =>
    href.charAt(0) === '.'
      || href.charAt(0) === '/'
      || this.checkAnchor(href);

  private setBreadcrumbs = (href: string, breadcrumbs: string[]) => `${breadcrumbs.join('/')}/${href}`;

  private renderImage = (src: string, title: string, alt: string): string => title
    ? `<img src="${src}" title="${title}" alt="${alt}" />`
    : `<img src="${src}" alt="${alt}" />`;

  private renderLink = (href: string, title: string, text: string) => title
    ? `<a href="${href}" title="${title}">${text}</a>`
    : `<a href="${href}">${text}</a>`;

  private updateImage = (breadcrumbs: string[]) => {
    if (breadcrumbs) {
      return (href: string, title: string, text: string) => {
        if (this.checkRelative(href)) {
          const link = `${this.config.server}${this.setBreadcrumbs(href, breadcrumbs)}`;
          return this.renderImage(link, title, text);
        } else {
          return this.renderImage(href, title, text);
        }
      }
    } else {
      return (href: string, title: string, text: string) => this.checkRelative(href)
        ? this.renderImage(`${this.config.server}${href}`, title, text)
        : this.renderImage(href, title, text);
    }
  }

  private updateLink = (doc: string, breadcrumbs: string[]) => {
    if (breadcrumbs) {
      return (href: string, title: string, text: string) => {
        if (this.checkRelative(href)) {
          const link = href.toLowerCase().endsWith('.md')
            ? `${this.setBreadcrumbs(href, breadcrumbs)}`
            : this.checkAnchor(href)
              ? `${this.setBreadcrumbs(`${doc}${href}`, breadcrumbs)}`
              : `${this.config.server}${this.setBreadcrumbs(`${href}`, breadcrumbs)}`;

          return this.renderLink(link, title, text);
        } else {
          return this.renderLink(href, title, text);
        }
      }
    } else {
      return (href: string, title: string, text: string) => {
        if (this.checkRelative(href)) {
          return href.toLowerCase().endsWith('.md')
            ? this.renderLink(href, title, text)
            : this.checkAnchor(href)
              ? this.renderLink(`${doc}${href}`, title, text)
              : this.renderLink(`${this.config.server}${href}`, title, text);
        } else {
          return this.renderLink(href, title, text);
        }
      }
    }
  }

  private updateRenderer = (doc: string, breadcrumbs: string[]) => {
    this.renderer.image = this.updateImage(breadcrumbs);
    this.renderer.link = this.updateLink(doc, breadcrumbs);
  }

  convert = (markdown: string, doc: string, breadcrumbs: string[] = null) => {
    this.updateRenderer(doc, breadcrumbs);
    return this.parser.parse(markdown);
  }
}
