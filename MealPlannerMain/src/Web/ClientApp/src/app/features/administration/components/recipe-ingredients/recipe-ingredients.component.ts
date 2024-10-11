import { Component, inject, Input, OnInit, signal } from '@angular/core';
import { WithDestroy } from '@app/_core/mixins/with-destroy-mixin';
import { DataUpdateEventService } from '@app/_core/services/data-update-event/data-update-event.service';
import { RecipeIngredientDto, RecipesClient } from '@app/web-api-client';
import { MessageService, ConfirmationService } from 'primeng/api';
import { DynamicDialogRef, DialogService } from 'primeng/dynamicdialog';
import { Observable, switchMap, takeUntil } from 'rxjs';
import { RecipeAddIngredientsDialogComponent } from '../recipe-add-ingredients-dialog/recipe-add-ingredients-dialog.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EmptyStateTableComponent } from '@app/features/shared/components/empty-state-table/empty-state-table.component';
import { ConfirmationDialogComponent } from '@app/features/shared/components/confirmation-dialog/confirmation-dialog.component';
import { InputTextModule } from 'primeng/inputtext';
import { RecipeIngredientQuantityEditComponent } from '../recipe-ingredient-quantity-edit/recipe-ingredient-quantity-edit.component';

@Component({
    selector: 'app-recipe-ingredients',
    standalone: true,
    imports: [
        AsyncPipe,
        TableModule,
        ButtonModule,
        InputTextModule,
        FormsModule,
        EmptyStateTableComponent,
        ConfirmationDialogComponent,
    ],
    templateUrl: './recipe-ingredients.component.html',
    styleUrl: './recipe-ingredients.component.scss',
})
export class RecipeIngredientsComponent
    extends WithDestroy()
    implements OnInit
{
    @Input() recipeId: string = '';

    recipeIngredients$: Observable<RecipeIngredientDto[]>;
    selectedRecipeIngredient: RecipeIngredientDto;

    searchValue: string | undefined;
    isDialogVisible = signal(false);

    constructor(
        private recipesClient: RecipesClient,
        private dataUpdateEventService: DataUpdateEventService,
        private messageService: MessageService
    ) {
        super();
    }

    ngOnInit(): void {
        this.recipeIngredients$ = this.dataUpdateEventService.dataUpdated$.pipe(
            switchMap(() =>
                this.recipesClient.getRecipeIngredients(this.recipeId)
            )
        );
    }

    ref: DynamicDialogRef | undefined;
    dialogService = inject(DialogService);
    displayRecipeIngredientDialog() {
        this.ref = this.dialogService.open(
            RecipeAddIngredientsDialogComponent,
            {
                header: 'Add Ingredients To Recipe',
                width: '50rem',
                contentStyle: { overflow: 'auto' },
                breakpoints: {
                    '960px': '75vw',
                    '640px': '90vw',
                },
                data: {
                    recipeId: this.recipeId,
                },
            }
        );
    }

    openEditIngredientDialog(recipeIngredientId: string, quantity: number) {
        console.log('recipeIngredientId', recipeIngredientId);
        console.log('quantity', quantity);

        this.ref = this.dialogService.open(
            RecipeIngredientQuantityEditComponent,
            {
                header: 'Edit Ingredient Quantity',
                width: '50rem',
                contentStyle: { overflow: 'auto' },
                breakpoints: {
                    '960px': '75vw',
                    '640px': '90vw',
                },
                data: {
                    recipeIngredientId: recipeIngredientId,
                    quantity: quantity,
                },
            }
        );
    }

    confirmationService = inject(ConfirmationService);
    removeIngredientFromRecipe(recipeIngredientId: string) {
        if (recipeIngredientId) {
            this.confirmationService.confirm({
                message: 'Please confirm, to un-link user from recipe.',
                header: 'Are you sure?',

                accept: () => {
                    this.recipesClient
                        .removeRecipeIngredient(recipeIngredientId)
                        .pipe(takeUntil(this.destroy$))
                        .subscribe({
                            complete: () => {
                                this.messageService.add({
                                    severity: 'success',
                                    summary: 'Ingredient Removed',
                                    detail: 'The selected ingredient has been removed.',
                                    key: 'global',
                                    life: 3000,
                                });
                                this.dataUpdateEventService.notifyDataUpdated();
                            },
                        });
                },
            });
        }
    }
}
