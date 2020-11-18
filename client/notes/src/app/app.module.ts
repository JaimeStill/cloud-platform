
import {
  FormsModule,
  ReactiveFormsModule
} from '@angular/forms';

import {
  CoreModule,
  MaterialModule
} from 'core';

import {
  RouteComponents,
  Routes
} from './routes';

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { environment } from '../environments/environment';
import { Views } from './views';

@NgModule({
  declarations: [
    AppComponent,
    ...RouteComponents,
    ...Views
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CoreModule.forRoot({ server: environment.server, api: environment.api }),
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    RouterModule.forRoot(Routes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
