import { Route } from '@angular/router';

import {
  HomeComponents,
  HomeRoutes
} from './home';

import {
  MarkdownComponents,
  MarkdownRoutes
} from './markdown';

export const RouteComponents = [
  ...HomeComponents,
  ...MarkdownComponents
]

export const Routes: Route[] = [
  ...HomeRoutes,
  ...MarkdownRoutes,
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
]
