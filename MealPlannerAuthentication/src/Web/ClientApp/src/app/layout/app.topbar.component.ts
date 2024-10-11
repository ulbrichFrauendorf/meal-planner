import {
    Component,
    ElementRef,
    inject,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MenuItem } from 'primeng/api';
import {
    LayoutService,
    LocalStorageColorSchemeKey,
    LocalStorageThemeKey,
} from './service/app.layout.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { NgClass, NgTemplateOutlet } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html',
    standalone: true,
    imports: [RouterLink, NgClass, NgTemplateOutlet],
})
export class AppTopBarComponent implements OnInit {
    private readonly oidcSecurityService = inject(OidcSecurityService);

    protected readonly authenticated = this.oidcSecurityService.authenticated;

    items!: MenuItem[];

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;

    isDark: boolean;

    constructor(public layoutService: LayoutService) {}

    ngOnInit(): void {
        this.isDark = this.colorScheme === 'dark';
    }

    login() {
        this.oidcSecurityService.authorize();
    }

    refreshSession() {
        this.oidcSecurityService.authorize();
    }

    logout() {
        this.oidcSecurityService.logoff().subscribe();
    }

    set colorScheme(val: string) {
        this.layoutService.config.update((config) => ({
            ...config,
            colorScheme: val,
        }));
    }
    get colorScheme(): string {
        return this.layoutService.config().colorScheme;
    }

    set theme(val: string) {
        this.layoutService.config.update((config) => ({
            ...config,
            theme: val,
        }));
    }
    get theme(): string {
        return this.layoutService.config().theme;
    }

    changeTheme() {
        this.isDark = !this.isDark;
        if (this.isDark) {
            this.theme = 'lara-dark-amber';
            this.colorScheme = 'dark';
        } else {
            this.theme = 'lara-light-amber';
            this.colorScheme = 'light';
        }

        localStorage.setItem(LocalStorageThemeKey, this.theme);
        localStorage.setItem(LocalStorageColorSchemeKey, this.colorScheme);
    }

    get scale(): number {
        return this.layoutService.config().scale;
    }
    set scale(_val: number) {
        this.layoutService.config.update((config) => ({
            ...config,
            scale: _val,
        }));
    }

    decrementScale() {
        this.scale--;
    }

    incrementScale() {
        this.scale++;
    }
}
