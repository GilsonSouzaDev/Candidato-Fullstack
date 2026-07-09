import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';

import { Candidato } from '../models/Candidato';
import { CandidatosService } from '../services/candidatos.service';

@Injectable({
  providedIn: 'root'
})
export class CandidatoResolver implements Resolve<Candidato> {
  constructor(private service: CandidatosService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Candidato> {
    const candidatoId = route.params['id'];

    if (candidatoId) {

      return this.service.save(candidatoId);

    }else{

      return of({
        id: 0,
        nome: '',
        filiacao: {  nomePai: '', nomeMae: '' },
        endereco: { logradouro: '', cep: '', numero: '', cidade: {  nome: '', estado: { nome: '', sigla: '' } } },
        telefones: [{ numero: '', tipo: 0 }],
        cursos: [{ nome: '' }]
    } as Candidato);

    }

  }
}
