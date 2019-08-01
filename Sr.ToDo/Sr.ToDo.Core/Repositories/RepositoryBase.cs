using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sr.ToDo.Core.Contracts;
using Sr.ToDo.Core.Dal;
using Task = System.Threading.Tasks.Task;

namespace Sr.ToDo.Core.Repositories
{
	public class RepositoryBase<T> : IRepositoryBase<T> where T : class
	{
		protected Dal.SrToDoContext _context;
		protected readonly DbSet<T> _entities;

		public RepositoryBase(Dal.SrToDoContext context)
		{
			_context = context;
			_entities = _context.Set<T>();
		}

		public ValueTask<T> Get(params object[] keyValues)
		{
			return _entities.FindAsync(keyValues);
		}

		public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
		{
			return _entities.FirstOrDefaultAsync(predicate);
		}

		public async Task<IEnumerable<T>> GetAll()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
		{
			return await _context.Set<T>().Where(predicate).ToListAsync();
		}

		public async void Add(T entity)
		{
			_entities.Add(entity);
		}

		public void Remove(T entity)
		{
			_entities.Remove(entity);
		}		
	}
}
