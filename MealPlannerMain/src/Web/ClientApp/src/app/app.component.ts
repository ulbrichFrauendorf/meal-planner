import { Component, OnInit, inject } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { PrimeNGConfig } from 'primeng/api';
import { Router, RouterOutlet } from '@angular/router';
import { WithDestroy } from './_core/mixins/with-destroy-mixin';
import { ViewStateService } from './_core/services/view-state/view-state.service';
import { ToastModule } from 'primeng/toast';
import { BehaviorSubject, filter, tap } from 'rxjs';
import { LoaderService } from './features/shared/services/loader/loader.service';
import { LoadingSpinnerComponent } from './features/shared/components/loading-spinner/loading-spinner.component';
import { AsyncPipe, NgIf } from '@angular/common';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    standalone: true,
    imports: [
        RouterOutlet,
        ToastModule,
        LoadingSpinnerComponent,
        NgIf,
        AsyncPipe,
    ],
})
export class AppComponent extends WithDestroy() implements OnInit {
    constructor(
        private primengConfig: PrimeNGConfig,
        private oidcSecurityService: OidcSecurityService,
        private viewStateService: ViewStateService,
        private loaderService: LoaderService
    ) {
        super();
    }

    public isLoading$ = new BehaviorSubject<boolean>(true);
    private router = inject(Router);
    ngOnInit() {
        this.oidcSecurityService
            .checkAuth()
            .pipe(
                tap(() => {
                    this.loaderService.setLoading(true);
                    this.isLoading$.next(true);
                }),
                filter((data) => !!data && !!data.userData?.sub)
            )
            .subscribe({
                next: () => this.viewStateService.setCurrentUser(),
                error: () => {
                    this.router.navigate(['/']); // Redirect to the home page
                },
                complete: () => {
                    this.loaderService.setLoading(false);
                    this.isLoading$.next(false);
                },
            });

        this.primengConfig.ripple = true;
    }
}
