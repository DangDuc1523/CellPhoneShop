using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetByPhoneIdAsync(int phoneId);
        Task AddAsync(Comment comment);
        Task UpdateAsync(int id, Comment comment);
        Task DeleteAsync(int id, int deletedBy);
    }
}
