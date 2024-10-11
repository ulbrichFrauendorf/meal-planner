import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WithDestroy } from '@app/_core/mixins/with-destroy-mixin';
import { DataUpdateEventService } from '@app/_core/services/data-update-event/data-update-event.service';
import { DialogActionsComponent } from '@app/features/shared/components/dialog-actions/dialog-actions.component';
import {
    AddRecipeIngredientCommand,
    IngredientDto,
    IngredientsClient,
    RecipesClient,
} from '@app/web-api-client';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { ListboxModule } from 'primeng/listbox';
import { Observable, takeUntil } from 'rxjs';

@Component({
    selector: 'app-recipe-add-ingredients-dialog',
    standalone: true,
    imports: [
        ListboxModule,
        CommonModule,
        FormsModule,
        DialogModule,
        ButtonModule,
        DialogActionsComponent,
    ],
    templateUrl: './recipe-add-ingredients-dialog.component.html',
    styleUrl: './recipe-add-ingredients-dialog.component.scss',
})
export class RecipeAddIngredientsDialogComponent
    extends WithDestroy()
    implements OnInit
{
    recipeId: string = '';

    current$: Observable<string>;
    ingredients$: Observable<IngredientDto[]>;
    selectedIngredients: IngredientDto[];

    constructor(
        private ingredientsClient: IngredientsClient,
        private recipeClient: RecipesClient,
        private messageService: MessageService,
        private dataUpdateEventService: DataUpdateEventService,
        public ref: DynamicDialogRef,
        public config: DynamicDialogConfig
    ) {
        super();
    }

    ngOnInit(): void {
        this.recipeId = this.config?.data?.recipeId;
        this.ingredients$ = this.ingredientsClient.getAllIngredients();
    }

    onCancel() {
        this.ref.close();
    }

    onSubmit() {
        if (this.selectedIngredients?.length > 0) {
            this.recipeClient
                .addRecipeIngredients({
                    recipeId: this.recipeId,
                    ingredientIds: this.selectedIngredients.map((x) => x.id),
                } as AddRecipeIngredientCommand)
                .pipe(takeUntil(this.destroy$))
                .subscribe({
                    complete: () => {
                        this.messageService.add({
                            severity: 'success',
                            summary: 'Ingredient Link Success',
                            detail: 'The selected ingredients have been linked.',
                            key: 'global',
                            life: 3000,
                        });
                        this.dataUpdateEventService.notifyDataUpdated();
                        this.ref.close();
                    },
                });
        }
    }
}
