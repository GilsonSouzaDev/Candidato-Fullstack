import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidaturasKanban } from './candidaturas-kanban';

describe('CandidaturasKanban', () => {
  let component: CandidaturasKanban;
  let fixture: ComponentFixture<CandidaturasKanban>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CandidaturasKanban]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CandidaturasKanban);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
