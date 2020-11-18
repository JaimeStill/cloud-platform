import {
  Injectable,
  ElementRef
} from '@angular/core';

import {
  fromEvent,
  Observable
} from 'rxjs';

import {
  debounceTime,
  distinctUntilChanged,
  map
} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CoreService {
  urlEncode = (value: string): string => {
    const regex = /[^a-zA-Z0-9-.]/gi;
    let newValue = value.replace(/\s/g, '-').toLowerCase();
    newValue = newValue.replace(regex, '');
    return newValue;
  }

  generateInputObservable = (input: ElementRef, debounce: number = 300): Observable<string> =>
    fromEvent(input.nativeElement, 'keyup')
      .pipe(
        debounceTime(debounce),
        map((event: any) => event.target.value),
        distinctUntilChanged()
      )

  generateUrlInputObservable = (input: ElementRef, debounce: number = 300): Observable<string> =>
    fromEvent(input.nativeElement, 'keyup')
      .pipe(
        debounceTime(debounce),
        map((event: any) => this.urlEncode(event.target.value)),
        distinctUntilChanged()
      )
}
