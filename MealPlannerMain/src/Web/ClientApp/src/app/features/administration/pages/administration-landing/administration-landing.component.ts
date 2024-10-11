import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-administration-landing',
    standalone: true,
    imports: [RouterOutlet],
    templateUrl: './administration-landing.component.html',
    styleUrl: './administration-landing.component.scss',
})
export class AdministrationLandingComponent {}
