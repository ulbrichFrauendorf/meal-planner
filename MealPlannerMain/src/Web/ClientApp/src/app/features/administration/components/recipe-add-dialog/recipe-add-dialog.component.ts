import { Component } from '@angular/core';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DialogActionsComponent } from '@app/features/shared/components/dialog-actions/dialog-actions.component';
import {
    Validators,
    FormBuilder,
    FormsModule,
    ReactiveFormsModule,
} from '@angular/forms';
import { WithDestroy } from '@app/_core/mixins/with-destroy-mixin';
import { DataUpdateEventService } from '@app/_core/services/data-update-event/data-update-event.service';
import { RecipesClient, AddRecipeCommand } from '@app/web-api-client';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { takeUntil } from 'rxjs';
import { NgIf } from '@angular/common';
import { KnobModule } from 'primeng/knob';

@Component({
    selector: 'app-recipe-add-dialog',
    standalone: true,
    imports: [
        NgIf,
        FloatLabelModule,
        InputTextModule,
        ButtonModule,
        DialogModule,
        KnobModule,
        DialogActionsComponent,
        FormsModule,
        ReactiveFormsModule,
    ],
    templateUrl: './recipe-add-dialog.component.html',
    styleUrl: './recipe-add-dialog.component.scss',
})
export class RecipeAddDialogComponent extends WithDestroy() {
    recipeForm = this.fb.group({
        name: ['', Validators.required],
        peopleFed: [1, Validators.required],
    });

    constructor(
        private fb: FormBuilder,
        private recipesClient: RecipesClient,
        private messageService: MessageService,
        private dataUpdateEventService: DataUpdateEventService,
        public ref: DynamicDialogRef
    ) {
        super();
    }

    onCancel() {
        this.ref.close();
    }

    onSubmit() {
        this.recipesClient
            .addRecipe(
                new AddRecipeCommand({
                    name: this.recipeForm.value.name,
                    peopleFed: this.recipeForm.value.peopleFed,
                })
            )
            .pipe(takeUntil(this.destroy$))
            .subscribe({
                complete: () => {
                    this.messageService.add({
                        severity: 'success',
                        summary: 'Recipe Add Success',
                        detail: 'New recipe has been added.',
                        key: 'global',
                        life: 3000,
                    });

                    this.dataUpdateEventService.notifyDataUpdated();
                    this.ref.close();
                },
            });
    }
}
