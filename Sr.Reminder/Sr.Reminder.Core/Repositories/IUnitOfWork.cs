using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sr.Reminder.Core.Repositories
{
	public interface IUnitOfWork
	{
		IReminderRepository Reminders { get; }

		int Commit();
		Task<int> CommitAsync();
		void Rollback();
	}
}
