using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sr.Reminder.Core.Repositories
{
	public interface IRepositoryBase<T>
	{
		Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);

		Task Add(T entity);
		Task Update(T entity);
		Task Remove(T entity);

		Task<IEnumerable<T>> GetAll();
		Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

		Task<int> CountAll();
		Task<int> CountWhere(Expression<Func<T, bool>> predicate);
	}
}
