import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/auth.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  nome = '';
  email = '';
  senha = '';
  msg = '';
  isError = false;
  isSaving = false;

  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit(): void {
  }

  onSubmit() {
    this.http.post(environment.apiUrl + '/admin/recrutadores', {
      nome: this.nome,
      email: this.email,
      senha: this.senha
    }, { responseType: 'text' }).subscribe({
      next: (res) => {
        this.msg = 'Recrutador cadastrado com sucesso!';
        this.isError = false;
        this.nome = '';
        this.email = '';
        this.senha = '';
      },
      error: (err) => {
        try {
          const parsed = JSON.parse(err.error);
          this.msg = parsed.error || parsed.Error || 'Erro ao cadastrar recrutador';
        } catch {
          this.msg = err.error || 'Erro ao cadastrar recrutador';
        }
        this.isError = true;
      }
    });
  }
}
