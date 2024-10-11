import { BehaviorSubject, Observable } from 'rxjs';

declare global {
   interface Function {
      asObservable<T>(
         this: () => T,
         onChange: (callback: () => void) => void
      ): Observable<T>;
   }
}

Function.prototype.asObservable = function <T>(
   this: () => T,
   onChange: (callback: () => void) => void
): Observable<T> {
   const subject = new BehaviorSubject<T>(this());

   // Assuming onChange is a method to subscribe to changes
   onChange(() => {
      subject.next(this());
   });

   return subject.asObservable();
};
