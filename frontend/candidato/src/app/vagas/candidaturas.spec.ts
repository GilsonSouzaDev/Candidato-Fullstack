import { TestBed } from '@angular/core/testing';

import { Candidaturas } from './candidaturas';

describe('Candidaturas', () => {
  let service: Candidaturas;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Candidaturas);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
