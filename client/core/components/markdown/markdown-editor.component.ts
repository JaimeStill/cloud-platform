import {
  Component,
  AfterViewInit,
  OnDestroy,
  Input,
  Output,
  EventEmitter,
  ViewChild,
  ElementRef
} from '@angular/core';

import {
  fromEvent,
  Subscription
} from 'rxjs';

import {
  debounceTime,
  distinctUntilChanged,
  map
} from 'rxjs/operators';

@Component({
  selector: 'markdown-editor',
  templateUrl: 'markdown-editor.component.html',
  styleUrls: ['markdown-editor.component.css']
})
export class MarkdownEditorComponent implements AfterViewInit, OnDestroy {
  private sub: Subscription;

  @ViewChild('editor') editor: ElementRef;
  @Input() interval = 250;
  @Input() padding = 8;
  @Input() fontSize = 14;
  @Input() font = 'Cascadia Code';
  @Input() resize = false;
  @Input() tab = 2;
  @Output() sync = new EventEmitter<string>();

  constructor(

  ) { }

  ngAfterViewInit() {
    this.sub = fromEvent(this.editor.nativeElement, 'keyup')
      .pipe(
        debounceTime(this.interval),
        map((event: any) => event.target.value),
        distinctUntilChanged()
      )
      .subscribe(value => this.sync.emit(value))
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  checkInput = (event: any) => {
    switch (event.key) {
      case 'Tab':
        event.preventDefault();
        const start = event.target.selectionStart;
        const end = event.target.selectionEnd;
        const value = event.target.value;
        const spacing = ' '.repeat(this.tab);
        event.target.value = `${value.substring(0, start)}${spacing}${value.substring(end, value.length)}`;
        event.target.selectionStart = event.target.selectionEnd = start + spacing.length;
        break;
    }
  }
}
