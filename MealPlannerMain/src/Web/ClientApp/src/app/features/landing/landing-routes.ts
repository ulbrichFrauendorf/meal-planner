import { Routes } from '@angular/router';
import { LandingDashboardComponent } from './pages/landing-dashboard/landing-dashboard.component';

export const LANDING_ROUTES: Routes = [
    {
        path: '',
        component: LandingDashboardComponent,
    },
    { path: '**', redirectTo: '/notfound' },
];
