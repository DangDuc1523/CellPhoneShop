using CellPhoneShop.Business.DTOs.OData;
using CellPhoneShop.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CellPhoneShop.API.Controllers.OData
{
    [Route("odata/customer")]
    [ApiController]
    public class CustomerODataController : ODataController
    {
        private readonly IODataService _odataService;
        private readonly ILogger<CustomerODataController> _logger;

        public CustomerODataController(IODataService odataService, ILogger<CustomerODataController> logger)
        {
            _odataService = odataService;
            _logger = logger;
        }

        /// <summary>
        /// Get phone variants with OData query support
        /// Supports: $filter, $orderby, $top, $skip, $expand, $select
        /// </summary>
        [HttpGet("phonevariants")]
        [EnableQuery(PageSize = 50, MaxTop = 100)]
        public IQueryable<PhoneVariantODataDto> GetPhoneVariants()
        {
            _logger.LogInformation("OData PhoneVariants query requested");
            return _odataService.GetPhoneVariantsQueryable();
        }

        /// <summary>
        /// Get specific phone variant by ID
        /// </summary>
        [HttpGet("phonevariants({variantId})")]
        [EnableQuery]
        public async Task<ActionResult<PhoneVariantODataDto>> GetPhoneVariant(int variantId)
        {
            _logger.LogInformation("OData PhoneVariant {VariantId} requested", variantId);
            
            var variant = await _odataService.GetPhoneVariantByIdAsync(variantId);
            if (variant == null)
            {
                return NotFound();
            }

            return Ok(variant);
        }

        /// <summary>
        /// Get phone variants by phone ID
        /// </summary>
        [HttpGet("phones({phoneId})/variants")]
        [EnableQuery]
        public IQueryable<PhoneVariantODataDto> GetPhoneVariantsByPhoneId(int phoneId)
        {
            _logger.LogInformation("OData PhoneVariants for Phone {PhoneId} requested", phoneId);
            
            return _odataService.GetPhoneVariantsQueryable()
                .Where(pv => pv.PhoneId == phoneId);
        }

        /// <summary>
        /// Get phone variants by brand ID
        /// </summary>
        [HttpGet("brands({brandId})/phonevariants")]
        [EnableQuery]
        public IQueryable<PhoneVariantODataDto> GetPhoneVariantsByBrandId(int brandId)
        {
            _logger.LogInformation("OData PhoneVariants for Brand {BrandId} requested", brandId);
            
            return _odataService.GetPhoneVariantsQueryable()
                .Where(pv => pv.BrandId == brandId);
        }

        /// <summary>
        /// Get phone variants by color ID
        /// </summary>
        [HttpGet("colors({colorId})/phonevariants")]
        [EnableQuery]
        public IQueryable<PhoneVariantODataDto> GetPhoneVariantsByColorId(int colorId)
        {
            _logger.LogInformation("OData PhoneVariants for Color {ColorId} requested", colorId);
            
            return _odataService.GetPhoneVariantsQueryable()
                .Where(pv => pv.ColorId == colorId);
        }

        /// <summary>
        /// Get available phone variants (in stock)
        /// </summary>
        [HttpGet("phonevariants/available")]
        [EnableQuery]
        public IQueryable<PhoneVariantODataDto> GetAvailablePhoneVariants()
        {
            _logger.LogInformation("OData Available PhoneVariants requested");
            
            return _odataService.GetPhoneVariantsQueryable()
                .Where(pv => pv.Stock > 0 && pv.Status == 1);
        }

        /// <summary>
        /// Get phone variants with discounts
        /// </summary>
        [HttpGet("phonevariants/discounted")]
        [EnableQuery]
        public IQueryable<PhoneVariantODataDto> GetDiscountedPhoneVariants()
        {
            _logger.LogInformation("OData Discounted PhoneVariants requested");
            
            return _odataService.GetPhoneVariantsQueryable()
                .Where(pv => pv.Price < pv.PhoneBasePrice);
        }
    }
} 