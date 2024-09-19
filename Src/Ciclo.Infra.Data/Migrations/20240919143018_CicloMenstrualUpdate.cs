using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ciclo.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CicloMenstrualUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CicloMenstrual_Usuarios_UsuarioId",
                table: "CicloMenstrual");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CicloMenstrual",
                table: "CicloMenstrual");

            migrationBuilder.RenameTable(
                name: "CicloMenstrual",
                newName: "CicloMenstruals");

            migrationBuilder.RenameIndex(
                name: "IX_CicloMenstrual_UsuarioId",
                table: "CicloMenstruals",
                newName: "IX_CicloMenstruals_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CicloMenstruals",
                table: "CicloMenstruals",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Anotacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anotacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anotacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Anotacoes_UsuarioId",
                table: "Anotacoes",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_CicloMenstruals_Usuarios_UsuarioId",
                table: "CicloMenstruals",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CicloMenstruals_Usuarios_UsuarioId",
                table: "CicloMenstruals");

            migrationBuilder.DropTable(
                name: "Anotacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CicloMenstruals",
                table: "CicloMenstruals");

            migrationBuilder.RenameTable(
                name: "CicloMenstruals",
                newName: "CicloMenstrual");

            migrationBuilder.RenameIndex(
                name: "IX_CicloMenstruals_UsuarioId",
                table: "CicloMenstrual",
                newName: "IX_CicloMenstrual_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CicloMenstrual",
                table: "CicloMenstrual",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CicloMenstrual_Usuarios_UsuarioId",
                table: "CicloMenstrual",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
