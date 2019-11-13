using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Persistence
{
    public class BaseRepository<T> : IGenericRepository<T> where T : class, IEntityBase, new()
    {
        private readonly ETutorContext _context;

        public BaseRepository(ETutorContext context)
        {
            _context = context;
            
            Set = _context.Set<T>();
        }

        public DbSet<T> Set { get; }
        
        public IOperationResult<T> Create(T entity)
        {
            Set.Add(entity);
            
            return BasicOperationResult<T>.Ok(entity);
        }

        public Task<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = Set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return queryable.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> Get()
        {
            IEnumerable<T> list = await Set.ToListAsync();

            return list;
        }

        public IOperationResult<T> Update(T entity)
        {
            var entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            
            return BasicOperationResult<T>.Ok(entity);
        }

        public IOperationResult<T> Remove(T entity)
        {
            _context.Remove(entity);
            
            return BasicOperationResult<T>.Ok(entity);
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate)
        {
            return await Set.Where(predicate).ToListAsync();
        }

        public Task Save() => _context.SaveChangesAsync();

        public Task<bool> Exists(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = Set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {     
                queryable = queryable.Include(include);
            }

            return queryable.AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = Set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return await queryable.Where(predicate).ToListAsync();
        }
    }
}