using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sr.Reminder.WebApi.Models
{
	public class Reminder
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		public string Description { get; set; }
		public ReminderPriority Priority { get; set; }
		public DateTime DueDate { get; set; }

		public IList<Task> Tasks { get; set; } = new List<Task>();

		public IList<Category> Category { get; set; } = new List<Category>();

		[DefaultValue(false)]
		public bool Done { get; set; }

		[DefaultValue(false)]
		public bool Deleted { get; set; }

		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }

		public override string ToString()
		{
			return string.Format(
				$"({GetType()})[Id={Id},Name={Name},Description={Description},Priority={Priority},DueDate={DueDate},TaskCount={Tasks.Count},CategoryCount={Category.Count},Done={Done},Deleted={Deleted},Created={Created},Updated={Updated}]", this);
		}
	}
}
