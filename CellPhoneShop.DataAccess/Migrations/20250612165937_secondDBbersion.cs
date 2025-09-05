using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CellPhoneShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class secondDBbersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Color__0C85DE4D",
                table: "PhoneVariant");

            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Creat__0D7A0286",
                table: "PhoneVariant");

            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Delet__0E6E26BF",
                table: "PhoneVariant");

            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Modif__0F624AF8",
                table: "PhoneVariant");

            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Phone__10566F31",
                table: "PhoneVariant");

            migrationBuilder.DropColumn(
                name: "Storage",
                table: "PhoneVariant");

            migrationBuilder.DropColumn(
                name: "Battery",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "CPU",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "FrontCamera",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "OS",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "RAM",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "RearCamera",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "SIM",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "Screen",
                table: "Phone");

            migrationBuilder.RenameColumn(
                name: "SKU",
                table: "PhoneVariant",
                newName: "Sku");

            migrationBuilder.AlterColumn<string>(
                name: "Sku",
                table: "PhoneVariant",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Phone",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PhoneAttribute",
                columns: table => new
                {
                    AttributeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhoneAtt__C218054B7F60ED59", x => x.AttributeID);
                    table.ForeignKey(
                        name: "FK__PhoneAttr__Creat__NEW1",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PhoneAttr__Delet__NEW3",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PhoneAttr__Modif__NEW2",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "VariantAttribute",
                columns: table => new
                {
                    VariantAttributeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VariantA__NEW10", x => x.VariantAttributeID);
                    table.ForeignKey(
                        name: "FK__VariantAt__Creat__NEW11",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__VariantAt__Delet__NEW13",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__VariantAt__Modif__NEW12",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PhoneAttributeMapping",
                columns: table => new
                {
                    PhoneID = table.Column<int>(type: "int", nullable: false),
                    AttributeID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhoneAtt__NEW4", x => new { x.PhoneID, x.AttributeID });
                    table.ForeignKey(
                        name: "FK__PhoneAttr__Attri__NEW6",
                        column: x => x.AttributeID,
                        principalTable: "PhoneAttribute",
                        principalColumn: "AttributeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__PhoneAttr__Creat__NEW7",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PhoneAttr__Delet__NEW9",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PhoneAttr__Modif__NEW8",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PhoneAttr__Phone__NEW5",
                        column: x => x.PhoneID,
                        principalTable: "Phone",
                        principalColumn: "PhoneID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VariantAttributeValue",
                columns: table => new
                {
                    ValueID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VariantAttributeID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VariantA__NEW14", x => x.ValueID);
                    table.ForeignKey(
                        name: "FK__VariantAt__Creat__NEW16",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__VariantAt__Delet__NEW18",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__VariantAt__Modif__NEW17",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__VariantAt__Varia__NEW15",
                        column: x => x.VariantAttributeID,
                        principalTable: "VariantAttribute",
                        principalColumn: "VariantAttributeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VariantAttributeMapping",
                columns: table => new
                {
                    VariantID = table.Column<int>(type: "int", nullable: false),
                    ValueID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VariantA__NEW19", x => new { x.VariantID, x.ValueID });
                    table.ForeignKey(
                        name: "FK__VariantAt__Creat__NEW22",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__VariantAt__Delet__NEW24",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__VariantAt__Modif__NEW23",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__VariantAt__Value__NEW21",
                        column: x => x.ValueID,
                        principalTable: "VariantAttributeValue",
                        principalColumn: "ValueID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__VariantAt__Varia__NEW20",
                        column: x => x.VariantID,
                        principalTable: "PhoneVariant",
                        principalColumn: "VariantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhoneAttribute_CreatedBy",
                table: "PhoneAttribute",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneAttribute_DeletedBy",
                table: "PhoneAttribute",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneAttribute_ModifiedBy",
                table: "PhoneAttribute",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneAttributeMapping_AttributeID",
                table: "PhoneAttributeMapping",
                column: "AttributeID");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneAttributeMapping_CreatedBy",
                table: "PhoneAttributeMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneAttributeMapping_DeletedBy",
                table: "PhoneAttributeMapping",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneAttributeMapping_ModifiedBy",
                table: "PhoneAttributeMapping",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttribute_CreatedBy",
                table: "VariantAttribute",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttribute_DeletedBy",
                table: "VariantAttribute",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttribute_ModifiedBy",
                table: "VariantAttribute",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeMapping_CreatedBy",
                table: "VariantAttributeMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeMapping_DeletedBy",
                table: "VariantAttributeMapping",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeMapping_ModifiedBy",
                table: "VariantAttributeMapping",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeMapping_ValueID",
                table: "VariantAttributeMapping",
                column: "ValueID");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeValue_CreatedBy",
                table: "VariantAttributeValue",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeValue_DeletedBy",
                table: "VariantAttributeValue",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeValue_ModifiedBy",
                table: "VariantAttributeValue",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeValue_VariantAttributeID",
                table: "VariantAttributeValue",
                column: "VariantAttributeID");

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Color__NEW26",
                table: "PhoneVariant",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ColorID");

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Creat__NEW27",
                table: "PhoneVariant",
                column: "CreatedBy",
                principalTable: "UserAccount",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Delet__NEW29",
                table: "PhoneVariant",
                column: "DeletedBy",
                principalTable: "UserAccount",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Modif__NEW28",
                table: "PhoneVariant",
                column: "ModifiedBy",
                principalTable: "UserAccount",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Phone__NEW25",
                table: "PhoneVariant",
                column: "PhoneID",
                principalTable: "Phone",
                principalColumn: "PhoneID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Color__NEW26",
                table: "PhoneVariant");

            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Creat__NEW27",
                table: "PhoneVariant");

            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Delet__NEW29",
                table: "PhoneVariant");

            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Modif__NEW28",
                table: "PhoneVariant");

            migrationBuilder.DropForeignKey(
                name: "FK__PhoneVari__Phone__NEW25",
                table: "PhoneVariant");

            migrationBuilder.DropTable(
                name: "PhoneAttributeMapping");

            migrationBuilder.DropTable(
                name: "VariantAttributeMapping");

            migrationBuilder.DropTable(
                name: "PhoneAttribute");

            migrationBuilder.DropTable(
                name: "VariantAttributeValue");

            migrationBuilder.DropTable(
                name: "VariantAttribute");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "PhoneVariant",
                newName: "SKU");

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "PhoneVariant",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Storage",
                table: "PhoneVariant",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Phone",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Battery",
                table: "Phone",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CPU",
                table: "Phone",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontCamera",
                table: "Phone",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OS",
                table: "Phone",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Other",
                table: "Phone",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RAM",
                table: "Phone",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RearCamera",
                table: "Phone",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SIM",
                table: "Phone",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Screen",
                table: "Phone",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Color__0C85DE4D",
                table: "PhoneVariant",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ColorID");

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Creat__0D7A0286",
                table: "PhoneVariant",
                column: "CreatedBy",
                principalTable: "UserAccount",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Delet__0E6E26BF",
                table: "PhoneVariant",
                column: "DeletedBy",
                principalTable: "UserAccount",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Modif__0F624AF8",
                table: "PhoneVariant",
                column: "ModifiedBy",
                principalTable: "UserAccount",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__PhoneVari__Phone__10566F31",
                table: "PhoneVariant",
                column: "PhoneID",
                principalTable: "Phone",
                principalColumn: "PhoneID");
        }
    }
}
