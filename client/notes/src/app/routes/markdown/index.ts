import { Route } from '@angular/router';
import { MarkdownRoute } from './markdown.route';

export const MarkdownComponents = [
  MarkdownRoute
];

export const MarkdownRoutes: Route[] = [
  { path: 'markdown', component: MarkdownRoute }
]
