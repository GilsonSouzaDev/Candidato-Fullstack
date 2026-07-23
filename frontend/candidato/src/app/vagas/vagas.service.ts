import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VagasService {
  private apiUrl = environment.apiUrl + '/vagas';

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

  deleteVaga(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
