using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace candidato.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioToCandidato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UsuarioId",
                table: "Candidatos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_UsuarioId",
                table: "Candidatos",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatos_Usuarios_UsuarioId",
                table: "Candidatos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatos_Usuarios_UsuarioId",
                table: "Candidatos");

            migrationBuilder.DropIndex(
                name: "IX_Candidatos_UsuarioId",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Candidatos");
        }
    }
}
