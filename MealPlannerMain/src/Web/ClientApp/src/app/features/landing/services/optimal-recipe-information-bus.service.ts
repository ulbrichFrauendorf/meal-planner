import {
    effect,
    Injectable,
    Injector,
    runInInjectionContext,
    signal,
} from '@angular/core';
import { CalculationResult } from '@app/web-api-client';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class OptimalRecipeInformationBusService {
    public calculationResult = signal<CalculationResult>(null);

    private calculationResultSubject$ = new BehaviorSubject<CalculationResult>(
        this.getCalculationResult()
    );

    constructor(private injector: Injector) {
        runInInjectionContext(this.injector, () => {
            effect(
                () => {
                    const result = this.getCalculationResult();
                    this.calculationResultSubject$.next(result);
                },
                { allowSignalWrites: true }
            );
        });
    }

    getCalculationResult() {
        return this.calculationResult();
    }

    setCalculationResult(calculationResult: CalculationResult) {
        this.calculationResult.set(calculationResult);
    }
}
