import { Routes } from '@angular/router';
import { ErrorComponent } from './error/error.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AccessComponent } from './access/access.component';

export const STATUS_ROUTES: Routes = [
   {
      path: 'error',
      component: ErrorComponent,
   },
   {
      path: 'unauthorized',
      component: AccessComponent,
   },
   {
      path: 'notfound',
      component: NotFoundComponent,
   },
   { path: '**', redirectTo: '/notfound' },
];
