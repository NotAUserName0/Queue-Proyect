using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts
{
    public interface IUserRepository
    {
        Task<bool> CreateAsync(UserEntity user, CancellationToken cancellationToken = default);
        Task<(IEnumerable<UserEntity> Users, int TotalRecords)> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<UserEntity?> GetByIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(UserEntity user, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(UserEntity user, CancellationToken cancellationToken = default);
    }
}
