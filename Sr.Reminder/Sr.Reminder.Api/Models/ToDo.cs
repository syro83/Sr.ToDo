using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sr.Reminder.WebApi.Models
{
	public class ToDo
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		public string Description { get; set; }

		public int Priority { get; set; }

		[DefaultValue(false)]
		public bool Done { get; set; }

		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }

		public override string ToString()
		{
			return string.Format(
				$"({GetType()})[Id={Id},Name={Name},Description={Description},Priority={Priority},Done={Done},Created={Created},Updated={Updated}]", this);
		}
	}
}
