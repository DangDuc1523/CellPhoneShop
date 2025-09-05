﻿using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class CommentRepository
    {
        private readonly CellPhoneShopContext _context;
        public CommentRepository(CellPhoneShopContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Comment>> GetByPhoneIdAsync(int phoneId)
        {
            return await _context.Comments
                .Where(c => c.PhoneId == phoneId)
                .ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(Guid id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, Guid deletedBy)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                comment.IsDeleted = true;
                comment.DeletedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }
    }


}
