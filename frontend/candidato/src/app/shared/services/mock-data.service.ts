import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MockDataService {

  private cursosMock = [
    'Ciência da Computação',
    'Engenharia de Software',
    'Sistemas de Informação',
    'Análise e Desenvolvimento de Sistemas',
    'Gestão da Tecnologia da Informação',
    'Engenharia da Computação',
    'Design de Interação',
    'Segurança da Informação',
    'Redes de Computadores',
    'Banco de Dados'
  ];

  private hardSkillsMock = [
    'Angular',
    'React',
    'Vue.js',
    'TypeScript',
    'JavaScript',
    'C#',
    '.NET Core',
    'Java',
    'Spring Boot',
    'Python',
    'Django',
    'Node.js',
    'SQL Server',
    'PostgreSQL',
    'MongoDB',
    'Docker',
    'Kubernetes',
    'AWS',
    'Azure',
    'Git',
    'CI/CD'
  ];

  private softSkillsMock = [
    'Comunicação Efetiva',
    'Liderança',
    'Trabalho em Equipe',
    'Resolução de Problemas',
    'Pensamento Crítico',
    'Gestão de Tempo',
    'Adaptabilidade',
    'Empatia',
    'Inteligência Emocional',
    'Proatividade'
  ];

  constructor() { }

  getCursos(): Observable<string[]> {
    return of(this.cursosMock).pipe(delay(300));
  }

  getHardSkills(): Observable<string[]> {
    return of(this.hardSkillsMock).pipe(delay(300));
  }

  getSoftSkills(): Observable<string[]> {
    return of(this.softSkillsMock).pipe(delay(300));
  }
}
