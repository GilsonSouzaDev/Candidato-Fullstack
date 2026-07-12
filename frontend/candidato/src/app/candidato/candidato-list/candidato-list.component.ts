import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { catchError, Observable, of, map } from 'rxjs';

import { CandidatosService } from '../services/candidatos.service';
import { ErrorDialogComponent } from 'src/app/shared/components/error-dialog/error-dialog.component';
import { ActivatedRoute, Router } from '@angular/router';
import { Candidato } from '../models/Candidato';
import { __values } from 'tslib';


@Component({
  selector: 'app-candidato-list',
  templateUrl: './candidato-list.component.html',
  styleUrls: ['./candidato-list.component.scss']
})

export class CandidatoListComponent implements OnInit{


  candidatos$: Observable<Candidato[]>;
  displayedColumns = ['nome','cursos', 'telefone', 'action'];


  constructor(private candidatoService: CandidatosService,
              public dialog: MatDialog,
              private router : Router,
              private route: ActivatedRoute
              )
  {

    this.candidatos$ = this.candidatoService.getMeuPerfil()
      .pipe(
        map(candidato => [candidato]),
        catchError(error => {
          if (error.status === 404) {
             return of([]);
          }
          this.onError('Erro ao carregar currículo');
          return of([])
        })
    );
  }

  onError(errorMsg: string) {
    this.dialog.open(ErrorDialogComponent, {
    data: errorMsg
  });

  }

  ngOnInit(): void {
  }

  onAdd(){
    this.router.navigate(['new'], {relativeTo: this.route })
  }


 




}
