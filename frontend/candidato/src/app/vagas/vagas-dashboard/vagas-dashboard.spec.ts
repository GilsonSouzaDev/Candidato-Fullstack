import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VagasDashboard } from './vagas-dashboard';

describe('VagasDashboard', () => {
  let component: VagasDashboard;
  let fixture: ComponentFixture<VagasDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VagasDashboard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VagasDashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
