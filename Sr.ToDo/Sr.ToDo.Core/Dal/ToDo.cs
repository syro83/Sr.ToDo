﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sr.ToDo.Core.Dal
{
	public partial class ToDo : IEntity
	{
		public ToDo()
		{
		}

		public int Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		[Column(TypeName = "ntext")]
		public string Description { get; set; }

		public int? Priority { get; set; }
		public bool? Done { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Updated { get; set; }
	}
}