using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sr.Reminder.Core.Contracts;
using Sr.Reminder.Core.Dal;

namespace Sr.Reminder.Core.Repositories
{
	public class ToDoRepository : RepositoryBase<Dal.ToDo>, IToDoRepository
	{
		public ToDoRepository(Dal.SrReminderContext context) : base(context)
		{
		}

	}
}
