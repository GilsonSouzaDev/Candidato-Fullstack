using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace candidato.Migrations
{
    /// <inheritdoc />
    public partial class AddFormacaoAndUpdateCurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CandidatoId",
                table: "Cursos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnoConclusao",
                table: "Cursos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instituicao",
                table: "Cursos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalHoras",
                table: "Cursos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Formacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Instituicao = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CandidatoId = table.Column<long>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Formacoes_CandidatoId",
                table: "Formacoes",
                column: "CandidatoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Formacoes");

            migrationBuilder.DropColumn(
                name: "AnoConclusao",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "Instituicao",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "TotalHoras",
                table: "Cursos");

            migrationBuilder.AlterColumn<long>(
                name: "CandidatoId",
                table: "Cursos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");
        }
    }
}
