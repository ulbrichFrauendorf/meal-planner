import {
    Directive,
    Input,
    OnChanges,
    TemplateRef,
    ViewContainerRef,
} from '@angular/core';

import { Subscription } from 'rxjs';
import { ClaimsClient } from '../../web-api-client';

@Directive({
    selector: '[hasClaim]',
    standalone: true,
})
export class HasClaimDirective implements OnChanges {
    claimsSub: Subscription = new Subscription();

    @Input('hasClaim') claim: string = '';

    constructor(
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef,
        private claimsClient: ClaimsClient
    ) {}

    ngOnChanges(): void {
        if (this.claimsSub) {
            this.claimsSub.unsubscribe();
        }

        this.claimsSub = this.claimsClient
            .hasClaim(this.claim)
            .subscribe((hasClaim) => {
                console.log('HasClaim', hasClaim);

                if (hasClaim) {
                    if (this.viewContainer.length < 1) {
                        this.viewContainer.createEmbeddedView(this.templateRef);
                    }
                } else {
                    this.viewContainer.clear();
                }
            });
    }
}
