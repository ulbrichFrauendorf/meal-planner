import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { map, catchError, of } from 'rxjs';
import { ClaimsClient } from '../../web-api-client';

export const claimsGuard: CanActivateFn = (route) => {
    const router = inject(Router);
    const expectedClaim = route.data['claim'];
    const claimsClient = inject(ClaimsClient);

    return claimsClient.hasClaim(expectedClaim).pipe(
        map((res) => {
            if (!res) {
                router.navigate(['status/unauthorized']);
                return false;
            }

            return true;
        }),
        catchError((error) => {
            console.error('Error during authentication check', error);
            router.navigate(['status/unauthorized']);
            return of(false);
        })
    );
};
