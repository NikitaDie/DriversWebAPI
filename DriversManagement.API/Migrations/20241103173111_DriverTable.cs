using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriversManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class DriverTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_VehicleCategory_CategoryId",
                table: "Drivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleCategory",
                table: "VehicleCategory");

            migrationBuilder.RenameTable(
                name: "VehicleCategory",
                newName: "VehicleCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleCategories",
                table: "VehicleCategories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Engine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuelCapacity = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                table: "Vehicles",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_VehicleCategories_CategoryId",
                table: "Drivers",
                column: "CategoryId",
                principalTable: "VehicleCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_VehicleCategories_CategoryId",
                table: "Drivers");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleCategories",
                table: "VehicleCategories");

            migrationBuilder.RenameTable(
                name: "VehicleCategories",
                newName: "VehicleCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleCategory",
                table: "VehicleCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_VehicleCategory_CategoryId",
                table: "Drivers",
                column: "CategoryId",
                principalTable: "VehicleCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
