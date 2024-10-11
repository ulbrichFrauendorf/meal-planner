import { Routes } from '@angular/router';
import { AppLayoutComponent } from '@layout/app.layout.component';
import { CallbackComponent } from '@auth/components/callback/callback.component';
import { AutoLoginPartialRoutesGuard } from 'angular-auth-oidc-client';
import { AppLayoutMinimalComponent } from '@layout/app.layout-minimal.component';
import { claimsGuard } from './_core/guards/claims.guard';

export const APP_ROUTES: Routes = [
    { path: '', pathMatch: 'full', redirectTo: 'home' },

    {
        path: '',
        component: AppLayoutComponent,
        canActivate: [AutoLoginPartialRoutesGuard, claimsGuard],
        data: { claim: 'iserve.administrator' },
        children: [
            {
                path: 'home',
                loadChildren: () =>
                    import('@dashboard/landing-routes').then(
                        (m) => m.LANDING_ROUTES
                    ),
                canActivate: [AutoLoginPartialRoutesGuard],
            },
            {
                path: 'user-management',
                loadChildren: () =>
                    import('@users/user-management-routes').then(
                        (m) => m.USER_MANAGEMENT_ROUTES
                    ),
                canActivate: [AutoLoginPartialRoutesGuard],
            },
        ],
    },
    {
        path: '',
        component: AppLayoutMinimalComponent,
        children: [
            {
                path: 'auth',
                loadChildren: () =>
                    import('@auth/auth-routes').then((m) => m.AUTH_ROUTES),
            },
        ],
    },
    {
        path: 'status',
        loadChildren: () =>
            import('@status/status-routes').then((m) => m.STATUS_ROUTES),
    },
    {
        path: 'callback',
        component: CallbackComponent,
    },
    { path: '**', redirectTo: '/status/notfound' },
];
