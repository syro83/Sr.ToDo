using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sr.Reminder.Core.Contracts
{
	public interface IUnitOfWork
	{
		IReminderRepository Reminders { get; }

		IToDoRepository ToDos { get; }

		int Commit();
		Task<int> CommitAsync();
		void Rollback();
	}
}
