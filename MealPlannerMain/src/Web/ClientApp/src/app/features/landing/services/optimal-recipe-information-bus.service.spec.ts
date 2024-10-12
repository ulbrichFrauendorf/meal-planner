import { TestBed } from '@angular/core/testing';

import { OptimalRecipeInformationBusService } from './optimal-recipe-information-bus.service';

describe('OptimalRecipeInformationBusService', () => {
  let service: OptimalRecipeInformationBusService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OptimalRecipeInformationBusService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
