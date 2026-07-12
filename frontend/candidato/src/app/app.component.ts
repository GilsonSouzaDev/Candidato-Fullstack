import { Component } from '@angular/core';
import { AuthService } from './core/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'candidato';
  isMenuOpen = false;
  
  constructor(public authService: AuthService) {}
  
  logout() {
    this.authService.logout();
    this.isMenuOpen = false;
  }
  
  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }
  
  closeMenu() {
    this.isMenuOpen = false;
  }
}
