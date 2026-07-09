import { Component, OnInit } from '@angular/core';
import { VagasService } from '../vagas.service';
import { CandidaturasService } from '../candidaturas.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-vagas-list',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule],
  templateUrl: './vagas-list.html',
  styleUrls: ['./vagas-list.scss'],
})
export class VagasList implements OnInit {
  vagas: any[] = [];
  candidatoId: number = 1; // Mock candidate ID
  msg = '';

  constructor(private vagasService: VagasService, private candidaturasService: CandidaturasService) {}

  ngOnInit() {
    this.vagasService.getVagasAbertas().subscribe(v => this.vagas = v);
  }

  apply(vagaId: number) {
    this.candidaturasService.apply(vagaId, this.candidatoId).subscribe({
      next: (res) => this.msg = res.Message,
      error: (err) => this.msg = err.error?.Error || 'Erro ao aplicar'
    });
  }
}
