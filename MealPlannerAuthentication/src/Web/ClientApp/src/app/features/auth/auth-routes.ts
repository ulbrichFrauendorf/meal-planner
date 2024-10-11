import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { AutoLoginPartialRoutesGuard } from 'angular-auth-oidc-client';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';

export const AUTH_ROUTES: Routes = [
    { path: '', pathMatch: 'full', redirectTo: 'login' },
    {
        path: 'change-password',
        component: ChangePasswordComponent,
        canActivate: [AutoLoginPartialRoutesGuard],
    },
    {
        path: 'reset-password',
        component: ResetPasswordComponent,
    },
    {
        path: 'login',
        component: LoginComponent,
    },
    { path: '**', redirectTo: '/notfound' },
];
