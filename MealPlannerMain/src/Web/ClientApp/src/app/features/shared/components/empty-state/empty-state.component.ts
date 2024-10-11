import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { LayoutService } from 'app/layout/service/app.layout.service';

@Component({
    selector: 'app-empty-state',
    standalone: true,
    imports: [NgIf, AsyncPipe, NgClass],
    templateUrl: './empty-state.component.html',
    styleUrl: './empty-state.component.scss',
})
export class EmptyStateComponent {
    constructor(public layoutService: LayoutService) {}

    @Input() fixed: boolean = false;

    imageLoaded: boolean = false;

    onImageLoad() {
        setTimeout(() => {
            this.imageLoaded = true;
        }, 200); // Delay before marking as loaded
    }
}
