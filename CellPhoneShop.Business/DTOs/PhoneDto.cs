using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Business.DTOs
{
    public class PhoneDto
    {
        public int PhoneId { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        public string PhoneName { get; set; }
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<PhoneAttributeMappingDto> AttributeMappings { get; set; }
    }

    public class CreatePhoneDto
    {
        [Required(ErrorMessage = "Brand ID is required")]
        public int? BrandId { get; set; }

        [Required(ErrorMessage = "Phone name is required")]
        [StringLength(200, ErrorMessage = "Phone name cannot exceed 200 characters")]
        public string PhoneName { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Base price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Base price must be greater than or equal to 0")]
        public decimal BasePrice { get; set; }

        public List<PhoneAttributeMappingDto> AttributeMappings { get; set; }
    }

    public class UpdatePhoneDto
    {
        [Required(ErrorMessage = "Brand ID is required")]
        public int? BrandId { get; set; }

        [Required(ErrorMessage = "Phone name is required")]
        [StringLength(200, ErrorMessage = "Phone name cannot exceed 200 characters")]
        public string PhoneName { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Base price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Base price must be greater than or equal to 0")]
        public decimal BasePrice { get; set; }

        public List<PhoneAttributeMappingDto> AttributeMappings { get; set; }
    }

    public class PhoneSearchDto
    {
        public string? SearchTerm { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Ram { get; set; }
        public string? Os { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool IsAscending { get; set; } = true;
    }

    public class PhoneSearchResultDto
    {
        public IEnumerable<PhoneDto> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
} 