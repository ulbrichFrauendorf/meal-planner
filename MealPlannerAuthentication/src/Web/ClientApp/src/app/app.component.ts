import { Component, inject, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { PrimeNGConfig } from 'primeng/api';
import { RouterOutlet } from '@angular/router';
import { ToastModule } from 'primeng/toast';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    standalone: true,
    imports: [RouterOutlet, ToastModule],
})
export class AppComponent implements OnInit {
    constructor(private primengConfig: PrimeNGConfig) {}

    private readonly oidcSecurityService = inject(OidcSecurityService);

    ngOnInit() {
        //this.oidcSecurityService.checkAuth().subscribe();
        this.primengConfig.ripple = true;
    }
}
