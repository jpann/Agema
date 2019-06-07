import { TestBed } from '@angular/core/testing';

import { LeafletGeoJsonService } from './leaflet-geo-json.service';

describe('LeafletGeoJsonService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LeafletGeoJsonService = TestBed.get(LeafletGeoJsonService);
    expect(service).toBeTruthy();
  });
});
