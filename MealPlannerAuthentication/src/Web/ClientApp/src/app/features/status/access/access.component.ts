import { Component, inject } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { WithDestroy } from 'app/_core/mixins/with-destroy-mixin';
import { LayoutService } from 'app/layout/service/app.layout.service';

@Component({
    selector: 'app-access',
    templateUrl: './access.component.html',
    styleUrl: './access.component.scss',
    standalone: true,
    imports: [CardModule, ButtonModule],
})
export class AccessComponent extends WithDestroy() {
    constructor() {
        super();
    }
    logout() {
        this.authService.logoff().subscribe();
    }
    public layoutService = inject(LayoutService);
    public authService = inject(OidcSecurityService);
}
