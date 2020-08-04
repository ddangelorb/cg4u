using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Core.Common.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity<TEntity>
    {
        Task AddAsync(TEntity obj);
        Task UpdateAsync(TEntity obj);
        Task DisableAsync(int id);
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> ListAllAsync();
        Task<bool> IsActiveAsync(int id);
    }
}
