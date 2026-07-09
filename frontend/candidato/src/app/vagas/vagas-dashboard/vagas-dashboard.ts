import { Component, OnInit } from '@angular/core';
import { VagasService } from '../vagas.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-vagas-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule],
  templateUrl: './vagas-dashboard.html',
  styleUrls: ['./vagas-dashboard.scss'],
})
export class VagasDashboard implements OnInit {
  vagas: any[] = [];
  constructor(private vagasService: VagasService) {}
  ngOnInit() {
    this.vagasService.getMinhasVagas().subscribe(v => this.vagas = v);
  }
}
