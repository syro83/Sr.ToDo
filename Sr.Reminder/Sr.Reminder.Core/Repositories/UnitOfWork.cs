using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sr.Reminder.Core.Contracts;

namespace Sr.Reminder.Core.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		protected Dal.SrReminderContext _context;

		public IReminderRepository Reminders { get; private set; }

		public IToDoRepository ToDos { get; private set; }

		public UnitOfWork(Dal.SrReminderContext context)
		{

			this._context = context;
			Reminders = new ReminderRepository(context);
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
