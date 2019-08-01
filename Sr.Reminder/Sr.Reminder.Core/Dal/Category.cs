using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sr.Reminder.Core.Dal
{
    public partial class Category : IEntity
	{
        public Category()
        {
            ReminderCategory = new HashSet<ReminderCategory>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty(nameof(Dal.ReminderCategory.Category))]
        public virtual ICollection<ReminderCategory> ReminderCategory { get; set; }
    }
}
