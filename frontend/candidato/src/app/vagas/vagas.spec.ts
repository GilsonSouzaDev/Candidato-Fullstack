import { TestBed } from '@angular/core/testing';

import { Vagas } from './vagas';

describe('Vagas', () => {
  let service: Vagas;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Vagas);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
