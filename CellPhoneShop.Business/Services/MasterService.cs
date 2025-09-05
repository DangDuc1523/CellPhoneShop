using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CellPhoneShop.Business.DTOs.Master;
using CellPhoneShop.Business.Interfaces;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;

namespace CellPhoneShop.Business.Services
{
    public class MasterService : IMasterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MasterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MasterDTO>> GetByCategoryAsync(string category)
        {
            var masters = await _unitOfWork.Masters.GetByCategoryAsync(category);
            return _mapper.Map<IEnumerable<MasterDTO>>(masters);
        }

        public async Task<IEnumerable<MasterDTO>> GetByCategoryAndMasterDataIdAsync(string category, int masterDataId)
        {
            var masters = await _unitOfWork.Masters.GetByCategoryAndMasterDataIdAsync(category, masterDataId);
            return _mapper.Map<IEnumerable<MasterDTO>>(masters);
        }
    }
}
