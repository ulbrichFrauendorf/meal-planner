import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { AppMenuitemComponent } from './app.menuitem.component';
import { NgFor, NgIf } from '@angular/common';
import { HasClaimDirective } from 'app/_core/directives/has-claim.directive';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html',
    standalone: true,
    imports: [NgFor, NgIf, AppMenuitemComponent, HasClaimDirective],
})
export class AppMenuComponent implements OnInit {
    model: any[] = [];

    constructor() {}

    ngOnInit() {
        this.model = [
            {
                label: 'Home',
                claim: 'mealplanner.user',
                items: [
                    {
                        label: 'Dashboard',
                        icon: 'pi pi-fw pi-home',
                        routerLink: ['/'],
                        claim: 'mealplanner.user',
                    },
                ],
            },
            {
                label: 'Administration',
                claim: 'mealplanner.administrator',
                items: [
                    {
                        label: 'Ingredients',
                        icon: 'pi pi-fw pi-id-card',
                        routerLink: ['administration/ingredients'],
                    },
                    {
                        label: 'Recipes',
                        icon: 'pi pi-fw pi-id-card',
                        routerLink: ['administration/recipes'],
                    },
                ],
            },
        ];
    }
}
