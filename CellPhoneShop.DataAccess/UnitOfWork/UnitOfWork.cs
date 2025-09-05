using CellPhoneShop.DataAccess.Respositories;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.DataAccess.Repositories;
using CellPhoneShop.DataAccess.Repositories.Interfaces;

namespace CellPhoneShop.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CellPhoneShopContext _context;

        private IDbContextTransaction _transaction;

        private IMasterRepository _masters;
        private IProvinceRepository _provinces;
        private IDistrictRepository _districts;
        private IWardRepository _wards;
        private IUserRepository _users;
        private IPhoneRepository _phones;
        private IColorRepository _colors;
        private IColorImageRepository _colorImages;
        private IPhoneVariantRepository _phoneVariants;
        private IBrandRepository _brands;

        private ICartItemRepository _cartItemRepository;
        private ICartRepository _cartRepository;
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;

        private IPhoneAttributeMappingRepository _phoneAttributeMappings;

        public UnitOfWork(CellPhoneShopContext context)
        {
            _context = context;
        }

        public IMasterRepository Masters => _masters ??= new MasterRepository(_context);
        public IProvinceRepository Provinces => _provinces ??= new ProvinceRepository(_context);
        public IDistrictRepository Districts => _districts ??= new DistrictRepository(_context);
        public IWardRepository Wards => _wards ??= new WardRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IPhoneRepository Phones => _phones ??= new PhoneRepository(_context);
        public IColorRepository Colors => _colors ??= new ColorRepository(_context);
        public IColorImageRepository ColorImages => _colorImages ??= new ColorImageRepository(_context);
        public IPhoneVariantRepository PhoneVariants => _phoneVariants ??= new PhoneVariantRepository(_context);
        public ICartItemRepository CartItems => _cartItemRepository ??= new CartItemRepository(_context);

        public IOrderDetailRepository OrderDetail =>  _orderDetailRepository ??= new OrderDetailRepository(_context);

        public ICartRepository Carts => _cartRepository ??= new CartRepository(_context);

        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);

        public IBrandRepository Brands => _brands ??= new BrandRepository(_context);

        public IPhoneAttributeMappingRepository PhoneAttributeMappings => _phoneAttributeMappings ??= new PhoneAttributeMappingRepository(_context);

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

