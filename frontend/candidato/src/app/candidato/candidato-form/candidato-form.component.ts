import { Candidato } from './../models/Candidato';
import { CandidatosService } from './../services/candidatos.service';
import { IntegracaoService } from 'src/app/shared/services/integracao.service';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { MockDataService } from 'src/app/shared/services/mock-data.service';

@Component({
  selector: 'app-candidato-form',
  templateUrl: './candidato-form.component.html',
  styleUrls: ['./candidato-form.component.scss']
})
export class CandidatoFormComponent implements OnInit {

perfilId?: number;
meuFormulario: FormGroup;
allFormacoes: string[] = [];
allHardSkills: string[] = [];
allSoftSkills: string[] = [];

    constructor(
      private formBuilder: FormBuilder,
      public dialog: MatDialog,
      private router : Router,
      private route: ActivatedRoute,
      private candidatoService: CandidatosService,
      private bar: MatSnackBar,
      private mockData: MockDataService,
      private integracaoService: IntegracaoService
      )

    {

    this.meuFormulario = this.formBuilder.group({

        Nome: ['' ,Validators.required],
        Cpf: ['', Validators.required],
        DataNascimento: ['', Validators.required],
        LinkedInUrl: [''],
        ResumoProfissional: [''],

          Filiacao: this.formBuilder.group({
            NomePai: [''],
            NomeMae: ['', Validators.required]
          }),

          Endereco: this.formBuilder.group({
            Logradouro: ['' ,Validators.required],
            Cep: ['' ,Validators.required],
            Numero: ['' ,Validators.required],
            Bairro: ['' ,Validators.required],
            Cidade: this.formBuilder.group({
              Nome: ['',Validators.required],
              Estado: this.formBuilder.group({
                Nome: ['',Validators.required],
                Sigla: ['',Validators.required]
              })

            })

          }),

          Telefones: this.formBuilder.array([]),

          Cursos: this.formBuilder.array([]),
          
          Formacoes: this.formBuilder.array([]),
          
          Habilidades: this.formBuilder.array([]),
          
          Experiencias: this.formBuilder.array([])

      })

    }

    ngOnInit() {
      this.integracaoService.getCursosGraduacao().subscribe(c => this.allFormacoes = c);
      this.mockData.getHardSkills().subscribe(c => this.allHardSkills = c);
      this.mockData.getSoftSkills().subscribe(c => this.allSoftSkills = c);

      this.candidatoService.getMeuPerfil().subscribe({
        next: (perfil) => {
          if (perfil) {
            this.perfilId = perfil.id;
            this.preencherFormulario(perfil);
          }
        },
        error: (err) => {
          // Normal if profile doesn't exist yet
        }
      });
    }

    preencherFormulario(perfil: Candidato) {
      this.meuFormulario.patchValue({
        Nome: perfil.nome,
        Cpf: perfil.cpf,
        DataNascimento: perfil.dataNascimento ? perfil.dataNascimento.toString().substring(0, 10) : null,
        LinkedInUrl: perfil.linkedInUrl,
        ResumoProfissional: perfil.resumoProfissional,
        Filiacao: perfil.filiacao ? {
          NomePai: perfil.filiacao.nomePai,
          NomeMae: perfil.filiacao.nomeMae
        } : null,
        Endereco: perfil.endereco ? {
          Logradouro: perfil.endereco.logradouro,
          Cep: perfil.endereco.cep,
          Numero: perfil.endereco.numero,
          Bairro: perfil.endereco.bairro,
          Cidade: perfil.endereco.cidade ? {
            Nome: perfil.endereco.cidade.nome,
            Estado: perfil.endereco.cidade.estado ? {
              Nome: perfil.endereco.cidade.estado.nome,
              Sigla: perfil.endereco.cidade.estado.sigla
            } : null
          } : null
        } : null
      });
      
      // Repopulate arrays
      perfil.telefones?.forEach(t => {
        this.telefones.push(this.formBuilder.group({
          Numero: [t.numero, Validators.required],
          Tipo: [t.tipo, Validators.required]
        }));
      });

      perfil.cursos?.forEach(c => {
        this.cursos.push(this.formBuilder.group({
          Nome: [c.nome, Validators.required],
          Instituicao: [c.instituicao],
          AnoConclusao: [c.anoConclusao],
          TotalHoras: [c.totalHoras]
        }));
      });

      perfil.formacoes?.forEach(f => {
        this.formacoes.push(this.formBuilder.group({
          Nome: [f.nome, Validators.required],
          Instituicao: [f.instituicao, Validators.required],
          Status: [f.status],
          DataInicio: [f.dataInicio ? f.dataInicio.toString().substring(0, 10) : ''],
          DataFim: [f.dataFim ? f.dataFim.toString().substring(0, 10) : '']
        }));
      });

      perfil.habilidades?.forEach(h => {
        this.habilidades.push(this.formBuilder.group({
          Nome: [h.nome, Validators.required],
          Tipo: [h.tipo, Validators.required],
          Nivel: [h.nivel, Validators.required]
        }));
      });

      perfil.experiencias?.forEach(e => {
        this.experiencias.push(this.formBuilder.group({
          Empresa: [e.empresa, Validators.required],
          Cargo: [e.cargo, Validators.required],
          DataInicio: [e.dataInicio ? e.dataInicio.toString().substring(0, 10) : ''],
          DataFim: [e.dataFim ? e.dataFim.toString().substring(0, 10) : ''],
          Descricao: [e.descricao]
        }));
      });
    }

