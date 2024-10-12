import { Component, OnInit, inject } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { ViewStateService } from 'app/_core/services/view-state/view-state.service';
import { LoaderService } from 'app/features/shared/services/loader/loader.service';
import { BehaviorSubject, filter, tap } from 'rxjs';
import { LoadingSpinnerComponent } from '@shared/components/loading-spinner/loading-spinner.component';
import { AsyncPipe, NgIf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
    selector: 'app-callback',
    templateUrl: './callback.component.html',
    styleUrl: './callback.component.scss',
    standalone: true,
    imports: [LoadingSpinnerComponent, NgIf, AsyncPipe],
})
export class CallbackComponent implements OnInit {
    private readonly oidcSecurityService = inject(OidcSecurityService);
    private readonly viewStateService = inject(ViewStateService);
    private readonly loaderService = inject(LoaderService);
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
                    this.viewStateService.setCurrentUser();
                },
            });
    }
}
