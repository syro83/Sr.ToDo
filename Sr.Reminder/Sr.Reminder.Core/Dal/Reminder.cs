using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sr.Reminder.Core.Dal
{
    public partial class Reminder : IEntity
	{
        public Reminder()
        {
            ReminderCategory = new HashSet<ReminderCategory>();
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        public int? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? Done { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

        [InverseProperty(nameof(Dal.ReminderCategory.Reminder))]
        public virtual ICollection<ReminderCategory> ReminderCategory { get; set; }
        [InverseProperty(nameof(Dal.Task.Reminder))]
        public virtual ICollection<Task> Task { get; set; }
    }
}
