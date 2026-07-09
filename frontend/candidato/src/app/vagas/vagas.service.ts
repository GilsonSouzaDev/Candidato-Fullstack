import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VagasService {
  private apiUrl = 'https://localhost:7198/api/vagas';

  constructor(private http: HttpClient) { }

  getVagasAbertas(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getMinhasVagas(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/minhas`);
  }

  getVagaById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  createVaga(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }
}
