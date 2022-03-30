using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ChatVia.Domain.Base;
using ChatVia.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfRepository<T> : IEfRepository<T>
            where T : EntityBase, IAggregateRoot
    {
        private readonly DbSet<T> _entity;

        private readonly AppDbContext _context;

        public EfRepository(AppDbContext context)
        {
            _entity = context.Set<T>();
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetListAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _entity.ToListAsync(cancellationToken);
            return entities;
        }

        public async Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var results = ApplySpecification(specification);
            return await results.ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync<TPrimary>(TPrimary id, CancellationToken cancellationToken = default)
        {
            var entity = await _entity.FindAsync(id);
            return entity;
        }

        public Task<T> GetByIdAsync<TPrimary>(TPrimary id, ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var results = ApplySpecification(specification);
            return results.FirstOrDefaultAsync(r => r.Id == id.ToString(), cancellationToken);
        }

        public Task<T> GetFirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var results = ApplySpecification(specification);
            return results.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var results = await _entity.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync();
            return results.Entity;
        }

        public async Task<T> DeleteAsync<TPrimary>(TPrimary id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id);

            if (entity is not null)
            {
                var results = _entity.Remove(entity);
                await _context.SaveChangesAsync();
                return results.Entity;
            }

            return default(T);
        }

        public async Task DeleteEntityAsync<E>(E entity, CancellationToken cancellationToken = default)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync<TPrimary>(TPrimary id, T entity, CancellationToken cancellationToken = default)
        {
            var entityById = await GetByIdAsync(id);

            if (entityById is not null)
            {
                _context.Entry(entityById).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entity;
            }

            return default(T);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, 
            CancellationToken cancellationToken = default)
            => await _entity.AnyAsync(predicate, cancellationToken);

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, 
            ISpecification<T> specification, 
            CancellationToken cancellationToken = default)
        {
            var results = ApplySpecification(specification);
            return results.AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync() => await _entity.CountAsync();

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(_entity.AsQueryable(), spec);
        }
    }
}
