import {
   Directive,
   Input,
   OnChanges,
   TemplateRef,
   ViewContainerRef,
} from '@angular/core';
import { ClaimsClient } from 'app/web-api-client';
import { Subscription } from 'rxjs';

@Directive({
   selector: '[appHasClaim]',
   standalone: true,
})
export class HasClaimDirective implements OnChanges {
   claimsSub: Subscription = new Subscription();

   @Input() appHasClaim: string = '';

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
         .hasClaim(this.appHasClaim)
         .subscribe((hasClaim) => {
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
