using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crickinfo_mvc_ef_core.Migrations
{
    /// <inheritdoc />
    public partial class Ver6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamTournament_Teams_TeamId",
                table: "TeamTournament");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamTournament_Tournaments_TournamentId",
                table: "TeamTournament");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamTournament",
                table: "TeamTournament");

            migrationBuilder.RenameTable(
                name: "TeamTournament",
                newName: "TeamTournaments");

            migrationBuilder.RenameIndex(
                name: "IX_TeamTournament_TournamentId",
                table: "TeamTournaments",
                newName: "IX_TeamTournaments_TournamentId");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamTournaments",
                table: "TeamTournaments",
                columns: new[] { "TeamId", "TournamentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeamTournaments_Teams_TeamId",
                table: "TeamTournaments",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamTournaments_Tournaments_TournamentId",
                table: "TeamTournaments",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamTournaments_Teams_TeamId",
                table: "TeamTournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamTournaments_Tournaments_TournamentId",
                table: "TeamTournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamTournaments",
                table: "TeamTournaments");

            migrationBuilder.RenameTable(
                name: "TeamTournaments",
                newName: "TeamTournament");

            migrationBuilder.RenameIndex(
                name: "IX_TeamTournaments_TournamentId",
                table: "TeamTournament",
                newName: "IX_TeamTournament_TournamentId");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamTournament",
                table: "TeamTournament",
                columns: new[] { "TeamId", "TournamentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeamTournament_Teams_TeamId",
                table: "TeamTournament",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamTournament_Tournaments_TournamentId",
                table: "TeamTournament",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
