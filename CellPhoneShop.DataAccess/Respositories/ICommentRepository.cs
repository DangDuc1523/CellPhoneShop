using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Respositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetByPhoneIdAsync(int phoneId);
        Task<Comment> GetByIdAsync(int id);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id, int deletedBy);
    }
}
