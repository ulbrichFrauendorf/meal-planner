import { Routes } from '@angular/router';
import { AdministrationLandingComponent } from './pages/administration-landing/administration-landing.component';
import { IngredientsComponent } from './pages/ingredients/ingredients.component';
import { RecipesComponent } from './pages/recipes/recipes.component';

export const ADMINISTRATION_ROUTES: Routes = [
    {
        path: '',
        component: AdministrationLandingComponent,
        children: [
            {
                path: 'ingredients',
                component: IngredientsComponent,
            },
            {
                path: 'recipes',
                component: RecipesComponent,
            },
        ],
    },
    { path: '**', redirectTo: '/notfound' },
];
