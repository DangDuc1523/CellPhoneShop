using CellPhoneShop.DataAccess.Respositories;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CellPhoneShop.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        public CommentService(ICommentRepository repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(Comment comment)
        {
            //comment.CommentId = Guid.NewGuid();
            comment.CreatedAt = DateTime.UtcNow;
            comment.IsDeleted = false;
            await _repository.AddAsync(comment);
        }

        public async Task DeleteAsync(int id, int deletedBy)
        {
            await _repository.DeleteAsync(id, deletedBy);
        }

        public async Task<IEnumerable<Comment>> GetByPhoneIdAsync(int phoneId)
        {
            return await _repository.GetByPhoneIdAsync(phoneId);
        }

        public async Task UpdateAsync(int id, Comment comment)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing != null)
            {
                //existing.Content = updated.Content;
                //existing.Rating = updated.Rating;
                existing.ModifiedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(existing);
            }
        }
    }
}
