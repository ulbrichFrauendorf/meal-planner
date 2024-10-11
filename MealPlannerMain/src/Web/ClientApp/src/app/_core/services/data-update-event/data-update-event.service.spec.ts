import { TestBed } from '@angular/core/testing';

import { DataUpdateEventService } from '../data-update-event.service';

describe('DataUpdateEventService', () => {
   let service: DataUpdateEventService;

   beforeEach(() => {
      TestBed.configureTestingModule({});
      service = TestBed.inject(DataUpdateEventService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });
});
