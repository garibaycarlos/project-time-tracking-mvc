using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectTimeTracking.Data.Migrations
{
    public partial class AddProjectTrackerToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectTracker",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ResourceId = table.Column<int>(nullable: false),
                    ProjectStatusId = table.Column<int>(nullable: false),
                    Initiator = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 150, nullable: true),
                    Hours = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    Modifiedby = table.Column<string>(maxLength: 255, nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTracker_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTracker_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTracker_ProjectStatus_ProjectStatusId",
                        column: x => x.ProjectStatusId,
                        principalTable: "ProjectStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTracker_Resource_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTracker_CategoryId",
                table: "ProjectTracker",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTracker_CustomerId",
                table: "ProjectTracker",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTracker_ProjectStatusId",
                table: "ProjectTracker",
                column: "ProjectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTracker_ResourceId",
                table: "ProjectTracker",
                column: "ResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTracker");
        }
    }
}
