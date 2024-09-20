using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ciclo.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAnotacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anotacoes_Usuarios_UsuarioId",
                table: "Anotacoes");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Anotacoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Anotacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AtualizadoEm",
                table: "Anotacoes",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<int>(
                name: "AtualizadoPor",
                table: "Anotacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CriadoEm",
                table: "Anotacoes",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<int>(
                name: "CriadoPor",
                table: "Anotacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Anotacoes",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Anotacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Anotacoes_Usuarios_UsuarioId",
                table: "Anotacoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anotacoes_Usuarios_UsuarioId",
                table: "Anotacoes");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Anotacoes");

            migrationBuilder.DropColumn(
                name: "AtualizadoEm",
                table: "Anotacoes");

            migrationBuilder.DropColumn(
                name: "AtualizadoPor",
                table: "Anotacoes");

            migrationBuilder.DropColumn(
                name: "CriadoEm",
                table: "Anotacoes");

            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "Anotacoes");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Anotacoes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Anotacoes");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Anotacoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Anotacoes_Usuarios_UsuarioId",
                table: "Anotacoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
