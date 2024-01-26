using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestDb34.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: false),
                    Received = table.Column<bool>(type: "boolean", nullable: false),
                    touserid = table.Column<int>(name: "to_user_id", type: "integer", nullable: true),
                    fromuserid = table.Column<int>(name: "from_user_id", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("message_pkey", x => x.id);
                    table.ForeignKey(
                        name: "messages_from_user_id_fkey",
                        column: x => x.fromuserid,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "messages_to_user_id_fkey",
                        column: x => x.touserid,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_from_user_id",
                table: "Messages",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_to_user_id",
                table: "Messages",
                column: "to_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
