import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from './core/role.guard';

const routes: Routes = [
 { path: '', pathMatch:'full', redirectTo: 'vagas'},
 { path: 'login', loadComponent: () => import('./auth/login/login').then(c => c.Login) },
 { path: 'register', loadComponent: () => import('./auth/register/register').then(c => c.Register) },
 { path: 'vagas', loadComponent: () => import('./vagas/vagas-list/vagas-list').then(c => c.VagasList) },
 { path: 'vagas/:id', loadComponent: () => import('./vagas/vaga-detalhes/vaga-detalhes').then(c => c.VagaDetalhes) },
 { path: 'dashboard', loadComponent: () => import('./vagas/vagas-dashboard/vagas-dashboard').then(c => c.VagasDashboard), canActivate: [RoleGuard], data: { expectedRoles: ['Recrutador', 'Admin'] } },
 { path: 'admin', loadComponent: () => import('./admin/admin-dashboard/admin-dashboard.component').then(c => c.AdminDashboardComponent), canActivate: [RoleGuard], data: { expectedRoles: ['Admin'] } },
 { path: 'candidaturas/:vagaId', loadComponent: () => import('./vagas/candidaturas-kanban/candidaturas-kanban').then(c => c.CandidaturasKanban), canActivate: [RoleGuard], data: { expectedRoles: ['Recrutador', 'Admin'] } },
 {
  path: 'candidato',
  loadChildren: () => import('./candidato/candidato.module').then(m => m.CandidatoModule),
  canActivate: [RoleGuard],
  data: { expectedRoles: ['Candidato', 'Recrutador', 'Admin'] }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
