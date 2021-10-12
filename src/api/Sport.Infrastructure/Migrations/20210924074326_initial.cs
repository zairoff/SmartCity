using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventSubscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSubscribers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pockets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pockets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SportEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplexId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SportTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplexId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vacancies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplexId = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacancies_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SportGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SportGroups_SportTypes_SportTypeId",
                        column: x => x.SportTypeId,
                        principalTable: "SportTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplexId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SportTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trainers_SportTypes_SportTypeId",
                        column: x => x.SportTypeId,
                        principalTable: "SportTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VacancyId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applicants_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplexId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    PocketId = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainees_Pockets_PocketId",
                        column: x => x.PocketId,
                        principalTable: "Pockets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trainees_SportGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "SportGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainerGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerGroups_SportGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "SportGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerGroups_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportEventId = table.Column<int>(type: "int", nullable: false),
                    TraineeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventParticipants_SportEvents_SportEventId",
                        column: x => x.SportEventId,
                        principalTable: "SportEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipants_Trainees_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Trainees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventWinners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParticipantId = table.Column<int>(type: "int", nullable: false),
                    Place = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventWinners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventWinners_EventParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "EventParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Pockets",
                columns: new[] { "Id", "Name", "PricePerMonth" },
                values: new object[,]
                {
                    { 1, "A", 10m },
                    { 2, "B", 120m },
                    { 3, "C", 123m }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Director" },
                    { 2, "Accountant" },
                    { 3, "Trainer" }
                });

            migrationBuilder.InsertData(
                table: "SportTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Football" },
                    { 2, "Backetball" },
                    { 3, "Tennis" }
                });

            migrationBuilder.InsertData(
                table: "SportGroups",
                columns: new[] { "Id", "Name", "SportTypeId" },
                values: new object[] { 1, "A", 1 });

            migrationBuilder.InsertData(
                table: "SportGroups",
                columns: new[] { "Id", "Name", "SportTypeId" },
                values: new object[] { 2, "B", 1 });

            migrationBuilder.InsertData(
                table: "SportGroups",
                columns: new[] { "Id", "Name", "SportTypeId" },
                values: new object[] { 3, "C", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_VacancyId",
                table: "Applicants",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionId",
                table: "Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_SportEventId",
                table: "EventParticipants",
                column: "SportEventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_TraineeId",
                table: "EventParticipants",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventWinners_ParticipantId",
                table: "EventWinners",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_SportGroups_SportTypeId",
                table: "SportGroups",
                column: "SportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_GroupId",
                table: "Trainees",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_PocketId",
                table: "Trainees",
                column: "PocketId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerGroups_GroupId",
                table: "TrainerGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerGroups_TrainerId",
                table: "TrainerGroups",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_EmployeeId",
                table: "Trainers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_SportTypeId",
                table: "Trainers",
                column: "SportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_PositionId",
                table: "Vacancies",
                column: "PositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "EventSubscribers");

            migrationBuilder.DropTable(
                name: "EventWinners");

            migrationBuilder.DropTable(
                name: "TrainerGroups");

            migrationBuilder.DropTable(
                name: "Vacancies");

            migrationBuilder.DropTable(
                name: "EventParticipants");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "SportEvents");

            migrationBuilder.DropTable(
                name: "Trainees");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Pockets");

            migrationBuilder.DropTable(
                name: "SportGroups");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "SportTypes");
        }
    }
}
