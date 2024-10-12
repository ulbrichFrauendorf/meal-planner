import { AppLayoutComponent } from './layout/app.layout.component';
import { CallbackComponent } from './features/landing/pages/callback/callback.component';
import { AutoLoginPartialRoutesGuard } from 'angular-auth-oidc-client';
import { claimsGuard } from './_core/guards/claims.guard';
import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'home',
    },
    {
        path: '',
        component: AppLayoutComponent,
        children: [
            {
                path: 'home',
                loadChildren: () =>
                    import('./features/landing/landing-routes').then(
                        (m) => m.LANDING_ROUTES
                    ),
                canActivate: [AutoLoginPartialRoutesGuard, claimsGuard],
                data: { claim: 'mealplanner.user' },
            },
            {
                path: 'administration',
                loadChildren: () =>
                    import('@administration/administration-routes').then(
                        (m) => m.ADMINISTRATION_ROUTES
                    ),
                canActivate: [AutoLoginPartialRoutesGuard, claimsGuard],
                data: { claim: 'mealplanner.administrator' },
            },
        ],
    },
    {
        path: 'status',
        loadChildren: () =>
            import('./features/status/status-routes').then(
                (m) => m.STATUS_ROUTES
            ),
    },
    { path: 'callback', component: CallbackComponent },
    { path: '**', redirectTo: '/status/notfound' },
];
