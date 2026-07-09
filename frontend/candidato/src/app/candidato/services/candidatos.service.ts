import { HttpClient,  HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { first, tap } from 'rxjs/operators';
import { Observable  } from 'rxjs';
import { Candidato } from '../models/Candidato';


@Injectable({
  providedIn: 'root'
})
export class CandidatosService {

  private readonly API = 'https://api-candidato-fake.vercel.app/Candidato';
  http: any;

  constructor( private httpClient: HttpClient) { }

  list() {
    return this.httpClient.get<Candidato[]>(this.API)
    .pipe(
      first(),
      tap(candidatos => console.log(candidatos))
    );
  }

  display(id: number){
      return this.httpClient.get<Candidato>(`${this.API}/${id}`)
      .pipe(
        first(),
        tap(candidatos => console.log(candidatos))
      );

  }


  edit(candidato: Candidato, id: number): Observable<Candidato> {
    const url = `${this.API}/${id}`;
    return this.httpClient.put<Candidato>(url, candidato).
    pipe(
      tap(updatedCandidato => console.log(`Candidato atualizado: ${updatedCandidato.id}`))
    );
  }

  delete(id: number): Observable<void> {
    const url = `${this.API}/${id}`;
    return this.httpClient.delete<void>(url).pipe(
      tap(() => console.log(`Candidato deletado: ${id}`))
    );
  }

  save(candidato: Candidato): Observable<Candidato> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.httpClient.post<Candidato>(this.API, candidato, { headers });
  }


}
