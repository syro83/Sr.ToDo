using System.Threading.Tasks;
using Sr.ToDo.Core.Contracts;

namespace Sr.ToDo.Core.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		protected Dal.SrToDoContext _context;

		public IToDoRepository ToDos { get; private set; }

		public UnitOfWork(Dal.SrToDoContext context)
		{
			this._context = context;
			ToDos = new ToDoRepository(context);
		}

		public int Commit()
		{
			return _context.SaveChanges();
		}

		public async Task<int> CommitAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Rollback()
		{
			_context.Dispose();
		}
	}
}