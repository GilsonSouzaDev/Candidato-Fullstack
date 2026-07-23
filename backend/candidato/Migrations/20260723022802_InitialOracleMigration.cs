using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace candidato.Migrations
{
    /// <inheritdoc />
    public partial class InitialOracleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    EstadoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
                    Sigla = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.EstadoId);
                });

            migrationBuilder.CreateTable(
                name: "Filiacoes",
                columns: table => new
                {
                    FiliacaoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NomePai = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true),
                    NomeMae = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiacoes", x => x.FiliacaoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SenhaHash = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Role = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cidades",
                columns: table => new
                {
                    CidadeId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
                    EstadoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidades", x => x.CidadeId);
                    table.ForeignKey(
                        name: "FK_Cidades_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vagas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Titulo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Requisitos = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NomeEmpresa = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Beneficios = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Atividades = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    RequisitosObrigatorios = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    RequisitosDesejaveis = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Salario = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    StatusAberta = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CriadoPorId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vagas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vagas_Usuarios_CriadoPorId",
                        column: x => x.CriadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    EnderecoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Logradouro = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
                    Cep = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    Numero = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Bairro = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CidadeId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.EnderecoId);
                    table.ForeignKey(
                        name: "FK_Enderecos_Cidades_CidadeId",
                        column: x => x.CidadeId,
                        principalTable: "Cidades",
                        principalColumn: "CidadeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    CandidatoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
                    Cpf = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ResumoProfissional = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FiliacaoId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EnderecoId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    UsuarioId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.CandidatoId);
                    table.ForeignKey(
                        name: "FK_Candidatos_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "EnderecoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidatos_Filiacoes_FiliacaoId",
                        column: x => x.FiliacaoId,
                        principalTable: "Filiacoes",
                        principalColumn: "FiliacaoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidatos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candidaturas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CandidatoId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    VagaId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    StatusCandidatura = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataAplicacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidaturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidaturas_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "CandidatoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidaturas_Vagas_VagaId",
                        column: x => x.VagaId,
                        principalTable: "Vagas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    CursoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
                    Instituicao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    AnoConclusao = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TotalHoras = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CandidatoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.CursoId);
                    table.ForeignKey(
                        name: "FK_Cursos_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "CandidatoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiencias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Empresa = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Cargo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DataFim = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Descricao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CandidatoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiencias_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "CandidatoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Formacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Instituicao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Status = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DataFim = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CandidatoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Formacoes_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "CandidatoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Habilidades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Tipo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Nivel = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CandidatoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habilidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Habilidades_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "CandidatoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telefones",
                columns: table => new
                {
                    TelefoneId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Numero = table.Column<string>(type: "VARCHAR(15)", maxLength: 15, nullable: false),
                    Tipo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CandidatoId = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefones", x => x.TelefoneId);
                    table.ForeignKey(
                        name: "FK_Telefones_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "CandidatoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_EnderecoId",
                table: "Candidatos",
                column: "EnderecoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_FiliacaoId",
                table: "Candidatos",
                column: "FiliacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_UsuarioId",
                table: "Candidatos",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_CandidatoId",
                table: "Candidaturas",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_VagaId",
                table: "Candidaturas",
                column: "VagaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cidades_EstadoId",
                table: "Cidades",
                column: "EstadoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_CandidatoId",
                table: "Cursos",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_CidadeId",
                table: "Enderecos",
                column: "CidadeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Experiencias_CandidatoId",
                table: "Experiencias",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Formacoes_CandidatoId",
                table: "Formacoes",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habilidades_CandidatoId",
                table: "Habilidades",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefones_CandidatoId",
                table: "Telefones",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_CriadoPorId",
                table: "Vagas",
                column: "CriadoPorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidaturas");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Experiencias");

            migrationBuilder.DropTable(
                name: "Formacoes");

            migrationBuilder.DropTable(
                name: "Habilidades");

            migrationBuilder.DropTable(
                name: "Telefones");

            migrationBuilder.DropTable(
                name: "Vagas");

            migrationBuilder.DropTable(
                name: "Candidatos");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "Filiacoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "Estados");
        }
    }
}
