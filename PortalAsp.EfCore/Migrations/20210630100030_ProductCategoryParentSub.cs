using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalAsp.EfCore.Migrations
{
    public partial class ProductCategoryParentSub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_CatalogSubCategories_CatalogSubCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_CatalogSubCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "CatalogSubCategoryId",
                table: "ProductCategories");

            migrationBuilder.AddColumn<long>(
                name: "ParentSubCategoryCatalogSubCategoryId",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ParentSubCategoryCatalogSubCategoryId",
                table: "ProductCategories",
                column: "ParentSubCategoryCatalogSubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_CatalogSubCategories_ParentSubCategoryCatalogSubCategoryId",
                table: "ProductCategories",
                column: "ParentSubCategoryCatalogSubCategoryId",
                principalTable: "CatalogSubCategories",
                principalColumn: "CatalogSubCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_CatalogSubCategories_ParentSubCategoryCatalogSubCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_ParentSubCategoryCatalogSubCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ParentSubCategoryCatalogSubCategoryId",
                table: "ProductCategories");

            migrationBuilder.AddColumn<long>(
                name: "CatalogSubCategoryId",
                table: "ProductCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CatalogSubCategoryId",
                table: "ProductCategories",
                column: "CatalogSubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_CatalogSubCategories_CatalogSubCategoryId",
                table: "ProductCategories",
                column: "CatalogSubCategoryId",
                principalTable: "CatalogSubCategories",
                principalColumn: "CatalogSubCategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
