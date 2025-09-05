using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CellPhoneShop.Domain.Models;

public partial class CellPhoneShopContext : DbContext
{
    public CellPhoneShopContext()
    {
    }

    public CellPhoneShopContext(DbContextOptions<CellPhoneShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<ColorImage> ColorImages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Master> Masters { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<PhoneAttribute> PhoneAttributes { get; set; }

    public virtual DbSet<PhoneAttributeMapping> PhoneAttributeMappings { get; set; }

    public virtual DbSet<PhonePromotion> PhonePromotions { get; set; }

    public virtual DbSet<PhoneVariant> PhoneVariants { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<VariantAttribute> VariantAttributes { get; set; }

    public virtual DbSet<VariantAttributeValue> VariantAttributeValues { get; set; }

    public virtual DbSet<VariantAttributeMapping> VariantAttributeMappings { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    public virtual DbSet<Warranty> Warranties { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(local);Database=CellPhoneShop;User Id=sa;Password=123;Encrypt=false;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__DAD4F3BEFBB42C55");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.BrandName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.LogoUrl).HasMaxLength(250);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BrandCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Brand__CreatedBy__6FE99F9F");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.BrandDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__Brand__DeletedBy__70DDC3D8");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BrandModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Brand__ModifiedB__71D1E811");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD79752FC7337");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cart__UserID__72C60C4A");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B2ACBA0D688");

            entity.ToTable("CartItem");

            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.VariantId).HasColumnName("VariantID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK__CartItem__CartID__73BA3083");

            entity.HasOne(d => d.Variant).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK__CartItem__Varian__74AE54BC");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Colors__8DA7676D021378B1");

            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.ColorName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(250);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.PhoneId).HasColumnName("PhoneID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ColorCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Colors__CreatedB__797309D9");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ColorDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__Colors__DeletedB__7A672E12");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ColorModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Colors__Modified__7B5B524B");

            entity.HasOne(d => d.Phone).WithMany(p => p.Colors)
                .HasForeignKey(d => d.PhoneId)
                .HasConstraintName("FK__Colors__PhoneID__7C4F7684");
        });

        modelBuilder.Entity<ColorImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ColorIma__7516F4EC9D9A2882");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(250);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Color).WithMany(p => p.ColorImages)
                .HasForeignKey(d => d.ColorId)
                .HasConstraintName("FK__ColorImag__Color__75A278F5");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ColorImageCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__ColorImag__Creat__76969D2E");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ColorImageDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__ColorImag__Delet__778AC167");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ColorImageModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__ColorImag__Modif__787EE5A0");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFAAB173E4F3");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.Content).HasMaxLength(500);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.PhoneId).HasColumnName("PhoneID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Phone).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PhoneId)
                .HasConstraintName("FK__Comment__PhoneID__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Comment__UserID__7E37BEF6");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PK__District__85FDA4C6FF5DADDC");

            entity.Property(e => e.DistrictId).ValueGeneratedNever();
            entity.Property(e => e.DistrictName).HasMaxLength(100);

            entity.HasOne(d => d.Province).WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK__Districts__Provi__7F2BE32F");
        });

        modelBuilder.Entity<Master>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Master__3214EC07F8ECE0A2");

            entity.ToTable("Master");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDD39F8B436E");

            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Author).WithMany(p => p.NewsAuthors)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__News__AuthorID__00200768");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NewsCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__News__CreatedBy__01142BA1");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.NewsDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__News__DeletedBy__02084FDA");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.NewsModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__News__ModifiedBy__02FC7413");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF99A18C87");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.AddressDetail).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__UserID__03F0984C");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30CCD69F7A1");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VariantId).HasColumnName("VariantID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__04E4BC85");

            entity.HasOne(d => d.Variant).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK__OrderDeta__Varia__05D8E0BE");
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.PhoneId).HasName("PK__Phone__F3EE4BD027224A8E");
            entity.ToTable("Phone");
            entity.Property(e => e.PhoneId).HasColumnName("PhoneID");
            entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.PhoneName).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(d => d.Brand).WithMany(p => p.Phones)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Phone__BrandID__06CD04F7");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PhoneCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Phone__CreatedBy__07C12930");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.PhoneDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__Phone__DeletedBy__08B54D69");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PhoneModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Phone__ModifiedB__09A971A2");
        });

        modelBuilder.Entity<PhonePromotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhonePro__3214EC275DC19495");

            entity.ToTable("PhonePromotion");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");
            entity.Property(e => e.VariantId).HasColumnName("VariantID");

            entity.HasOne(d => d.Promotion).WithMany(p => p.PhonePromotions)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__PhoneProm__Promo__0A9D95DB");

            entity.HasOne(d => d.Variant).WithMany(p => p.PhonePromotions)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK__PhoneProm__Varia__0B91BA14");
        });

        modelBuilder.Entity<PhoneVariant>(entity =>
        {
            entity.HasKey(e => e.VariantId).HasName("PK__PhoneVar__0EA233E4FF148454");
            entity.ToTable("PhoneVariant");
            
            entity.Property(e => e.VariantId).HasColumnName("VariantID");
            entity.Property(e => e.PhoneId).HasColumnName("PhoneID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Stock).HasColumnType("int");
            entity.Property(e => e.Sku).HasMaxLength(50);
            entity.Property(e => e.Status).HasColumnType("int");
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Phone).WithMany(p => p.PhoneVariants)
                .HasForeignKey(d => d.PhoneId)
                .HasConstraintName("FK__PhoneVari__Phone__NEW25");

            entity.HasOne(d => d.Color).WithMany(p => p.PhoneVariants)
                .HasForeignKey(d => d.ColorId)
                .HasConstraintName("FK__PhoneVari__Color__NEW26");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PhoneVariantCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__PhoneVari__Creat__NEW27");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PhoneVariantModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__PhoneVari__Modif__NEW28");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.PhoneVariantDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__PhoneVari__Delet__NEW29");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__52C42F2F7EE87D58");

            entity.ToTable("Promotion");

            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PromotionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Promotion__Creat__114A936A");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.PromotionDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__Promotion__Delet__123EB7A3");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PromotionModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Promotion__Modif__1332DBDC");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__Province__FD0A6F83240EA978");

            entity.Property(e => e.ProvinceId).ValueGeneratedNever();
            entity.Property(e => e.ProvinceName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CCACCFD2B959");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.Email, "UQ__UserAcco__A9D1053452242E96").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AddressDetail).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__UserAccou__Creat__14270015");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.InverseDeletedByNavigation)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__UserAccou__Delet__151B244E");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InverseModifiedByNavigation)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__UserAccou__Modif__160F4887");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.WardId).HasName("PK__Wards__C6BD9BCAA45268A1");

            entity.Property(e => e.WardId).ValueGeneratedNever();
            entity.Property(e => e.WardName).HasMaxLength(100);

            entity.HasOne(d => d.District).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Wards__DistrictI__17036CC0");
        });

        modelBuilder.Entity<Warranty>(entity =>
        {
            entity.HasKey(e => e.WarrantyId).HasName("PK__Warranty__2ED318F3C9728E3D");

            entity.ToTable("Warranty");

            entity.Property(e => e.WarrantyId).HasColumnName("WarrantyID");
            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.VariantId).HasColumnName("VariantID");
            entity.Property(e => e.WarrantyCenter).HasMaxLength(255);

            entity.HasOne(d => d.Variant).WithMany(p => p.Warranties)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK__Warranty__Varian__17F790F9");
        });

        modelBuilder.Entity<PhoneAttribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId).HasName("PK__PhoneAtt__C218054B7F60ED59");
            entity.ToTable("PhoneAttribute");
            
            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PhoneAttributeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__PhoneAttr__Creat__NEW1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PhoneAttributeModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__PhoneAttr__Modif__NEW2");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.PhoneAttributeDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__PhoneAttr__Delet__NEW3");
        });

        modelBuilder.Entity<PhoneAttributeMapping>(entity =>
        {
            entity.HasKey(e => new { e.PhoneId, e.AttributeId }).HasName("PK__PhoneAtt__NEW4");
            entity.ToTable("PhoneAttributeMapping");
            
            entity.Property(e => e.PhoneId).HasColumnName("PhoneID");
            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.Value).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Phone).WithMany(p => p.PhoneAttributeMappings)
                .HasForeignKey(d => d.PhoneId)
                .HasConstraintName("FK__PhoneAttr__Phone__NEW5");

            entity.HasOne(d => d.Attribute).WithMany(p => p.PhoneAttributeMappings)
                .HasForeignKey(d => d.AttributeId)
                .HasConstraintName("FK__PhoneAttr__Attri__NEW6");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PhoneAttributeMappingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__PhoneAttr__Creat__NEW7");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PhoneAttributeMappingModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__PhoneAttr__Modif__NEW8");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.PhoneAttributeMappingDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__PhoneAttr__Delet__NEW9");
        });

        modelBuilder.Entity<VariantAttribute>(entity =>
        {
            entity.HasKey(e => e.VariantAttributeId).HasName("PK__VariantA__NEW10");
            entity.ToTable("VariantAttribute");
            
            entity.Property(e => e.VariantAttributeId).HasColumnName("VariantAttributeID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VariantAttributeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__VariantAt__Creat__NEW11");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.VariantAttributeModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__VariantAt__Modif__NEW12");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.VariantAttributeDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__VariantAt__Delet__NEW13");
        });

        modelBuilder.Entity<VariantAttributeValue>(entity =>
        {
            entity.HasKey(e => e.ValueId).HasName("PK__VariantA__NEW14");
            entity.ToTable("VariantAttributeValue");
            
            entity.Property(e => e.ValueId).HasColumnName("ValueID");
            entity.Property(e => e.VariantAttributeId).HasColumnName("VariantAttributeID");
            entity.Property(e => e.Value).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.VariantAttribute).WithMany(p => p.VariantAttributeValues)
                .HasForeignKey(d => d.VariantAttributeId)
                .HasConstraintName("FK__VariantAt__Varia__NEW15");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VariantAttributeValueCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__VariantAt__Creat__NEW16");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.VariantAttributeValueModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__VariantAt__Modif__NEW17");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.VariantAttributeValueDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__VariantAt__Delet__NEW18");
        });

        modelBuilder.Entity<VariantAttributeMapping>(entity =>
        {
            entity.HasKey(e => new { e.VariantId, e.ValueId }).HasName("PK__VariantA__NEW19");
            entity.ToTable("VariantAttributeMapping");
            
            entity.Property(e => e.VariantId).HasColumnName("VariantID");
            entity.Property(e => e.ValueId).HasColumnName("ValueID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.PhoneVariant).WithMany(p => p.VariantAttributeMappings)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK__VariantAt__Varia__NEW20");

            entity.HasOne(d => d.AttributeValue).WithMany(p => p.VariantAttributeMappings)
                .HasForeignKey(d => d.ValueId)
                .HasConstraintName("FK__VariantAt__Value__NEW21");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VariantAttributeMappingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__VariantAt__Creat__NEW22");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.VariantAttributeMappingModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__VariantAt__Modif__NEW23");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.VariantAttributeMappingDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__VariantAt__Delet__NEW24");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
