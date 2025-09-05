using CellPhoneShop.Domain.Models;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Respositories.Interfaces
{
    public interface  ICartRepository :IRepository<Cart>
    {
        Task<Cart> GetActiveCartByUserIdAsync(int userId);
        Task<Cart> GetOrCreateCartForUserAsync(int userId);
    }
}
