import { TestBed } from '@angular/core/testing';

import { LeafletEsriService } from './leaflet-esri.service';

describe('LeafletEsriServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LeafletEsriService= TestBed.get(LeafletEsriService);
    expect(service).toBeTruthy();
  });
});
