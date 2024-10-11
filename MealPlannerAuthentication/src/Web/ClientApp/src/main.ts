import { enableProdMode, importProvidersFrom } from '@angular/core';

import { environment } from './environments/environment';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { APP_ROUTES } from './app/app-routing';
import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import { AuthorizeInterceptor } from './app/_core/interceptors/api-authorization/authorize.interceptor';
import { CsrfHeaderInterceptor } from './app/_core/interceptors/csrf-header/csrf-header.interceptor';
import {
    HTTP_INTERCEPTORS,
    provideHttpClient,
    withInterceptors,
} from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { provideAnimations } from '@angular/platform-browser/animations';
import { frontEndApiInterceptor } from './app/_core/interceptors/front-end-api/front-end-api.interceptor';
import { HasClaimDirective } from './app/_core/directives/has-claim.directive';
import { provideRouter, withRouterConfig } from '@angular/router';
import { AppSettingsService } from './app/_core/services/app-settings.service';
import { provideAuth, StsConfigLoader } from 'angular-auth-oidc-client';
import { httpLoaderFactory } from './app/_core/config/auth-config';

if (environment.production) {
    enableProdMode();
}

bootstrapApplication(AppComponent, {
    providers: [
        MessageService,
        provideHttpClient(withInterceptors([frontEndApiInterceptor])),
        AppSettingsService,

        provideAuth({
            loader: {
                provide: StsConfigLoader,
                useFactory: httpLoaderFactory,
                deps: [AppSettingsService],
            },
        }),
        importProvidersFrom(HasClaimDirective),
        provideRouter(
            APP_ROUTES,
            withRouterConfig({
                onSameUrlNavigation: 'reload',
                canceledNavigationResolution: 'replace',
                defaultQueryParamsHandling: 'replace',
            })
        ),
        {
            provide: HTTP_INTERCEPTORS,
            useClass: CsrfHeaderInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthorizeInterceptor,
            multi: true,
        },
        { provide: LocationStrategy, useClass: PathLocationStrategy },
        provideAnimations(),
    ],
}).catch((err) => console.error(err));
