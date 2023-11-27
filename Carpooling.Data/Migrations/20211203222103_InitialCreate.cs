using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Carpooling.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PictureUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alternative = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TravelRole = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ImageId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDestination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDestination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Departure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FreeSpots = table.Column<int>(type: "int", nullable: false),
                    TravelStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    FeedbacksCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RatingSum = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TripCandidates",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripCandidates", x => new { x.TripId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TripCandidates_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripCandidates_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripCandidates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TripComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripComments_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "IsDeleted", "TravelRole" },
                values: new object[,]
                {
                    { new Guid("943b692d-330e-405d-a019-c3d728442142"), false, 2 },
                    { new Guid("943b692d-330e-405d-a019-c3d728442144"), false, 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "Image", "ImageId", "LastName", "Password", "PhoneNumber", "Username" },
                values: new object[,]
                {
                    { new Guid("943b692d-330e-405d-a019-c3d728442143"), "stenlyto@abv.bg", "Stanislav", null, null, "Simeonov", "cd5392bb8db39640ea5ee097ce0a832f8e506cc1c0ca685812575acd83ee5a8c", "0854545454", "Stenlyto" },
                    { new Guid("943b692d-330e-405d-a019-c3d728442141"), "miro44@abv.bg", "Stanimir", null, null, "Ivanov", "b6bd0cb173bd0081e448a2400b622f75ddb8cc36c17edebf0ba3d1a08fa164c0", "0864646464", "Mirko" },
                    { new Guid("943b692d-330e-405d-a019-c3d728442145"), "pepi14@abv.bg", "Petko", null, null, "Mitev", "428ed7ff0b40a507792b58445998623722c87818bcbdec0869010e5340df6fb0", "0812122112", "Pepi" },
                    { new Guid("943b692d-330e-405d-a019-c3d728442153"), "gogi@abv.bg", "Georgi", null, null, "Mitev", "2c4950b025be3ac9215b130365e974a9a400fb8b5984f2f31db747b86b962ffd", "0854545459", "gogi" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "Image", "ImageId", "IsAdmin", "LastName", "Password", "PhoneNumber", "Username" },
                values: new object[] { new Guid("943b692d-330e-405d-a019-c3d728442233"), "tisho@abv.bg", "Todor", null, null, true, "Todorov", "d54d0553dd78e8aea5db95ffa176de9019bce947fc02aecca916378ae216a4e3", "0854545453", "tisho" });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "Departure", "DriverId", "EndDestination", "FreeSpots", "IsDeleted", "StartDestination" },
                values: new object[,]
                {
                    { new Guid("943b692d-330e-405d-a019-c3d728442146"), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("943b692d-330e-405d-a019-c3d728442143"), "Plovdiv", 3, false, "Sofia" },
                    { new Guid("943b692d-330e-405d-a019-c3d728442148"), new DateTime(2021, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("943b692d-330e-405d-a019-c3d728442145"), "Sofia", 3, false, "Plovdiv" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("943b692d-330e-405d-a019-c3d728442144"), new Guid("943b692d-330e-405d-a019-c3d728442143") },
                    { new Guid("943b692d-330e-405d-a019-c3d728442142"), new Guid("943b692d-330e-405d-a019-c3d728442143") },
                    { new Guid("943b692d-330e-405d-a019-c3d728442142"), new Guid("943b692d-330e-405d-a019-c3d728442145") },
                    { new Guid("943b692d-330e-405d-a019-c3d728442144"), new Guid("943b692d-330e-405d-a019-c3d728442145") }
                });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "Id", "Description", "IsDeleted", "Rating", "RoleId", "TripId", "UserId" },
                values: new object[,]
                {
                    { new Guid("943b692d-330e-405d-a019-c3d728442151"), null, false, 5, new Guid("943b692d-330e-405d-a019-c3d728442142"), new Guid("943b692d-330e-405d-a019-c3d728442146"), new Guid("943b692d-330e-405d-a019-c3d728442143") },
                    { new Guid("943b692d-330e-405d-a019-c3d728442152"), null, false, 4, new Guid("943b692d-330e-405d-a019-c3d728442144"), new Guid("943b692d-330e-405d-a019-c3d728442146"), new Guid("943b692d-330e-405d-a019-c3d728442141") },
                    { new Guid("943b692d-330e-405d-a019-c3d728442153"), null, false, 3, new Guid("943b692d-330e-405d-a019-c3d728442142"), new Guid("943b692d-330e-405d-a019-c3d728442148"), new Guid("943b692d-330e-405d-a019-c3d728442145") },
                    { new Guid("943b692d-330e-405d-a019-c3d728442154"), null, false, 2, new Guid("943b692d-330e-405d-a019-c3d728442144"), new Guid("943b692d-330e-405d-a019-c3d728442148"), new Guid("943b692d-330e-405d-a019-c3d728442141") },
                    { new Guid("943b692d-330e-405d-a019-c3d728442155"), null, false, 1, new Guid("943b692d-330e-405d-a019-c3d728442144"), new Guid("943b692d-330e-405d-a019-c3d728442148"), new Guid("943b692d-330e-405d-a019-c3d728442153") }
                });

            migrationBuilder.InsertData(
                table: "TripCandidates",
                columns: new[] { "TripId", "UserId", "DriverId", "Id", "IsDeleted" },
                values: new object[] { new Guid("943b692d-330e-405d-a019-c3d728442146"), new Guid("943b692d-330e-405d-a019-c3d728442141"), new Guid("943b692d-330e-405d-a019-c3d728442143"), new Guid("143b692d-330e-405d-a019-c3d728442149"), false });

            migrationBuilder.InsertData(
                table: "TripCandidates",
                columns: new[] { "TripId", "UserId", "DriverId", "Id", "IsApproved", "IsDeleted" },
                values: new object[] { new Guid("943b692d-330e-405d-a019-c3d728442146"), new Guid("943b692d-330e-405d-a019-c3d728442153"), new Guid("943b692d-330e-405d-a019-c3d728442143"), new Guid("143b692d-330e-405d-a019-c3d728442151"), true, false });

            migrationBuilder.InsertData(
                table: "TripCandidates",
                columns: new[] { "TripId", "UserId", "DriverId", "Id", "IsDeleted" },
                values: new object[] { new Guid("943b692d-330e-405d-a019-c3d728442146"), new Guid("943b692d-330e-405d-a019-c3d728442233"), new Guid("943b692d-330e-405d-a019-c3d728442143"), new Guid("143b692d-330e-405d-a019-c3d828442153"), false });

            migrationBuilder.InsertData(
                table: "TripCandidates",
                columns: new[] { "TripId", "UserId", "DriverId", "Id", "IsApproved", "IsDeleted" },
                values: new object[] { new Guid("943b692d-330e-405d-a019-c3d728442148"), new Guid("943b692d-330e-405d-a019-c3d728442141"), new Guid("943b692d-330e-405d-a019-c3d728442145"), new Guid("143b692d-330e-405d-a019-c3d728442150"), true, false });

            migrationBuilder.InsertData(
                table: "TripCandidates",
                columns: new[] { "TripId", "UserId", "DriverId", "Id", "IsDeleted" },
                values: new object[] { new Guid("943b692d-330e-405d-a019-c3d728442148"), new Guid("943b692d-330e-405d-a019-c3d728442153"), new Guid("943b692d-330e-405d-a019-c3d728442145"), new Guid("143b692d-330e-405d-a019-c3d728442152"), false });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_RoleId",
                table: "Feedbacks",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_TripId",
                table: "Feedbacks",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TripCandidates_DriverId",
                table: "TripCandidates",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TripCandidates_UserId",
                table: "TripCandidates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TripComments_TripId",
                table: "TripComments",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DriverId",
                table: "Trips",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ImageId",
                table: "Users",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "TripCandidates");

            migrationBuilder.DropTable(
                name: "TripComments");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
