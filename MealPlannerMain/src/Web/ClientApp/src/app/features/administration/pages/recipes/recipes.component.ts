import {
    Component,
    inject,
    OnInit,
    QueryList,
    ViewChild,
    ViewChildren,
} from '@angular/core';
import { RecipeSetupComponent } from '../../components/recipe-setup/recipe-setup.component';
import { RecipeAddDialogComponent } from '../../components/recipe-add-dialog/recipe-add-dialog.component';
import { CommonModule } from '@angular/common';
import { DataUpdateEventService } from '@app/_core/services/data-update-event/data-update-event.service';
import { ButtonModule } from 'primeng/button';
import { Table, TableModule } from 'primeng/table';
import { DynamicDialogRef, DialogService } from 'primeng/dynamicdialog';
import { Observable, switchMap } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { RecipeDto, RecipesClient } from '@app/web-api-client';

@Component({
    selector: 'app-recipes',
    standalone: true,
    imports: [
        CommonModule,
        TableModule,
        ButtonModule,
        InputTextModule,
        RecipeSetupComponent,
        FormsModule,
        RecipeAddDialogComponent,
    ],
    templateUrl: './recipes.component.html',
    styleUrl: './recipes.component.scss',
})
export class RecipesComponent implements OnInit {
    @ViewChild('recipeTable', { static: false })
    recipeTable: Table;

    @ViewChildren('recipeRow') recipeRows!: QueryList<any>;

    recipes$: Observable<RecipeDto[]>;
    expandedrecipe: { [key: string]: boolean } = {};
    searchValue: string | undefined;

    constructor(
        private recipesClient: RecipesClient,
        private dataUpdateEventService: DataUpdateEventService
    ) {}

    ngOnInit(): void {
        this.recipes$ = this.dataUpdateEventService.dataUpdated$.pipe(
            switchMap(() => {
                const recipe = this.recipesClient.getAllRecipes();
                return recipe;
            })
        );
    }

    onRowExpand(event: any) {
        const recipe = event.data;
        this.expandedrecipe = {};
        this.expandedrecipe[recipe.id] = true;
        this.scrollToRow(recipe.id);
    }

    scrollToRow(id: string) {
        setTimeout(() => {
            const rowElement = this.recipeRows.find(
                (row) => row.nativeElement.getAttribute('data-id') === id
            );

            if (rowElement) {
                const opt: ScrollToOptions = {
                    top: rowElement.nativeElement.offsetTop,
                };
                this.recipeTable.scrollTo(opt);
            }
        }, 0);
    }

    ref: DynamicDialogRef | undefined;
    dialogService = inject(DialogService);
    displayRecipeDialog() {
        this.ref = this.dialogService.open(RecipeAddDialogComponent, {
            header: 'Add New Recipe',
            width: '50rem',
            contentStyle: { overflow: 'auto' },
            breakpoints: {
                '960px': '75vw',
                '640px': '90vw',
            },
        });
    }
}
