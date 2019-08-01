using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sr.Reminder.WebApi.Models
{
	public class Task
	{
		public int Id { get; set; }
		public string Description { get; set; }

		public int ReminderId { get; set; }
		public Reminder Reminder { get; set; }

		public override string ToString()
		{
			return string.Format(
				$"({GetType()})[Id={Id},Description={Description}", this);
		}
	}
}
