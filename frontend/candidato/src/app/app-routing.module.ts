import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
 { path: '', pathMatch:'full', redirectTo: 'vagas'},
 { path: 'login', loadComponent: () => import('./auth/login/login').then(c => c.Login) },
 { path: 'register', loadComponent: () => import('./auth/register/register').then(c => c.Register) },
 { path: 'vagas', loadComponent: () => import('./vagas/vagas-list/vagas-list').then(c => c.VagasList) },
 { path: 'dashboard', loadComponent: () => import('./vagas/vagas-dashboard/vagas-dashboard').then(c => c.VagasDashboard) },
 { path: 'candidaturas/:vagaId', loadComponent: () => import('./vagas/candidaturas-kanban/candidaturas-kanban').then(c => c.CandidaturasKanban) },
 {
  path: 'Candidato',
  loadChildren: () => import('./candidato/candidato.module').then(m => m.CandidatoModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
