import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
   providedIn: 'root',
})
export class DataUpdateEventService {
   private dataUpdatedSource = new BehaviorSubject<void>(null);
   private downloadUpdatedSource = new BehaviorSubject<void>(null);
   dataUpdated$ = this.dataUpdatedSource.asObservable();
   downloadUpdated$ = this.downloadUpdatedSource.asObservable();

   notifyDataUpdated() {
      this.dataUpdatedSource.next();
   }

   notifyDownloadUpdated() {
      this.downloadUpdatedSource.next();
   }
}
