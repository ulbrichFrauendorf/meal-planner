import {
    ChangeDetectorRef,
    Component,
    HostBinding,
    Input,
    OnInit,
} from '@angular/core';
import {
    NavigationEnd,
    Router,
    RouterLinkActive,
    RouterLink,
} from '@angular/router';
import {
    animate,
    state,
    style,
    transition,
    trigger,
} from '@angular/animations';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { MenuService } from './app.menu.service';
import { LayoutService } from './service/app.layout.service';
import { RippleModule } from 'primeng/ripple';
import { NgIf, NgClass, NgFor } from '@angular/common';
import { ClaimsClient } from 'app/web-api-client';
import { WithDestroy } from 'app/_core/mixins/with-destroy-mixin';

@Component({
    // eslint-disable-next-line @angular-eslint/component-selector
    selector: '[app-menuitem]',
    template: `
        <ng-container>
            <div
                *ngIf="root && item.visible !== false"
                class="layout-menuitem-root-text"
            >
                {{ item.label }}
            </div>
            <a
                *ngIf="
                    (!item.routerLink || item.items) && item.visible !== false
                "
                [attr.href]="item.url"
                (click)="itemClick($event)"
                [ngClass]="item.class"
                [attr.target]="item.target"
                tabindex="0"
                pRipple
            >
                <i [ngClass]="item.icon" class="layout-menuitem-icon"></i>
                <span class="layout-menuitem-text">{{ item.label }}</span>
                <i
                    class="pi pi-fw pi-angle-down layout-submenu-toggler"
                    *ngIf="item.items"
                ></i>
            </a>
            <a
                *ngIf="item.routerLink && !item.items && item.visible !== false"
                (click)="itemClick($event)"
                [ngClass]="item.class"
                [routerLink]="item.routerLink"
                routerLinkActive="active-route"
                [routerLinkActiveOptions]="
                    item.routerLinkActiveOptions || {
                        paths: 'exact',
                        queryParams: 'ignored',
                        matrixParams: 'ignored',
                        fragment: 'ignored'
                    }
                "
                [fragment]="item.fragment"
                [queryParamsHandling]="item.queryParamsHandling"
                [preserveFragment]="item.preserveFragment"
                [skipLocationChange]="item.skipLocationChange"
                [replaceUrl]="item.replaceUrl"
                [state]="item.state"
                [queryParams]="item.queryParams"
                [attr.target]="item.target"
                tabindex="0"
                pRipple
            >
                <i [ngClass]="item.icon" class="layout-menuitem-icon"></i>
                <span class="layout-menuitem-text">{{ item.label }}</span>
                <i
                    class="pi pi-fw pi-angle-down layout-submenu-toggler"
                    *ngIf="item.items"
                ></i>
            </a>

            <ul
                *ngIf="item.items && item.visible !== false"
                [@children]="submenuAnimation"
            >
                <ng-template
                    ngFor
                    let-child
                    let-i="index"
                    [ngForOf]="item.items"
                >
                    <li
                        app-menuitem
                        [item]="child"
                        [index]="i"
                        [parentKey]="key"
                        [class]="child.badgeClass"
                    ></li>
                </ng-template>
            </ul>
        </ng-container>
    `,
    animations: [
        trigger('children', [
            state(
                'collapsed',
                style({
                    height: '0',
                })
            ),
            state(
                'expanded',
                style({
                    height: '*',
                })
            ),
            transition(
                'collapsed <=> expanded',
                animate('400ms cubic-bezier(0.86, 0, 0.07, 1)')
            ),
        ]),
    ],
    standalone: true,
    imports: [NgIf, RippleModule, NgClass, RouterLinkActive, RouterLink, NgFor],
})
export class AppMenuitemComponent extends WithDestroy() implements OnInit {
    @Input() item: any;

    @Input() index!: number;

    @Input() @HostBinding('class.layout-root-menuitem') root!: boolean;

    @Input() parentKey!: string;

    @Input() userClaim: string | null;

    active = false;

    menuSourceSubscription: Subscription;

    menuResetSubscription: Subscription;

    key: string = '';

    constructor(
        public layoutService: LayoutService,
        private cd: ChangeDetectorRef,
        public router: Router,
        private menuService: MenuService,
        private claimsClient: ClaimsClient
    ) {
        super();
        this.menuSourceSubscription = this.menuService.menuSource$.subscribe(
            (value) => {
                Promise.resolve(null).then(() => {
                    if (value.routeEvent) {
                        this.active =
                            value.key === this.key ||
                            value.key.startsWith(this.key + '-')
                                ? true
                                : false;
                    } else {
                        if (
                            value.key !== this.key &&
                            !value.key.startsWith(this.key + '-')
                        ) {
                            this.active = false;
                        }
                    }
                });
            }
        );

        this.menuResetSubscription = this.menuService.resetSource$.subscribe(
            () => {
                this.active = false;
            }
        );

        this.router.events
            .pipe(filter((event) => event instanceof NavigationEnd))
            .subscribe(() => {
                if (this.item.routerLink) {
                    this.updateActiveStateFromRoute();
                }
            });
    }

    claimsSub: Subscription = new Subscription();

    updateVisibility() {
        if (this.claimsSub) {
            this.claimsSub.unsubscribe();
        }

        if (this.item.claim) {
            this.claimsSub = this.claimsClient
                .hasClaim(this.item.claim)
                .subscribe((hasClaim) => {
                    this.item.visible = hasClaim;
                    this.cd.detectChanges();
                });
        }
    }

    ngOnInit() {
        this.key = this.parentKey
            ? this.parentKey + '-' + this.index
            : String(this.index);

        if (this.item.routerLink) {
            this.updateActiveStateFromRoute();
        }

        if (this.claimsSub) {
            this.claimsSub.unsubscribe();
        }

        this.updateVisibility();
    }

    updateActiveStateFromRoute() {
        const activeRoute = this.router.isActive(this.item.routerLink[0], {
            paths: 'exact',
            queryParams: 'ignored',
            matrixParams: 'ignored',
            fragment: 'ignored',
        });

        if (activeRoute) {
            this.menuService.onMenuStateChange({
                key: this.key,
                routeEvent: true,
            });
        }
    }

    itemClick(event: Event) {
        // avoid processing disabled items
        if (this.item.disabled) {
            event.preventDefault();
            return;
        }

        // execute command
        if (this.item.command) {
            this.item.command({ originalEvent: event, item: this.item });
        }

        // toggle active state
        if (this.item.items) {
            this.active = !this.active;
        }

        this.menuService.onMenuStateChange({ key: this.key });
    }

    get submenuAnimation() {
        return this.root ? 'expanded' : this.active ? 'expanded' : 'collapsed';
    }

    @HostBinding('class.active-menuitem')
    get activeClass() {
        return this.active && !this.root;
    }
}
