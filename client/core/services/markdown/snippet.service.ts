import {
  DomSanitizer,
  SafeHtml
} from '@angular/platform-browser';

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { SnippetTheme } from '../../models';

import * as prism from 'prismjs';

import 'prismjs/components/prism-bash';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-css';
import 'prismjs/components/prism-javascript';
import 'prismjs/components/prism-json';
import 'prismjs/components/prism-markdown';
import 'prismjs/components/prism-markup';
import 'prismjs/components/prism-powershell';
import 'prismjs/components/prism-scss';
import 'prismjs/components/prism-sql';
import 'prismjs/components/prism-typescript';
import 'prismjs/components/prism-yaml';

@Injectable({
  providedIn: 'root'
})
export class SnippetService {
  private themes = new BehaviorSubject<SnippetTheme[]>([
    { label: 'Coy', css: 'snippet-coy' },
    { label: 'Dark', css: 'snippet-dark' },
    { label: 'Funky', css: 'snippet-funky' },
    { label: 'Okaidia', css: 'snippet-okaidia' },
    { label: 'Solarized Light', css: 'snippet-solarizedlight' },
    { label: 'Tomorrow', css: 'snippet-tomorrow' },
    { label: 'Twilight', css: 'snippet-twilight' },
    { label: 'Default', css: 'snippet-default' },
    { label: 'a11y Dark', css: 'snippet-a11y-dark' },
    { label: 'Atom Dark', css: 'snippet-atom-dark' },
    { label: 'Atelier Sulphur Pool', css: 'snippet-base16-ateliersulphurpool' },
    { label: 'Cb', css: 'snippet-cb' },
    { label: 'Coldark Cold', css: 'snippet-coldark-cold' },
    { label: 'Coldark Dark', css: 'snippet-coldark-dark' },
    { label: 'Coy (No Shadows)', css: 'snippet-coy-without-shadows' },
    { label: 'Darcula', css: 'snippet-darcula' },
    { label: 'Dracula', css: 'snippet-dracula' },
    { label: 'Duotone Dark', css: 'snippet-duotone-dark' },
    { label: 'Duotone Earth', css: 'snippet-duotone-earth' },
    { label: 'Duotone Forest', css: 'snippet-duotone-forest' },
    { label: 'Duotone Light', css: 'snippet-duotone-light' },
    { label: 'Duotone Sea', css: 'snippet-duotone-sea' },
    { label: 'Duotone Space', css: 'snippet-duotone-space' },
    { label: 'GitHub Colors', css: 'snippet-ghcolors' },
    { label: 'Hopscotch', css: 'snippet-hopscotch' },
    { label: 'Material Dark', css: 'snippet-material-dark' },
    { label: 'Material Light', css: 'snippet-material-light' },
    { label: 'Material Oceanic', css: 'snippet-material-oceanic' },
    { label: 'Nord', css: 'snippet-nord' },
    { label: 'Pojoaque', css: 'snippet-pojoaque' },
    { label: 'Shades of Purple', css: 'snippet-shades-of-purple' },
    { label: 'Synthwave 84', css: 'snippet-synthwave84' },
    { label: 'Visual Studio', css: 'snippet-vs' },
    { label: 'VS Code Dark+', css: 'snippet-vsc-dark-plus' },
    { label: 'Xonokai', css: 'snippet-xonokai' }
  ]);
  themes$ = this.themes.asObservable();

  private snippet = new BehaviorSubject<SafeHtml>(null);
  snippet$ = this.snippet.asObservable();

  constructor(
    private http: HttpClient,
    private sanitizer: DomSanitizer
  ) { }

  highlight = (code: string, lang: string): string => prism.highlight(code, prism.languages[lang || 'js'], lang || 'js');

  setSnippet = (code: string, lang: string) => this.snippet.next(
    this.sanitizer.bypassSecurityTrustHtml(this.highlight(code, lang))
  );

  getSnippet = (url: string, lang: string): Promise<SafeHtml> => new Promise((resolve, reject) => {
    this.http.get<string>(url, { responseType: 'text' } as Object)
      .subscribe(
        data => {
          this.setSnippet(data, lang);
          resolve(this.snippet.value);
        },
        err => {
          console.log(err);
          reject(err);
        }
      )
  })
}
