import { Component, Input } from '@angular/core';
import { CalculationResult } from '@app/web-api-client';

@Component({
    selector: 'app-recipe-maximize-result',
    standalone: true,
    imports: [],
    templateUrl: './recipe-maximize-result.component.html',
    styleUrl: './recipe-maximize-result.component.scss',
})
export class RecipeMaximizeResultComponent {
    @Input() data: CalculationResult;
}
