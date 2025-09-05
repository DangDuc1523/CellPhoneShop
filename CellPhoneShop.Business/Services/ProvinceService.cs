using AutoMapper;
using CellPhoneShop.Business.DTOs.Location;
using CellPhoneShop.Business.Interfaces;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;

namespace CellPhoneShop.Business.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProvinceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProvinceDTO>> GetAllProvincesAsync()
        {
            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            return _mapper.Map<IEnumerable<ProvinceDTO>>(provinces);
        }

        public async Task<ProvinceDTO?> GetProvinceByIdAsync(int id)
        {
            var province = await _unitOfWork.Provinces.GetByIdAsync(id);
            return _mapper.Map<ProvinceDTO>(province);
        }
    }
}
