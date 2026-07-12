import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { VagasService } from '../vagas.service';
import { CandidaturasService } from '../candidaturas.service';
import { AuthService } from '../../core/auth.service';

@Component({
  selector: 'app-vaga-detalhes',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule],
  templateUrl: './vaga-detalhes.html',
  styleUrls: ['./vaga-detalhes.scss'],
})
export class VagaDetalhes implements OnInit {
  vaga: any = null;
  minhasInscricoes: number[] = [];
  msg = '';
  isError = false;
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vagasService: VagasService,
    private candidaturasService: CandidaturasService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = +idParam;
      this.loadVaga(id);
    }
  }

  loadVaga(id: number) {
    this.vagasService.getVagaById(id).subscribe({
      next: (v) => {
        this.vaga = v;
        this.loading = false;
        if (this.isCandidato()) {
          this.candidaturasService.getMinhasInscricoes().subscribe(ids => {
            this.minhasInscricoes = ids;
          });
        }
      },
      error: (err) => {
        this.msg = 'Vaga não encontrada.';
        this.isError = true;
        this.loading = false;
      }
    });
  }

  isCandidato(): boolean {
    return this.authService.getToken() !== null && this.authService.getUserRole() === 'Candidato';
  }

  parseList(text: string): string[] {
    if (!text) return [];
    return text.split('\n').map(l => l.trim()).filter(l => l.length > 0);
  }

  get isApplied(): boolean {
    if (!this.vaga) return false;
    return this.minhasInscricoes.includes(this.vaga.id);
  }

  apply() {
    if (!this.vaga) return;
    if (!this.authService.getToken()) {
      this.router.navigate(['/login']);
      return;
    }

    if (!this.isCandidato()) {
      this.msg = 'Apenas candidatos podem se aplicar a vagas.';
      this.isError = true;
      return;
    }

    this.candidaturasService.apply(this.vaga.id).subscribe({
      next: (res) => {
        this.msg = 'Candidatura enviada com sucesso!';
        this.isError = false;
        if (!this.minhasInscricoes.includes(this.vaga.id)) {
          this.minhasInscricoes.push(this.vaga.id);
        }
      },
      error: (err) => {
        this.msg = err.error?.error || 'Erro ao aplicar.';
        this.isError = true;
      }
    });
  }

  cancelApply() {
    if (!this.vaga) return;
    if (!this.authService.getToken()) return;

    this.candidaturasService.cancel(this.vaga.id).subscribe({
      next: (res) => {
        this.msg = 'Inscrição cancelada com sucesso!';
        this.isError = false;
        this.minhasInscricoes = this.minhasInscricoes.filter(id => id !== this.vaga.id);
      },
      error: (err) => {
        this.msg = err.error?.error || 'Erro ao cancelar.';
        this.isError = true;
      }
    });
  }
}
