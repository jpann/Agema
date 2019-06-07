import { TestBed } from '@angular/core/testing';

import { OpenWeatherMapServiceService } from './open-weather-map-service.service';

describe('OpenWeatherMapServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: OpenWeatherMapServiceService = TestBed.get(OpenWeatherMapServiceService);
    expect(service).toBeTruthy();
  });
});
