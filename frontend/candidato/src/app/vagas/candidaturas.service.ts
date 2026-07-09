import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CandidaturasService {
  private apiUrl = 'https://localhost:7198/api/candidaturas';

  constructor(private http: HttpClient) { }

  apply(vagaId: number, candidatoId: number): Observable<any> {
    return this.http.post(this.apiUrl, { vagaId, candidatoId });
  }

  getCandidaturas(vagaId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/vaga/${vagaId}`);
  }

  updateStatus(id: number, status: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/status`, { status });
  }
}