    filterFormacoes(val: string): string[] {
      if (!val) return this.allFormacoes;
      const lower = val.toLowerCase();
      return this.allFormacoes.filter(c => c.toLowerCase().includes(lower));
    }

    filterSkills(val: string, tipo: string): string[] {
      const list = tipo === 'SoftSkill' ? this.allSoftSkills : this.allHardSkills;
      if (!val) return list;
      const lower = val.toLowerCase();
      return list.filter(c => c.toLowerCase().includes(lower));
    }

    get Nome(){
      return this.meuFormulario.get('Nome')!;
    }
    get NomeMae(){
      return this.meuFormulario.get('Filiacao.NomeMae')!;
    }
    get Logradouro(){
      return this.meuFormulario.get('Endereco.Logradouro')!;
    }
    get Numero(){
      return this.meuFormulario.get('Endereco.Numero')!;
    }
    get Cep(){
      return this.meuFormulario.get('Endereco.Cep')!;
    }
    get Cidade(){
      return this.meuFormulario.get('Endereco.Cidade.Nome')!;
    }
    get Estado(){
      return this.meuFormulario.get('Endereco.Cidade.Estado.Nome')!;
    }
    get Sigla(){
      return this.meuFormulario.get('Endereco.Cidade.Estado.Sigla')!;
    }
    get Telefones(){
      return this.meuFormulario.controls["Telefones"] as FormArray;
    }
    get Cursos(){
      return this.meuFormulario.controls["Cursos"] as FormArray;
    }


    get telefones() {
      return this.meuFormulario.controls["Telefones"] as FormArray;
    }

    get cursos(){
      return this.meuFormulario.controls["Cursos"] as FormArray;
    }

    get formacoes(){
      return this.meuFormulario.controls["Formacoes"] as FormArray;
    }

    get habilidades() {
      return this.meuFormulario.controls["Habilidades"] as FormArray;
    }

    get experiencias() {
      return this.meuFormulario.controls["Experiencias"] as FormArray;
    }


    addPhone() {
      const phoneForm = this.formBuilder.group({
        Numero: ['', Validators.required],
        Tipo: ['', Validators.required]
      });
      this.telefones.push(phoneForm);
    }

    deletePhone(phoneIndex: number) {
      this.telefones.removeAt(phoneIndex);
    }

    addCourse(){
    const courseForm =  this.formBuilder.group({
      Nome: ['', Validators.required],
      Instituicao: [''],
      AnoConclusao: [''],
      TotalHoras: ['']
      });
      this.cursos.push(courseForm);
    }

    deleteCourse(courseIndex: number) {
      this.cursos.removeAt(courseIndex)
    }

    addFormacao(){
    const formacaoForm =  this.formBuilder.group({
      Nome: ['', Validators.required],
      Instituicao: ['', Validators.required],
      Status: [''],
      DataInicio: [''],
      DataFim: ['']
      });
      this.formacoes.push(formacaoForm);
    }

    deleteFormacao(index: number) {
      this.formacoes.removeAt(index)
    }

