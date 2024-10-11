import { Component, inject } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';

import { BehaviorSubject, debounceTime, filter, tap } from 'rxjs';
import { AsyncPipe, NgIf } from '@angular/common';

@Component({
    selector: 'app-callback',
    templateUrl: './callback.component.html',
    styleUrl: './callback.component.scss',
    standalone: true,
    imports: [NgIf, AsyncPipe],
})
export class CallbackComponent {
    private readonly oidcSecurityService = inject(OidcSecurityService);
    public isLoading$ = new BehaviorSubject<boolean>(true);

    ngOnInit() {
        this.oidcSecurityService
            .checkAuth()
            .pipe(
                tap(() => {
                    this.isLoading$.next(true);
                }),
                filter((data) => !!data && !!data.userData?.sub)
            )
            .subscribe({
                error: () => this.oidcSecurityService.authorize(),
                complete: () => {
                    this.isLoading$.next(false);
                },
            });
    }
}
