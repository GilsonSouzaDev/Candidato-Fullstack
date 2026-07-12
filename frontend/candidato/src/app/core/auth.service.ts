import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = '/api/auth';
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpClient, private router: Router) { }

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('token');
  }

  public getToken(): string | null {
    return localStorage.getItem('token');
  }

  public getDecodedToken(): any {
    const token = this.getToken();
    if (token) {
      try {
        return jwtDecode(token);
      } catch (Error) {
        return null;
      }
    }
    return null;
  }

  public getUserRole(): string {
    const token = this.getDecodedToken();
    return token ? (token.role || token['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || '') : '';
  }

  public getUserId(): string {
    const token = this.getDecodedToken();
    return token ? (token.nameid || token['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || '') : '';
  }

  register(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  login(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, data).pipe(
      tap((response: any) => {
        const token = response?.token || response?.Token;
        if (token) {
          localStorage.setItem('token', token);
          this.loggedIn.next(true);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }
}
