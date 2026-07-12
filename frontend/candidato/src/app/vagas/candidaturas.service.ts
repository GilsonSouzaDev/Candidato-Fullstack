import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CandidaturasService {
  private apiUrl = '/api/candidaturas';

  constructor(private http: HttpClient) { }

  apply(vagaId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/aplicar`, { vagaId });
  }

  getCandidaturas(vagaId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/vaga/${vagaId}`);
  }

  updateStatus(id: number, status: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/status`, { status });
  }

  getMinhasInscricoes(): Observable<number[]> {
    return this.http.get<number[]>(`${this.apiUrl}/minhas`);
  }

  cancel(vagaId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/vaga/${vagaId}`);
  }
}