    addHabilidade() {
      const form = this.formBuilder.group({
        Nome: ['', Validators.required],
        Tipo: ['', Validators.required],
        Nivel: ['', Validators.required]
      });
      this.habilidades.push(form);
    }

    deleteHabilidade(index: number) {
      this.habilidades.removeAt(index);
    }

    addExperiencia() {
      const form = this.formBuilder.group({
        Empresa: ['', Validators.required],
        Cargo: ['', Validators.required],
        DataInicio: [''],
        DataFim: [''],
        Descricao: ['']
      });
      this.experiencias.push(form);
    }

    deleteExperiencia(index: number) {
      this.experiencias.removeAt(index);
    }

    onError() {
      this.bar.open('Erro ao salvar candidato', '' , {duration: 5000})
    };

    onSucess(){
      this.bar.open('Sucesso ao salvar candidato', '' , {duration: 5000})
    }

    home(){
      this.router.navigate([''], {relativeTo: this.route })
    }



    enviarFormulario(): void {
      if (this.meuFormulario.invalid) {
        this.meuFormulario.markAllAsTouched();
        this.bar.open('Existem campos que não foram corretamente preenchidos.', 'Fechar', { duration: 5000 });
        return;
      } else {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        data: 'Deseja realmente inserir um candidato?'
      });
      dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const obj: Candidato = {
          id: this.perfilId || 0,
          nome: this.Nome.value,
          cpf: this.meuFormulario.get('Cpf')?.value,
          dataNascimento: this.meuFormulario.get('DataNascimento')?.value,
          linkedInUrl: this.meuFormulario.get('LinkedInUrl')?.value,
          resumoProfissional: this.meuFormulario.get('ResumoProfissional')?.value,
          filiacao: {
            nomePai: this.meuFormulario.get('Filiacao.NomePai')?.value,
            nomeMae: this.meuFormulario.get('Filiacao.NomeMae')?.value
          },
          endereco: {
            logradouro: this.meuFormulario.get('Endereco.Logradouro')?.value,
            cep: this.meuFormulario.get('Endereco.Cep')?.value,
            numero: this.meuFormulario.get('Endereco.Numero')?.value,
            bairro: this.meuFormulario.get('Endereco.Bairro')?.value,
            cidade: {
              nome: this.meuFormulario.get('Endereco.Cidade.Nome')?.value,
              estado: {
                nome: this.meuFormulario.get('Endereco.Cidade.Estado.Nome')?.value,
                sigla: this.meuFormulario.get('Endereco.Cidade.Estado.Sigla')?.value
              }
            }
          },
          telefones: this.telefones.controls.map(x => ({ numero: x.get('Numero')?.value, tipo: x.get('Tipo')?.value })),
          cursos: this.cursos.controls.map(x => ({ nome: x.get('Nome')?.value, instituicao: x.get('Instituicao')?.value, anoConclusao: x.get('AnoConclusao')?.value, totalHoras: x.get('TotalHoras')?.value })),
          formacoes: this.formacoes.controls.map(x => ({ nome: x.get('Nome')?.value, instituicao: x.get('Instituicao')?.value, status: x.get('Status')?.value, dataInicio: x.get('DataInicio')?.value, dataFim: x.get('DataFim')?.value })),
          habilidades: this.habilidades.controls.map(x => ({ nome: x.get('Nome')?.value, tipo: x.get('Tipo')?.value, nivel: x.get('Nivel')?.value })),
          experiencias: this.experiencias.controls.map(x => ({ empresa: x.get('Empresa')?.value, cargo: x.get('Cargo')?.value, dataInicio: x.get('DataInicio')?.value, dataFim: x.get('DataFim')?.value, descricao: x.get('Descricao')?.value }))
        };

        const req = this.perfilId ? this.candidatoService.edit(obj, this.perfilId) : this.candidatoService.save(obj);

        req.subscribe(
        () => {
            this.dialog.closeAll();
            this.bar.open(this.perfilId ? 'Candidato atualizado com sucesso!' : 'Candidato inserido com sucesso!', 'Fechar', { duration: 5000 });
            this.home();
        },
          (error) => {
            const backendError = error?.error?.error || error?.error?.Error || error?.error?.message;
            if (backendError) {
              this.bar.open(backendError, 'Fechar', { duration: 8000 });
            } else {
              this.onError();
            }
          }
          );
        }
      });
    }
    }


}



