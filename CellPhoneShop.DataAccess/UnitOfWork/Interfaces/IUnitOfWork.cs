using CellPhoneShop.DataAccess.Respositories;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.DataAccess.Repositories.Interfaces;

namespace CellPhoneShop.DataAccess.UnitOfWork.Interfaces
{
    public interface    IUnitOfWork : IDisposable
    {
        ICartItemRepository CartItems { get; }
        IOrderDetailRepository OrderDetail { get; }
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }
        IMasterRepository Masters { get; }
        IProvinceRepository Provinces { get; }
        IDistrictRepository Districts { get; }
        IWardRepository Wards { get; }
        IUserRepository Users { get; }
        IPhoneRepository Phones { get; }
        IColorRepository Colors { get; }
        IColorImageRepository ColorImages { get; }
        IPhoneVariantRepository PhoneVariants { get; }
        IPhoneAttributeMappingRepository PhoneAttributeMappings { get; }

        IBrandRepository Brands { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task<int> SaveChangesAsync();
        Task CommitAsync();
        Task RollbackAsync();

    }
}
