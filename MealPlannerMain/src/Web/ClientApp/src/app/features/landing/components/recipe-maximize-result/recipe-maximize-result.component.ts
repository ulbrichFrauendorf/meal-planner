import { Component, Input, OnInit } from '@angular/core';
import { CalculationResult } from '@app/web-api-client';
import { TableModule } from 'primeng/table';
import { ChartModule } from 'primeng/chart';
import { FieldsetModule } from 'primeng/fieldset';

@Component({
    selector: 'app-recipe-maximize-result',
    standalone: true,
    imports: [TableModule, ChartModule, FieldsetModule],
    templateUrl: './recipe-maximize-result.component.html',
    styleUrl: './recipe-maximize-result.component.scss',
})
export class RecipeMaximizeResultComponent implements OnInit {
    @Input() data: CalculationResult;

    constructor() {}

    totalPeopleFed: number;
    chartData: unknown;
    chartOptions: unknown;

    backgroundColors = [
        '--orange-500',
        '--teal-500',
        '--blue-500',
        '--indigo-500',
        '--yellow-500',
        '--green-500',
        '--purple-500',
        '--pink-500',
    ];

    ngOnInit(): void {
        this.totalPeopleFed = this.data.peopleFed;

        const recipes = this.data.recipes;

        //Chart initialization
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        this.chartOptions = {
            plugins: {
                legend: {
                    labels: {
                        color: textColor,
                    },
                },
            },
        };

        this.chartData = {
            labels: recipes.map((recipe) => recipe.name),
            datasets: [
                {
                    data: recipes.map((recipe) => recipe.peopleFed),
                    backgroundColor: this.backgroundColors.map((color) => {
                        const hexColor = documentStyle.getPropertyValue(color);
                        const transparentColor = hexColor.concat('bf');
                        return transparentColor;
                    }),
                    hoverBackgroundColor: this.backgroundColors.map((color) => {
                        const hexColor = documentStyle.getPropertyValue(color);
                        const transparentColor = hexColor.concat('99');
                        return transparentColor;
                    }),
                },
            ],
        };
    }
}
