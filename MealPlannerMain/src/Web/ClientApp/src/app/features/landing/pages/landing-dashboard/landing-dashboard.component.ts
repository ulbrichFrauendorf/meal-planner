import { AsyncPipe, NgIf } from '@angular/common';
import { Component, inject, OnInit, Signal } from '@angular/core';
import { AccordionModule } from 'primeng/accordion';
import { IngredientStockInputComponent } from '../../components/ingredient-stock-input/ingredient-stock-input.component';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { FieldsetModule } from 'primeng/fieldset';
import { CalculationResult } from '@app/web-api-client';
import { OptimalRecipeInformationBusService } from '../../services/optimal-recipe-information-bus.service';
import { RecipeMaximizeResultComponent } from "../../components/recipe-maximize-result/recipe-maximize-result.component";

@Component({
    selector: 'app-landing-dashboard',
    standalone: true,
    templateUrl: './landing-dashboard.component.html',
    styleUrl: './landing-dashboard.component.scss',
    imports: [
    AsyncPipe,
    NgIf,
    FieldsetModule,
    AccordionModule,
    IngredientStockInputComponent,
    LoadingSpinnerComponent,
    RecipeMaximizeResultComponent
],
})
export class LandingDashboardComponent implements OnInit {
    constructor() {}
    calculationResult: Signal<CalculationResult>;

    expandedIdx: number[];

    optimalRecipeInformationBusService = inject(
        OptimalRecipeInformationBusService
    );

    ngOnInit(): void {
        this.calculationResult = this.optimalRecipeInformationBusService.calculationResult;
        this.expandedIdx = [0];
    }

    onActiveIndexChange() {
        this.expandedIdx = [];
    }
}
