using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CellPhoneShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Master",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterDataId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Master__3214EC07F8ECE0A2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    ProvinceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Province__FD0A6F83240EA978", x => x.ProvinceId);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProvinceId = table.Column<int>(type: "int", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    WardId = table.Column<int>(type: "int", nullable: true),
                    AddressDetail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK__UserAcco__1788CCACCFD2B959", x => x.UserID);
                    table.ForeignKey(
                        name: "FK__UserAccou__Creat__14270015",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__UserAccou__Delet__151B244E",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__UserAccou__Modif__160F4887",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: true),
                    DistrictName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__District__85FDA4C6FF5DADDC", x => x.DistrictId);
                    table.ForeignKey(
                        name: "FK__Districts__Provi__7F2BE32F",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceId");
                });

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    BrandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK__Brand__DAD4F3BEFBB42C55", x => x.BrandID);
                    table.ForeignKey(
                        name: "FK__Brand__CreatedBy__6FE99F9F",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Brand__DeletedBy__70DDC3D8",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Brand__ModifiedB__71D1E811",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__51BCD79752FC7337", x => x.CartID);
                    table.ForeignKey(
                        name: "FK__Cart__UserID__72C60C4A",
                        column: x => x.UserID,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    AuthorID = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__News__954EBDD39F8B436E", x => x.NewsID);
                    table.ForeignKey(
                        name: "FK__News__AuthorID__00200768",
                        column: x => x.AuthorID,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__News__CreatedBy__01142BA1",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__News__DeletedBy__02084FDA",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__News__ModifiedBy__02FC7413",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    WardId = table.Column<int>(type: "int", nullable: true),
                    AddressDetail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__C3905BAF99A18C87", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK__Order__UserID__03F0984C",
                        column: x => x.UserID,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    PromotionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiscountPercent = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
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
                    table.PrimaryKey("PK__Promotio__52C42F2F7EE87D58", x => x.PromotionID);
                    table.ForeignKey(
                        name: "FK__Promotion__Creat__114A936A",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Promotion__Delet__123EB7A3",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Promotion__Modif__1332DBDC",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    WardId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    WardName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Wards__C6BD9BCAA45268A1", x => x.WardId);
                    table.ForeignKey(
                        name: "FK__Wards__DistrictI__17036CC0",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId");
                });

            migrationBuilder.CreateTable(
                name: "Phone",
                columns: table => new
                {
                    PhoneID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandID = table.Column<int>(type: "int", nullable: true),
                    PhoneName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Screen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FrontCamera = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RearCamera = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CPU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Battery = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SIM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Other = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK__Phone__F3EE4BD027224A8E", x => x.PhoneID);
                    table.ForeignKey(
                        name: "FK__Phone__BrandID__06CD04F7",
                        column: x => x.BrandID,
                        principalTable: "Brand",
                        principalColumn: "BrandID");
                    table.ForeignKey(
                        name: "FK__Phone__CreatedBy__07C12930",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Phone__DeletedBy__08B54D69",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Phone__ModifiedB__09A971A2",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ColorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneID = table.Column<int>(type: "int", nullable: true),
                    ColorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
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
                    table.PrimaryKey("PK__Colors__8DA7676D021378B1", x => x.ColorID);
                    table.ForeignKey(
                        name: "FK__Colors__CreatedB__797309D9",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Colors__DeletedB__7A672E12",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Colors__Modified__7B5B524B",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Colors__PhoneID__7C4F7684",
                        column: x => x.PhoneID,
                        principalTable: "Phone",
                        principalColumn: "PhoneID");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comment__C3B4DFAAB173E4F3", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK__Comment__PhoneID__7D439ABD",
                        column: x => x.PhoneID,
                        principalTable: "Phone",
                        principalColumn: "PhoneID");
                    table.ForeignKey(
                        name: "FK__Comment__UserID__7E37BEF6",
                        column: x => x.UserID,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "ColorImages",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorID = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
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
                    table.PrimaryKey("PK__ColorIma__7516F4EC9D9A2882", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK__ColorImag__Color__75A278F5",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ColorID");
                    table.ForeignKey(
                        name: "FK__ColorImag__Creat__76969D2E",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__ColorImag__Delet__778AC167",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__ColorImag__Modif__787EE5A0",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PhoneVariant",
                columns: table => new
                {
                    VariantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneID = table.Column<int>(type: "int", nullable: true),
                    ColorID = table.Column<int>(type: "int", nullable: true),
                    Storage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhoneVar__0EA233E4FF148454", x => x.VariantID);
                    table.ForeignKey(
                        name: "FK__PhoneVari__Color__0C85DE4D",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ColorID");
                    table.ForeignKey(
                        name: "FK__PhoneVari__Creat__0D7A0286",
                        column: x => x.CreatedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PhoneVari__Delet__0E6E26BF",
                        column: x => x.DeletedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PhoneVari__Modif__0F624AF8",
                        column: x => x.ModifiedBy,
                        principalTable: "UserAccount",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PhoneVari__Phone__10566F31",
                        column: x => x.PhoneID,
                        principalTable: "Phone",
                        principalColumn: "PhoneID");
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CartItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartID = table.Column<int>(type: "int", nullable: true),
                    VariantID = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CartItem__488B0B2ACBA0D688", x => x.CartItemID);
                    table.ForeignKey(
                        name: "FK__CartItem__CartID__73BA3083",
                        column: x => x.CartID,
                        principalTable: "Cart",
                        principalColumn: "CartID");
                    table.ForeignKey(
                        name: "FK__CartItem__Varian__74AE54BC",
                        column: x => x.VariantID,
                        principalTable: "PhoneVariant",
                        principalColumn: "VariantID");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    VariantID = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__D3B9D30CCD69F7A1", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__04E4BC85",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Varia__05D8E0BE",
                        column: x => x.VariantID,
                        principalTable: "PhoneVariant",
                        principalColumn: "VariantID");
                });

            migrationBuilder.CreateTable(
                name: "PhonePromotion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VariantID = table.Column<int>(type: "int", nullable: true),
                    PromotionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhonePro__3214EC275DC19495", x => x.ID);
                    table.ForeignKey(
                        name: "FK__PhoneProm__Promo__0A9D95DB",
                        column: x => x.PromotionID,
                        principalTable: "Promotion",
                        principalColumn: "PromotionID");
                    table.ForeignKey(
                        name: "FK__PhoneProm__Varia__0B91BA14",
                        column: x => x.VariantID,
                        principalTable: "PhoneVariant",
                        principalColumn: "VariantID");
                });

            migrationBuilder.CreateTable(
                name: "Warranty",
                columns: table => new
                {
                    WarrantyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VariantID = table.Column<int>(type: "int", nullable: true),
                    WarrantyPeriod = table.Column<int>(type: "int", nullable: true),
                    WarrantyCenter = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Warranty__2ED318F3C9728E3D", x => x.WarrantyID);
                    table.ForeignKey(
                        name: "FK__Warranty__Varian__17F790F9",
                        column: x => x.VariantID,
                        principalTable: "PhoneVariant",
                        principalColumn: "VariantID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brand_CreatedBy",
                table: "Brand",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_DeletedBy",
                table: "Brand",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_ModifiedBy",
                table: "Brand",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserID",
                table: "Cart",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartID",
                table: "CartItem",
                column: "CartID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_VariantID",
                table: "CartItem",
                column: "VariantID");

            migrationBuilder.CreateIndex(
                name: "IX_ColorImages_ColorID",
                table: "ColorImages",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_ColorImages_CreatedBy",
                table: "ColorImages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ColorImages_DeletedBy",
                table: "ColorImages",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ColorImages_ModifiedBy",
                table: "ColorImages",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_CreatedBy",
                table: "Colors",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_DeletedBy",
                table: "Colors",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ModifiedBy",
                table: "Colors",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_PhoneID",
                table: "Colors",
                column: "PhoneID");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PhoneID",
                table: "Comment",
                column: "PhoneID");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserID",
                table: "Comment",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_News_AuthorID",
                table: "News",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_News_CreatedBy",
                table: "News",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_News_DeletedBy",
                table: "News",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_News_ModifiedBy",
                table: "News",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserID",
                table: "Order",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderID",
                table: "OrderDetail",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_VariantID",
                table: "OrderDetail",
                column: "VariantID");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_BrandID",
                table: "Phone",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_CreatedBy",
                table: "Phone",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_DeletedBy",
                table: "Phone",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_ModifiedBy",
                table: "Phone",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PhonePromotion_PromotionID",
                table: "PhonePromotion",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_PhonePromotion_VariantID",
                table: "PhonePromotion",
                column: "VariantID");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneVariant_ColorID",
                table: "PhoneVariant",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneVariant_CreatedBy",
                table: "PhoneVariant",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneVariant_DeletedBy",
                table: "PhoneVariant",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneVariant_ModifiedBy",
                table: "PhoneVariant",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneVariant_PhoneID",
                table: "PhoneVariant",
                column: "PhoneID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_CreatedBy",
                table: "Promotion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_DeletedBy",
                table: "Promotion",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_ModifiedBy",
                table: "Promotion",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_CreatedBy",
                table: "UserAccount",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_DeletedBy",
                table: "UserAccount",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_ModifiedBy",
                table: "UserAccount",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__UserAcco__A9D1053452242E96",
                table: "UserAccount",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wards_DistrictId",
                table: "Wards",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Warranty_VariantID",
                table: "Warranty",
                column: "VariantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "ColorImages");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Master");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "PhonePromotion");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Warranty");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "PhoneVariant");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Phone");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "UserAccount");
        }
    }
}
