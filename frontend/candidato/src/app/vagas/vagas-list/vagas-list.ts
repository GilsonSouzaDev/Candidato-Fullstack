import { Component, OnInit } from '@angular/core';
import { VagasService } from '../vagas.service';
import { CandidaturasService } from '../candidaturas.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/auth.service';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-vagas-list',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule, RouterModule],
  templateUrl: './vagas-list.html',
  styleUrls: ['./vagas-list.scss'],
})
export class VagasList implements OnInit {
  vagas: any[] = [];
  minhasInscricoes: number[] = [];
  filtroMinhas = false;
  msg = '';
  isError = false;
  searchQuery = '';
  
  // Pagination
  page = 1;
  pageSize = 3;

  constructor(
    private vagasService: VagasService, 
    private candidaturasService: CandidaturasService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.vagasService.getVagasAbertas().subscribe(v => this.vagas = v);
    if (this.isCandidato()) {
      this.candidaturasService.getMinhasInscricoes().subscribe(ids => this.minhasInscricoes = ids);
    }
  }

  isCandidato(): boolean {
    return this.authService.getToken() !== null && this.authService.getUserRole() === 'Candidato';
  }

  get displayedVagas() {
    let result = this.vagas;
    
    if (this.filtroMinhas) {
      result = result.filter(v => this.minhasInscricoes.includes(v.id));
    }
    
    if (this.searchQuery.trim()) {
      const q = this.searchQuery.toLowerCase().trim();
      result = result.filter(v => 
        v.titulo?.toLowerCase().includes(q) || 
        v.requisitos?.toLowerCase().includes(q) || 
        v.descricao?.toLowerCase().includes(q)
      );
    }
    
    return result;
  }

  onSearchChange() {
    this.page = 1;
  }

  get paginatedVagas() {
    const start = (this.page - 1) * this.pageSize;
    return this.displayedVagas.slice(start, start + this.pageSize);
  }

  get totalPages() {
    return Math.ceil(this.displayedVagas.length / this.pageSize);
  }

  nextPage() {
    if (this.page < this.totalPages) {
      this.page++;
    }
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
    }
  }

  toggleFiltroMinhas() {
    this.filtroMinhas = !this.filtroMinhas;
    this.page = 1; // reset page on filter change
  }

  apply(vagaId: number) {
    if (!this.authService.getToken()) {
      this.router.navigate(['/login']);
      return;
    }

    if (this.authService.getUserRole() !== 'Candidato') {
      this.msg = 'Apenas candidatos podem se aplicar a vagas.';
      this.isError = true;
      return;
    }

    this.candidaturasService.apply(vagaId).subscribe({
      next: (res) => {
        this.msg = res.message || res.Message || 'Candidatura enviada com sucesso!';
        this.isError = false;
        if (!this.minhasInscricoes.includes(vagaId)) {
          this.minhasInscricoes.push(vagaId);
        }
      },
      error: (err) => {
        this.msg = err.error?.error || err.error?.Error || err.error?.message || 'Erro ao aplicar. Tente novamente.';
        this.isError = true;
      }
    });
  }

  cancelApply(vagaId: number) {
    if (!this.authService.getToken()) return;

    this.candidaturasService.cancel(vagaId).subscribe({
      next: (res) => {
        this.msg = res.message || res.Message || 'Inscrição cancelada com sucesso!';
        this.isError = false;
        this.minhasInscricoes = this.minhasInscricoes.filter(id => id !== vagaId);
      },
      error: (err) => {
        this.msg = err.error?.error || err.error?.Error || err.error?.message || 'Erro ao cancelar inscrição.';
        this.isError = true;
      }
    });
  }
}
