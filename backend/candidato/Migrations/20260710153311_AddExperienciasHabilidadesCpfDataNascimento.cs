using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace candidato.Migrations
{
    /// <inheritdoc />
    public partial class AddExperienciasHabilidadesCpfDataNascimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Candidatos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Candidatos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "Candidatos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumoProfissional",
                table: "Candidatos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Experiencias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Empresa = table.Column<string>(type: "TEXT", nullable: true),
                    Cargo = table.Column<string>(type: "TEXT", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    CandidatoId = table.Column<long>(type: "INTEGER", nullable: false)
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
                name: "Habilidades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    Nivel = table.Column<string>(type: "TEXT", nullable: true),
                    CandidatoId = table.Column<long>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Experiencias_CandidatoId",
                table: "Experiencias",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habilidades_CandidatoId",
                table: "Habilidades",
                column: "CandidatoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experiencias");

            migrationBuilder.DropTable(
                name: "Habilidades");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "ResumoProfissional",
                table: "Candidatos");
        }
    }
}
