using CellPhoneShop.Business.Interfaces;
using CellPhoneShop.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CellPhoneShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly IWardService _wardService;
        private readonly IMasterService _masterService;

        public LocationController(
            IProvinceService provinceService,
            IDistrictService districtService,
            IWardService wardService,
            IMasterService masterService)
        {
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;
            _masterService = masterService;
        }

        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var provinces = await _provinceService.GetAllProvincesAsync();
            return Ok(provinces);
        }

        [HttpGet("provinces/{id}")]
        public async Task<IActionResult> GetProvince(int id)
        {
            var province = await _provinceService.GetProvinceByIdAsync(id);
            return province == null ? NotFound() : Ok(province);
        }

        [HttpGet("districts/province/{provinceId}")]
        public async Task<IActionResult> GetDistricts(int provinceId)
        {
            var districts = await _districtService.GetDistrictsByProvinceIdAsync(provinceId);
            return Ok(districts);
        }

        [HttpGet("wards/district/{districtId}")]
        public async Task<IActionResult> GetWards(int districtId)
        {
            var wards = await _wardService.GetWardsByDistrictIdAsync(districtId);
            return Ok(wards);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var masters = await _masterService.GetByCategoryAsync(category);
            return Ok(masters);
        }

        [HttpGet("category/{category}/masterDataId/{masterDataId}")]
        public async Task<IActionResult> GetByCategoryAndMasterDataId(string category, int masterDataId)
        {
            var masters = await _masterService.GetByCategoryAndMasterDataIdAsync(category, masterDataId);
            return Ok(masters);
        }
    }
}
