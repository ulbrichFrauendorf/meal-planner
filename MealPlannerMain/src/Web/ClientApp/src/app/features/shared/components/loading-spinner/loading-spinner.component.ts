import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { LoaderService } from '../../services/loader/loader.service';


@Component({
   selector: 'app-loading-spinner',
   standalone: true,
   imports: [CommonModule, ProgressSpinnerModule],
   templateUrl: './loading-spinner.component.html',
   styleUrl: './loading-spinner.component.scss',
})
export class LoadingSpinnerComponent {
   constructor(public loaderService: LoaderService) {}
}
