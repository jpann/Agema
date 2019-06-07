import { TestBed } from '@angular/core/testing';

import { LeafletHEREService } from './leaflet-here.service';

describe('LeafletHEREService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LeafletHEREService = TestBed.get(LeafletHEREService);
    expect(service).toBeTruthy();
  });
});
