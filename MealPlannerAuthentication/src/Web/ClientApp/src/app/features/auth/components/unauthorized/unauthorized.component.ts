import { Component, inject } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
    selector: 'app-unauthorized',
    templateUrl: 'unauthorized.component.html',
    standalone: true,
})
export class UnauthorizedComponent {
    private readonly oidcSecurityService = inject(OidcSecurityService);

    constructor() {
        this.oidcSecurityService.authorize();
    }
}
