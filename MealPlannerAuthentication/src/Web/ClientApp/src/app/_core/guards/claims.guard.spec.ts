import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { claimsGuard } from './claims.guard';

describe('claimsGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => claimsGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
