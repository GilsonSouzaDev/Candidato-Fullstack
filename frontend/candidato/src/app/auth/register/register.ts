import { Component } from '@angular/core';
import { AuthService } from '../../core/auth.service';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './register.html',
  styleUrls: ['./register.scss'],
})
export class Register {
  nome = '';
  email = '';
  senha = '';
  error = '';
  success = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.register({ nome: this.nome, email: this.email, senha: this.senha }).subscribe({
      next: () => {
        this.success = 'Conta criada com sucesso! Redirecionando...';
        this.error = '';
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (err) => {
        this.error = err.error?.Error || 'Erro no registro';
        this.success = '';
      }
    });
  }
}
