using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace candidato.Migrations
{
    /// <inheritdoc />
    public partial class AddJobDetailsToVaga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Atividades",
                table: "Vagas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Beneficios",
                table: "Vagas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeEmpresa",
                table: "Vagas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequisitosDesejaveis",
                table: "Vagas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequisitosObrigatorios",
                table: "Vagas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Atividades",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "Beneficios",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "NomeEmpresa",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "RequisitosDesejaveis",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "RequisitosObrigatorios",
                table: "Vagas");
        }
    }
}
