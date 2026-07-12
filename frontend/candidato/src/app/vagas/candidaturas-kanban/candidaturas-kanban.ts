import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CandidaturasService } from '../candidaturas.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-candidaturas-kanban',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule],
  templateUrl: './candidaturas-kanban.html',
  styleUrls: ['./candidaturas-kanban.scss'],
})
export class CandidaturasKanban implements OnInit {
  vagaId!: number;
  candidaturas: any[] = [];
  
  colunas = ['Triagem', 'Em Analise', 'Entrevista', 'Aprovado', 'Reprovado'];

  constructor(private route: ActivatedRoute, private candidaturasService: CandidaturasService) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.vagaId = +params['vagaId'];
      this.loadData();
    });
  }

  loadData() {
    this.candidaturasService.getCandidaturas(this.vagaId).subscribe(data => {
      this.candidaturas = data;
    });
  }

  getCandidaturasPorStatus(status: string) {
    return this.candidaturas.filter(c => c.statusCandidatura === status);
  }

  updateStatus(candidaturaId: number, novoStatus: string) {
    this.candidaturasService.updateStatus(candidaturaId, novoStatus).subscribe(() => {
      this.loadData();
    });
  }

  toggleCard(c: any) {
    c.expanded = !c.expanded;
  }
}
