import { Subject } from 'rxjs';
import { OnDestroy } from '@angular/core';

type Constructor<T = {}> = new (...args: any[]) => T;

export function WithDestroy<T extends Constructor<{}>>(
    Base: T = class {} as any
) {
    return class extends Base implements OnDestroy {
        destroy$ = new Subject<void>();

        ngOnDestroy() {
            this.destroy$.next();
            this.destroy$.complete();
        }
    };
}
