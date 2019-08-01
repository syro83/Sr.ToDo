using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sr.Reminder.WebApi.Models
{
	public enum ReminderPriority : byte
	{
		Unset = 0,
		Low = 1,
		Medium = 2,
		High = 3,
		TopPrio = 5
	}
}
