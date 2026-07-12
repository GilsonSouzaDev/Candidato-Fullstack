import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const expectedRoles = route.data['expectedRoles'] as Array<string>;
    const userRole = this.authService.getUserRole();

    if (!this.authService.getToken() || !expectedRoles.includes(userRole)) {
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }
}
