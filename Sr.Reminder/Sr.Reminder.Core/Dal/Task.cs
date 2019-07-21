using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sr.Reminder.Core.Dal
{
    public partial class Task : IEntity
	{
        public int Id { get; set; }
        public int ReminderId { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [ForeignKey(nameof(ReminderId))]
        [InverseProperty(nameof(Dal.Reminder.Task))]
        public virtual Reminder Reminder { get; set; }
    }
}
