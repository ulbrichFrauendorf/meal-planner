import { Injectable } from '@angular/core';
import { AngularSettingsDto, BootstrapClient } from 'app/web-api-client';
import { Observable, of, tap } from 'rxjs';

@Injectable({
   providedIn: 'root',
})
export class AppSettingsService {
   constructor(private bootstrapClient: BootstrapClient) {}

   private settings: AngularSettingsDto;

   getSettings(): Observable<AngularSettingsDto> {
      if (this.settings) {
         return of(this.settings);
      } else {
         return this.bootstrapClient
            .getAngularApplicationSettings()
            .pipe(tap((settings) => (this.settings = settings)));
      }
   }
}
