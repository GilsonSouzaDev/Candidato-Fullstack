import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IntegracaoService {

  private apiUrl = environment.apiUrl + '/integracao';

  constructor(private http: HttpClient) { }

  getCursosGraduacao(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/cursos-graduacao`);
  }
}
