using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SWN.MobileService.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MessageText = table.Column<string>(nullable: false),
                    MessageTransactionId = table.Column<long>(nullable: false),
                    ContactPointId = table.Column<long>(nullable: false),
                    RecipientId = table.Column<long>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    MessageType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileRecipients",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MobileUserId = table.Column<long>(nullable: false),
                    MessageDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileRecipients_MessageDetails_MessageDetailId",
                        column: x => x.MessageDetailId,
                        principalTable: "MessageDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MobileRecipients_MessageDetailId",
                table: "MobileRecipients",
                column: "MessageDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobileRecipients");

            migrationBuilder.DropTable(
                name: "MessageDetails");
        }
    }
}
