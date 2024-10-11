import { inject, NgZone } from '@angular/core';
import { HttpEvent, HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MessageService } from 'primeng/api';

export const frontEndApiInterceptor: HttpInterceptorFn = (
   req: HttpRequest<any>,
   next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
   const messageService = inject(MessageService);
   const ngZone = inject(NgZone);

   return next(req).pipe(
      catchError((error: HttpErrorResponse) => {
         let errorMessage = 'Unknown error occurred';

         if (
            error.error instanceof Blob &&
            error.error.type === 'application/json'
         ) {
            error.error.text().then((jsonError: string) => {
               const err = JSON.parse(jsonError);
               errorMessage = err?.detail || errorMessage;
               console.log(errorMessage);

               ngZone.run(() => {
                  messageService.add({
                     severity: 'error',
                     summary: 'Server Error',
                     detail: errorMessage,
                     key: 'global',
                  });
               });
            });
         }

         return throwError(() => new Error(errorMessage));
      })
   );
};
