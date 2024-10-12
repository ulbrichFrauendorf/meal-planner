import { AsyncPipe } from '@angular/common';
import {
    Component,
    EventEmitter,
    inject,
    OnInit,
    Output,
    signal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WithDestroy } from '@app/_core/mixins/with-destroy-mixin';
import {
    GetOptimalRecipeInformationQuery,
    IngredientDto,
    IngredientsClient,
    RecipeOptimizerClient,
} from '@app/web-api-client';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { takeUntil, tap } from 'rxjs';
import { InputNumberModule } from 'primeng/inputnumber';
import { FieldsetModule } from 'primeng/fieldset';
import { OptimalRecipeInformationBusService } from '../../services/optimal-recipe-information-bus.service';
import { MessageService } from 'primeng/api';
import { LoaderService } from '@app/features/shared/services/loader/loader.service';

@Component({
    selector: 'app-ingredient-stock-input',
    standalone: true,
    imports: [
        AsyncPipe,
        TableModule,
        FormsModule,
        ButtonModule,
        InputNumberModule,
        FieldsetModule,
    ],
    templateUrl: './ingredient-stock-input.component.html',
    styleUrl: './ingredient-stock-input.component.scss',
})
export class IngredientStockInputComponent
    extends WithDestroy()
    implements OnInit
{
    @Output() activeIndexChange = new EventEmitter<void>();

    constructor() {
        super();
    }

    ingredients = signal<IngredientDto[]>([]);
    ingredientsClient = inject(IngredientsClient);

    ngOnInit(): void {
        this.ingredientsClient
            .getAllIngredients()
            .pipe(takeUntil(this.destroy$))
            .subscribe({
                next: (ingredients) => this.ingredients.set(ingredients),
            });
    }

    optimalRecipeInformationBusService = inject(
        OptimalRecipeInformationBusService
    );
    recipeOptimizerClient = inject(RecipeOptimizerClient);
    messageService = inject(MessageService);
    loaderService = inject(LoaderService);

    onSubmit() {
        this.recipeOptimizerClient
            .getOptimalRecipeInformation({
                availableIngredients: this.ingredients(),
            } as GetOptimalRecipeInformationQuery)
            .pipe(
                tap(() => this.loaderService.setLoading(true)),
                takeUntil(this.destroy$)
            )
            .subscribe({
                next: (result) => {
                    this.optimalRecipeInformationBusService.calculationResult.set(
                        result
                    );
                    this.activeIndexChange.emit();
                },
                complete: () => {
                    this.messageService.add({
                        severity: 'success',
                        summary: 'Calculation Success',
                        detail: 'Fimine has been minimized in the world.',
                        key: 'global',
                        life: 3000,
                    });
                    this.loaderService.setLoading(false);
                },
            });

        this.activeIndexChange.emit();
    }
}
