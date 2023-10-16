using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProspectLabApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLeafletNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeafletName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeafletName",
                table: "Products");
        }
    }
}
