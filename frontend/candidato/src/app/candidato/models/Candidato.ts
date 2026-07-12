export interface Candidato {
  id: number;
  nome: string;
  cpf?: string;
  dataNascimento?: string;
  linkedInUrl?: string;
  resumoProfissional?: string;
  filiacao: {nomePai: string, nomeMae: string};
  endereco: {logradouro: string, cep: string, numero: string, bairro: string, cidade: {nome: string, estado:{nome: string, sigla: string}}};
  telefones: {numero: string, tipo: number} [];
  cursos?: { nome: string, instituicao: string, anoConclusao: number, totalHoras: number } [];
  formacoes?: { nome: string, instituicao: string, status: string, dataInicio: string, dataFim: string } [];
  habilidades?: { nome: string, tipo: string, nivel: string } [];
  experiencias?: { empresa: string, cargo: string, dataInicio: string, dataFim: string, descricao: string } [];
}


