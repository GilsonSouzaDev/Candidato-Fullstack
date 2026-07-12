import { Component, OnInit } from '@angular/core';
import { VagasService } from '../vagas.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-vagas-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, FormsModule],
  templateUrl: './vagas-dashboard.html',
  styleUrls: ['./vagas-dashboard.scss'],
})
export class VagasDashboard implements OnInit {
  vagas: any[] = [];
  showModal = false;
  novaVaga: any = {
    titulo: '',
    descricao: '',
    requisitos: '', // legacy
    salario: null,
    nomeEmpresa: '',
    beneficios: '',
    atividades: '',
    requisitosObrigatorios: '',
    requisitosDesejaveis: ''
  };

  constructor(private vagasService: VagasService, private router: Router) {}
  ngOnInit() {
    this.loadVagas();
  }

  goToPipeline(id: number) {
    this.router.navigate(['/candidaturas', id]);
  }

  loadVagas() {
    this.vagasService.getMinhasVagas().subscribe(v => this.vagas = v);
  }

  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  salvarVaga() {
    this.vagasService.createVaga(this.novaVaga).subscribe({
      next: (res) => {
        this.loadVagas();
        this.closeModal();
        this.novaVaga = { 
          titulo: '', descricao: '', requisitos: '', salario: null,
          nomeEmpresa: '', beneficios: '', atividades: '', requisitosObrigatorios: '', requisitosDesejaveis: '' 
        };
      },
      error: (err) => {
        console.error('Erro ao criar vaga:', err);
      }
    });
  }

  deletarVaga(id: number) {
    if(confirm('Tem certeza que deseja excluir esta vaga?')) {
      this.vagasService.deleteVaga(id).subscribe({
        next: () => this.loadVagas(),
        error: (err) => console.error('Erro ao excluir vaga:', err)
      });
    }
  }
}
