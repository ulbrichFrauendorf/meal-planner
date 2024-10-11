import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import {
    provideHttpClient,
    withInterceptors,
    HTTP_INTERCEPTORS,
} from '@angular/common/http';

import { bootstrapApplication } from '@angular/platform-browser';

import { AuthorizeInterceptor } from 'app/_core/interceptors/api-authorization/authorize.interceptor';
import { CsrfHeaderInterceptor } from 'app/_core/interceptors/csrf-header/csrf-header.interceptor';
import { frontEndApiInterceptor } from 'app/_core/interceptors/front-end-api/front-end-api.interceptor';
import { APP_ROUTES } from 'app/app-routes';
import { AppComponent } from 'app/app.component';
import { environment } from 'environments/environment.prod';
import { ConfirmationService, MessageService } from 'primeng/api';
import { provideAnimations } from '@angular/platform-browser/animations';
import { AuthConfigModule } from 'app/_core/config/auth-config.module';
import { DialogService } from 'primeng/dynamicdialog';
import { enableProdMode, importProvidersFrom } from '@angular/core';
import { provideRouter, withRouterConfig } from '@angular/router';

if (environment.production) {
    enableProdMode();
}

bootstrapApplication(AppComponent, {
    providers: [
        MessageService,
        ConfirmationService,
        DialogService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthorizeInterceptor,
            multi: true,
        },
        provideHttpClient(withInterceptors([frontEndApiInterceptor])),
        importProvidersFrom(AuthConfigModule),
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

        { provide: LocationStrategy, useClass: PathLocationStrategy },
        provideAnimations(),
    ],
}).catch((err) => console.error(err));
