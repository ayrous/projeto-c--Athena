/*ATENÇÃO: ESSA CLASE SÓ EXISTE PARA TESTE, ELA ESTAVA PRESENTE NO DO FERNANDO, PORÉM APARENTEMENTE O MODO DE 
CONFIGURAÇÃO DO BANCO DE DADOS É DIFERENTE, PRECISA SER ANALISADO PARA SER IMPEMENTADO NA NOSSA SOLUÇÃO, OU ENTÃO
NEM SER USADO. ELA CONFIGURA AS TABLES QUE VÃO SER SALVAR NO DB.*/
namespace CorujasDev.TodoList.Infra.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class BancoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NomeCompleto = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar", maxLength: 20, nullable: false),
                    Senha = table.Column<string>(type: "varchar", maxLength: 20, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PessoaJuridica",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Cnpj = table.Column<string>(type: "varchar", maxLength: 20, nullable: false),
                    IdAdministrador = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoaJuridica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PessoaJuridica_Administrador_IdAdministrador",
                        column: x => x.IdAdministrador,
                        principalTable: "Administrador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PessoaJuridica_IdUAdministrador",
                table: "PessoaJuridica",
                column: "IdAdministrador");

            migrationBuilder.CreateIndex(
                name: "IX_Administrador_Email",
                table: "Administrador",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PessoaJuridica");

            migrationBuilder.DropTable(
                name: "Administrador");
        }
    }
}
