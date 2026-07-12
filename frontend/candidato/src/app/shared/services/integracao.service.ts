import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IntegracaoService {

  private apiUrl = '/api/integracao';

  constructor(private http: HttpClient) { }

  getCursosGraduacao(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/cursos-graduacao`);
  }
}
