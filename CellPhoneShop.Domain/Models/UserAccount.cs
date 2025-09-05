using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class UserAccount
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public int? WardId { get; set; }

    public string? AddressDetail { get; set; }

    public int Role { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Brand> BrandCreatedByNavigations { get; set; } = new List<Brand>();

    public virtual ICollection<Brand> BrandDeletedByNavigations { get; set; } = new List<Brand>();

    public virtual ICollection<Brand> BrandModifiedByNavigations { get; set; } = new List<Brand>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Color> ColorCreatedByNavigations { get; set; } = new List<Color>();

    public virtual ICollection<Color> ColorDeletedByNavigations { get; set; } = new List<Color>();

    public virtual ICollection<ColorImage> ColorImageCreatedByNavigations { get; set; } = new List<ColorImage>();

    public virtual ICollection<ColorImage> ColorImageDeletedByNavigations { get; set; } = new List<ColorImage>();

    public virtual ICollection<ColorImage> ColorImageModifiedByNavigations { get; set; } = new List<ColorImage>();

    public virtual ICollection<Color> ColorModifiedByNavigations { get; set; } = new List<Color>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual UserAccount? CreatedByNavigation { get; set; }

    public virtual UserAccount? DeletedByNavigation { get; set; }

    public virtual ICollection<UserAccount> InverseCreatedByNavigation { get; set; } = new List<UserAccount>();

    public virtual ICollection<UserAccount> InverseDeletedByNavigation { get; set; } = new List<UserAccount>();

    public virtual ICollection<UserAccount> InverseModifiedByNavigation { get; set; } = new List<UserAccount>();

    public virtual UserAccount? ModifiedByNavigation { get; set; }

    public virtual ICollection<News> NewsAuthors { get; set; } = new List<News>();

    public virtual ICollection<News> NewsCreatedByNavigations { get; set; } = new List<News>();

    public virtual ICollection<News> NewsDeletedByNavigations { get; set; } = new List<News>();

    public virtual ICollection<News> NewsModifiedByNavigations { get; set; } = new List<News>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Phone> PhoneCreatedByNavigations { get; set; } = new List<Phone>();

    public virtual ICollection<Phone> PhoneDeletedByNavigations { get; set; } = new List<Phone>();

    public virtual ICollection<Phone> PhoneModifiedByNavigations { get; set; } = new List<Phone>();

    public virtual ICollection<PhoneVariant> PhoneVariantCreatedByNavigations { get; set; } = new List<PhoneVariant>();

    public virtual ICollection<PhoneVariant> PhoneVariantDeletedByNavigations { get; set; } = new List<PhoneVariant>();

    public virtual ICollection<PhoneVariant> PhoneVariantModifiedByNavigations { get; set; } = new List<PhoneVariant>();

    public virtual ICollection<Promotion> PromotionCreatedByNavigations { get; set; } = new List<Promotion>();

    public virtual ICollection<Promotion> PromotionDeletedByNavigations { get; set; } = new List<Promotion>();

    public virtual ICollection<Promotion> PromotionModifiedByNavigations { get; set; } = new List<Promotion>();


    // New navigation properties for PhoneAttribute
    public virtual ICollection<PhoneAttribute> PhoneAttributeCreatedByNavigations { get; set; } = new List<PhoneAttribute>();

    public virtual ICollection<PhoneAttribute> PhoneAttributeModifiedByNavigations { get; set; } = new List<PhoneAttribute>();

    public virtual ICollection<PhoneAttribute> PhoneAttributeDeletedByNavigations { get; set; } = new List<PhoneAttribute>();

    // New navigation properties for PhoneAttributeMapping
    public virtual ICollection<PhoneAttributeMapping> PhoneAttributeMappingCreatedByNavigations { get; set; } = new List<PhoneAttributeMapping>();

    public virtual ICollection<PhoneAttributeMapping> PhoneAttributeMappingModifiedByNavigations { get; set; } = new List<PhoneAttributeMapping>();

    public virtual ICollection<PhoneAttributeMapping> PhoneAttributeMappingDeletedByNavigations { get; set; } = new List<PhoneAttributeMapping>();

    // New navigation properties for VariantAttribute
    public virtual ICollection<VariantAttribute> VariantAttributeCreatedByNavigations { get; set; } = new List<VariantAttribute>();

    public virtual ICollection<VariantAttribute> VariantAttributeModifiedByNavigations { get; set; } = new List<VariantAttribute>();

    public virtual ICollection<VariantAttribute> VariantAttributeDeletedByNavigations { get; set; } = new List<VariantAttribute>();

    // New navigation properties for VariantAttributeValue
    public virtual ICollection<VariantAttributeValue> VariantAttributeValueCreatedByNavigations { get; set; } = new List<VariantAttributeValue>();

    public virtual ICollection<VariantAttributeValue> VariantAttributeValueModifiedByNavigations { get; set; } = new List<VariantAttributeValue>();

    public virtual ICollection<VariantAttributeValue> VariantAttributeValueDeletedByNavigations { get; set; } = new List<VariantAttributeValue>();

    // New navigation properties for VariantAttributeMapping
    public virtual ICollection<VariantAttributeMapping> VariantAttributeMappingCreatedByNavigations { get; set; } = new List<VariantAttributeMapping>();

    public virtual ICollection<VariantAttributeMapping> VariantAttributeMappingModifiedByNavigations { get; set; } = new List<VariantAttributeMapping>();

    public virtual ICollection<VariantAttributeMapping> VariantAttributeMappingDeletedByNavigations { get; set; } = new List<VariantAttributeMapping>();
}

