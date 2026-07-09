import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VagasList } from './vagas-list';

describe('VagasList', () => {
  let component: VagasList;
  let fixture: ComponentFixture<VagasList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VagasList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VagasList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
