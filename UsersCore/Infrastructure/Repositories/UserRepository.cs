using Application.Models.Dto;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            await _db.Users.AddAsync(user, cancellationToken);
            var changes = await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            var isSuccess = changes > 0;
            return isSuccess;
        }

        public async Task<(IEnumerable<UserEntity> Users, int TotalRecords)> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 50 : pageSize; // tope de 200 registros por página
            var query = _db.Users.AsNoTracking().OrderBy(c => c.Id);
            var totalRecords = await query.CountAsync(cancellationToken).ConfigureAwait(false);
            var users = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            return (users, totalRecords);
        }

        public async Task<UserEntity?> GetByIdAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(c => c.Id == userId, cancellationToken).ConfigureAwait(false);

            return user;
        }

        public async Task<bool> UpdateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            _db.Users.Update(user);
            var changes = await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            var isSuccess = changes > 0;
            return isSuccess;
        }

        public async Task<bool> DeleteAsync(UserEntity user, CancellationToken cancellationToken)
        {
            _db.Users.Remove(user);
            var changes = await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            var isSuccess = changes > 0;
            return isSuccess;
        }
    }
}
