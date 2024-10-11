import {
    Injectable,
    Injector,
    effect,
    inject,
    runInInjectionContext,
    signal,
} from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { isNullOrWhitespace } from 'app/_core/utils/null-or-whitespace';

import { BehaviorSubject, combineLatest, filter } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class ViewStateService {
    private readonly oidcSecurityService = inject(OidcSecurityService);
    private emptyState = signal<boolean>(true);
    private currentUser = signal<string>('');
    private currentTenant = signal<string>('');
    private currentRecipe = signal<string>('');
    private currentIntegratedApi = signal<string>(
        '01f2d46a-8a0b-4790-b468-ce93c7e7b912'
    );

    private emptyStateSubject = new BehaviorSubject<boolean>(
        this.getEmptyState()
    );
    private currentUserSubject = new BehaviorSubject<string>(
        this.getCurrentUser()
    );
    private currentTenantSubject = new BehaviorSubject<string>(
        this.getCurrentTenant()
    );
    private currentrecipeubject = new BehaviorSubject<string>(
        this.getCurrentRecipe()
    );
    private currentIntegratedApiSubject = new BehaviorSubject<string>(
        this.getCurrentIntegratedApi()
    );

    emptyState$ = this.emptyStateSubject.asObservable();
    currentUser$ = this.currentUserSubject.asObservable();
    currentTenant$ = this.currentTenantSubject.asObservable();
    currentRecipe$ = this.currentrecipeubject.asObservable();
    currentIntegratedApi$ = this.currentIntegratedApiSubject.asObservable();

    constructor(private injector: Injector) {
        runInInjectionContext(this.injector, () => {
            effect(
                () => {
                    const empty = this.getEmptyState();
                    this.emptyStateSubject.next(empty);
                },
                { allowSignalWrites: true }
            );

            effect(
                () => {
                    const userId = this.getCurrentUser();
                    this.currentUserSubject.next(userId);
                },
                { allowSignalWrites: true }
            );

            effect(
                () => {
                    const tenantId = this.getCurrentTenant();
                    this.currentTenantSubject.next(tenantId);
                },
                { allowSignalWrites: true }
            );

            effect(
                () => {
                    const recipeId = this.getCurrentRecipe();
                    this.currentrecipeubject.next(recipeId);
                },
                { allowSignalWrites: true }
            );

            effect(
                () => {
                    const integratedApiId = this.getCurrentIntegratedApi();
                    this.currentIntegratedApiSubject.next(integratedApiId);
                },
                { allowSignalWrites: true }
            );
        });
    }

    getEmptyState() {
        return this.emptyState();
    }

    setEmptyState(isEmpty: boolean) {
        this.emptyState.set(isEmpty);
    }

    getCurrentUser() {
        return this.currentUser();
    }

    setCurrentUser() {
        const userId = this.oidcSecurityService.userData().userData.sub;
        this.currentUser.set(userId);
    }

    getCurrentTenant() {
        return this.currentTenant();
    }

    setCurrentTenant(tenantId: string) {
        this.emptyState.set(false);
        this.currentTenant.set(tenantId);
    }

    getCurrentRecipe() {
        return this.currentRecipe();
    }

    setCurrentRecipe(recipeId: string) {
        this.currentRecipe.set(recipeId);
    }

    getCurrentIntegratedApi() {
        return this.currentIntegratedApi();
    }

    setCurrentIntegratedApi(currentIntegratedApiId: string) {
        this.currentIntegratedApi.set(currentIntegratedApiId);
    }

    getCurrentRecipeIntegratedApi() {
        return combineLatest([
            this.currentRecipe$,
            this.currentIntegratedApi$,
        ]).pipe(
            filter(
                ([recipeId, integratedApiId]) =>
                    !isNullOrWhitespace(recipeId) &&
                    !isNullOrWhitespace(integratedApiId)
            )
        );
    }

    getCurrentTenantIntegration() {
        return combineLatest([
            this.currentTenant$,
            this.currentIntegratedApi$,
        ]).pipe(
            filter(
                ([tenantId, integratedApiId]) =>
                    !isNullOrWhitespace(tenantId) &&
                    !isNullOrWhitespace(integratedApiId)
            )
        );
    }
}
