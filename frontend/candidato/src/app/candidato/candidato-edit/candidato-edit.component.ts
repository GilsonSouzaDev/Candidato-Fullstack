import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { CandidatosService } from '../services/candidatos.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { Candidato } from '../models/Candidato';
import { MockDataService } from 'src/app/shared/services/mock-data.service';

@Component({
  selector: 'app-candidato-edit',
  templateUrl: './candidato-edit.component.html',
  styleUrls: ['./candidato-edit.component.scss']
})
export class CandidatoEditComponent implements OnInit {

  public candidato: any;
  public meuFormulario: FormGroup;
  public id: number;
  allCursos: string[] = [];
  allHardSkills: string[] = [];
  allSoftSkills: string[] = [];

  constructor(private candidatoService: CandidatosService,
              private route: ActivatedRoute,
              private formBuilder: FormBuilder,
              public dialog: MatDialog,
              private router : Router,
              private bar: MatSnackBar,
              private mockData: MockDataService
              )
  {

    this.id = this.route.snapshot.params['id']

    this.candidatoService.display(this.id).subscribe(
      (response) => {
        this.candidato = response;
        this.preencherFormulario();
        console.log(this.candidato)
      },
      (error) => {
        console.log(error);
      }
    );

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
    this.mockData.getCursos().subscribe(c => this.allCursos = c);
    this.mockData.getHardSkills().subscribe(c => this.allHardSkills = c);
    this.mockData.getSoftSkills().subscribe(c => this.allSoftSkills = c);
  }

  filterCursos(val: string): string[] {
    if (!val) return this.allCursos;
    const lower = val.toLowerCase();
    return this.allCursos.filter(c => c.toLowerCase().includes(lower));
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
  const courseForm = this.formBuilder.group({
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
  const formacaoForm = this.formBuilder.group({
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


  preencherFormulario() {
    let dataNascimentoValue = '';
    if(this.candidato.dataNascimento) {
       // extrair apenas a parte da data, ex: YYYY-MM-DD
       dataNascimentoValue = this.candidato.dataNascimento.split('T')[0];
    }

    this.meuFormulario.patchValue({
      id: this.candidato.id,
      Nome: this.candidato.nome,
      Cpf: this.candidato.cpf,
      DataNascimento: dataNascimentoValue,
      LinkedInUrl: this.candidato.linkedInUrl,
      ResumoProfissional: this.candidato.resumoProfissional,
      Filiacao: {
        id: this.candidato.filiacao.id,
        NomePai: this.candidato.filiacao.nomePai,
        NomeMae: this.candidato.filiacao.nomeMae
      },
      Endereco: {
        id: this.candidato.endereco.id,
        Logradouro: this.candidato.endereco.logradouro,
        Cep: this.candidato.endereco.cep,
        Numero: this.candidato.endereco.numero,
        Bairro: this.candidato.endereco.bairro,
        Cidade: {
          id: this.candidato.endereco.cidade.id,
          Nome: this.candidato.endereco.cidade.nome,
          Estado: {
            id: this.candidato.endereco.cidade.estado.id,
            Nome:this.candidato.endereco.cidade.estado.nome,
            Sigla: this.candidato.endereco.cidade.estado.sigla
          },
        },
      }
    });

    if (this.candidato.telefones && this.candidato.telefones.length > 0) {
      const telefonesFormArray = this.meuFormulario.get('Telefones') as FormArray;
      this.candidato.telefones.forEach((telefone: any) => {
          telefonesFormArray.push(this.formBuilder.group({
            id: [telefone.id],
            Numero: [telefone.numero, Validators.required],
            Tipo: [telefone.tipo, Validators.required]
          }));
        });
      }

    if (this.candidato.cursos && this.candidato.cursos.length > 0) {
      const cursosFormArray = this.meuFormulario.get('Cursos') as FormArray;
      this.candidato.cursos.forEach((curso: any) => {
        cursosFormArray.push(this.formBuilder.group({
          id: [curso.id],
          Nome: [curso.nome, Validators.required],
          Instituicao: [curso.instituicao],
          AnoConclusao: [curso.anoConclusao],
          TotalHoras: [curso.totalHoras]
        }));
      });
    }

    if (this.candidato.formacoes && this.candidato.formacoes.length > 0) {
      const formacoesFormArray = this.meuFormulario.get('Formacoes') as FormArray;
      this.candidato.formacoes.forEach((formacao: any) => {
        let inicio = formacao.dataInicio ? formacao.dataInicio.split('T')[0] : '';
        let fim = formacao.dataFim ? formacao.dataFim.split('T')[0] : '';
        formacoesFormArray.push(this.formBuilder.group({
          id: [formacao.id],
          Nome: [formacao.nome, Validators.required],
          Instituicao: [formacao.instituicao, Validators.required],
          Status: [formacao.status],
          DataInicio: [inicio],
          DataFim: [fim]
        }));
      });
    }

    if (this.candidato.habilidades && this.candidato.habilidades.length > 0) {
      const habilidadesFormArray = this.meuFormulario.get('Habilidades') as FormArray;
      this.candidato.habilidades.forEach((hab: any) => {
        habilidadesFormArray.push(this.formBuilder.group({
          id: [hab.id],
          Nome: [hab.nome, Validators.required],
          Tipo: [hab.tipo, Validators.required],
          Nivel: [hab.nivel, Validators.required]
        }));
      });
    }

    if (this.candidato.experiencias && this.candidato.experiencias.length > 0) {
      const experienciasFormArray = this.meuFormulario.get('Experiencias') as FormArray;
      this.candidato.experiencias.forEach((exp: any) => {
        let inicio = exp.dataInicio ? exp.dataInicio.split('T')[0] : '';
        let fim = exp.dataFim ? exp.dataFim.split('T')[0] : '';
        experienciasFormArray.push(this.formBuilder.group({
          id: [exp.id],
          Empresa: [exp.empresa, Validators.required],
          Cargo: [exp.cargo, Validators.required],
          DataInicio: [inicio],
          DataFim: [fim],
          Descricao: [exp.descricao]
        }));
      });
    }

  }


  onError() {
    this.bar.open('Erro ao salvar candidato', '' , {duration: 5000})
  };

  onSucess(){
    this.bar.open('Sucesso ao salvar candidato', '' , {duration: 5000})
  }

  home(){
    this.router.navigate(['/candidato/display', this.id])
  }



  enviarFormulario() {
    if (this.meuFormulario.invalid) {
      this.bar.open('Existem campos que não foram corretamente preenchidos .', 'Fechar');
      return;
    } else {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        data: 'Deseja realmente alterar este candidato?'
      });
      dialogRef.afterClosed().subscribe((result) => {
      if (result) {
      // Mapear de volta a estrutura para a API
      const obj = {
          id: this.id,
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

      this.candidatoService.edit(obj, this.id).subscribe(
        (sucess) => {
          this.dialog.closeAll();
          this.bar.open('Candidato editado com sucesso!', 'Fechar', { duration: 5000 });
          this.home();
      },
        (error) => {
          this.onError();
          if (error && error.errors && error.errors.CandidatoModel) {
            const candidatoModelError = error.errors.CandidatoModel;
            this.bar.open('Erro ao editar candidato. Tente novamente mais tarde.', 'Fechar');
            }
          }
        );
      }
    });
    }
  }

}
