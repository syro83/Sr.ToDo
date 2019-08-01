using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sr.Reminder.WebApi.Models
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		//public IList<Reminder> Reminders { get; set; } = new List<Reminder>();

		public override string ToString()
		{
			return string.Format(
				$"({GetType()})[Id={Id},Name={Name}", this);
		}
	}
}
