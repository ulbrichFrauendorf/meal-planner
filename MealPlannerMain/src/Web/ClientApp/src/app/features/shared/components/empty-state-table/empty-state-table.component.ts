import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { LayoutService } from '@app/layout/service/app.layout.service';

@Component({
    selector: 'app-empty-state-table',
    standalone: true,
    imports: [NgIf, AsyncPipe, NgClass],
    templateUrl: './empty-state-table.component.html',
    styleUrl: './empty-state-table.component.scss',
})
export class EmptyStateTableComponent {
    constructor(public layoutService: LayoutService) {}

    imageLoaded: boolean = false;

    onImageLoad() {
        setTimeout(() => {
            this.imageLoaded = true;
        }, 200); // Delay before marking as loaded
    }
}
