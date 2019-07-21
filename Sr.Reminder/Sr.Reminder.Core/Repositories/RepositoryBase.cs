using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sr.Reminder.Core.Dal;
using Task = System.Threading.Tasks.Task;

namespace Sr.Reminder.Core.Repositories
{
	public class RepositoryBase<T> : IRepositoryBase<T> where T : class
	{
		protected Dal.SrReminderContext Context;

		public RepositoryBase(Dal.SrReminderContext context)
		{
			Context = context;
		}

		public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
		{
			return Context.Set<T>().FirstOrDefaultAsync(predicate);
		}

		public async Task Add(T entity)
		{
			// await Context.AddAsync(entity);
			await Context.Set<T>().AddAsync(entity);
			await Context.SaveChangesAsync();
		}

		public Task Update(T entity)
		{
			// In case AsNoTracking is used
			Context.Entry(entity).State = EntityState.Modified;
			return Context.SaveChangesAsync();
		}

		public Task Remove(T entity)
		{
			Context.Set<T>().Remove(entity);
			return Context.SaveChangesAsync();
		}

		public async Task<IEnumerable<T>> GetAll()
		{
			return await Context.Set<T>().ToListAsync();
		}

		public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
		{
			return await Context.Set<T>().Where(predicate).ToListAsync();
		}

		public Task<int> CountAll()
		{
			return Context.Set<T>().CountAsync();
		}

		public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
		{
			return Context.Set<T>().CountAsync(predicate);
		}
	}
}
