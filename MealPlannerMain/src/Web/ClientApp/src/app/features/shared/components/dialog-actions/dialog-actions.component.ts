import { Component, EventEmitter, Input, Output } from '@angular/core';

import { ButtonModule } from 'primeng/button';

@Component({
    selector: 'app-dialog-actions',
    standalone: true,
    imports: [ButtonModule],
    templateUrl: './dialog-actions.component.html',
    styleUrl: './dialog-actions.component.scss',
})
export class DialogActionsComponent {
    @Input() submitLabel = 'Submit';
    @Output() cancel = new EventEmitter<void>();
    @Output() submit = new EventEmitter<void>();

    onCancel() {
        this.cancel.emit();
    }

    onSubmit() {
        this.submit.emit();
    }
}
