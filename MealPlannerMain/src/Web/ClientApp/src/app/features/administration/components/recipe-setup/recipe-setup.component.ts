import { Component, inject, Input, OnInit } from '@angular/core';
import { RecipeIngredientsComponent } from '../recipe-ingredients/recipe-ingredients.component';
import { TabMenuModule } from 'primeng/tabmenu';
import { NgIf } from '@angular/common';
import { MenuItem } from 'primeng/api';
import { PanelModule } from 'primeng/panel';
import { ActiveTabService } from '@app/_core/services/active-tab/active-tab.service';
import { FieldsetModule } from 'primeng/fieldset';

@Component({
    selector: 'app-recipe-setup',
    standalone: true,
    imports: [
        PanelModule,
        TabMenuModule,
        FieldsetModule,
        NgIf,
        RecipeIngredientsComponent,
    ],
    templateUrl: './recipe-setup.component.html',
    styleUrl: './recipe-setup.component.scss',
})
export class RecipeSetupComponent implements OnInit {
    @Input() recipeId: string = '';
    items: MenuItem[];
    activeItem: MenuItem | undefined;

    tabService = inject(ActiveTabService);

    ngOnInit() {
        this.items = [
            {
                label: 'Ingredients',
                icon: 'pi pi-book',
            },
        ];

        const activeIndex = this.tabService.getActiveTabIndex();

        this.activeItem = this.items[activeIndex];
    }

    onActiveItemChange(menuItem: MenuItem) {
        const activeIndex = this.items.indexOf(menuItem);
        this.tabService.setActiveTabIndex(activeIndex);
        this.activeItem = menuItem;
    }
}
