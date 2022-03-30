using Ardalis.Specification;
using ChatVia.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Domain.Interfaces
{
    public interface IEfRepository<T>
            where T : EntityBase, IAggregateRoot
    {
        Task<IReadOnlyList<T>> GetListAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync<TPrimary>(TPrimary id, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync<TPrimary>(TPrimary id, ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<T> GetFirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> DeleteAsync<TPrimary>(TPrimary id, CancellationToken cancellationToken = default);
        Task DeleteEntityAsync<E>(E entity, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync<TPrimary>(TPrimary id, T entity, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<int> CountAsync();
        Task<int> SaveChangesAsync();
    }
}
