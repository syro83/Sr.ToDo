using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sr.Reminder.Core.Repositories
{
	public interface IRepositoryBase<T> where T : class
	{
		ValueTask<T> Get(params object[] keyValues);
		Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
		Task<IEnumerable<T>> GetAll();
		Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);

		void Add(T entity);

		void Remove(T entity);
	}
}
