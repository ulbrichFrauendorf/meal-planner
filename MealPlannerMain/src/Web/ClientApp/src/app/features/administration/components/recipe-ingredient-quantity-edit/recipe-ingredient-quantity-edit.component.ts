import { Component, OnInit } from '@angular/core';
import { DialogActionsComponent } from '../../../shared/components/dialog-actions/dialog-actions.component';
import { KnobModule } from 'primeng/knob';
import { WithDestroy } from '@app/_core/mixins/with-destroy-mixin';
import { DataUpdateEventService } from '@app/_core/services/data-update-event/data-update-event.service';
import {
    RecipeIngredientsClient,
    UpdateRecipeIngredientCommand,
} from '@app/web-api-client';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { takeUntil } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-recipe-ingredient-quantity-edit',
    standalone: true,
    imports: [DialogActionsComponent, KnobModule, FormsModule],
    templateUrl: './recipe-ingredient-quantity-edit.component.html',
    styleUrl: './recipe-ingredient-quantity-edit.component.scss',
})
export class RecipeIngredientQuantityEditComponent
    extends WithDestroy()
    implements OnInit
{
    recipeIngredientId: string = '';
    quantity: number = 0;

    constructor(
        private recipeIngredientsClient: RecipeIngredientsClient,
        private messageService: MessageService,
        private dataUpdateEventService: DataUpdateEventService,
        public ref: DynamicDialogRef,
        public config: DynamicDialogConfig
    ) {
        super();
    }

    ngOnInit(): void {
        this.recipeIngredientId = this.config?.data?.recipeIngredientId;
        this.quantity = this.config?.data?.quantity;
    }

    onCancel() {
        this.ref.close();
    }

    onSubmit() {
        if (this.recipeIngredientId) {
            this.recipeIngredientsClient
                .updateRecipeIngredient(this.recipeIngredientId, {
                    recipeIngredientId: this.recipeIngredientId,
                    quantity: this.quantity,
                } as UpdateRecipeIngredientCommand)
                .pipe(takeUntil(this.destroy$))
                .subscribe({
                    complete: () => {
                        this.messageService.add({
                            severity: 'success',
                            summary: 'Ingredient Quantity Updated',
                            detail: "The selected ingredient's quantity have been updated.",
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
