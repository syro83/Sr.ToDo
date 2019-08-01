using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sr.Reminder.Core.Dal
{
    public partial class ReminderCategory
    {
        public int ReminderId { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(Dal.Category.ReminderCategory))]
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(ReminderId))]
        [InverseProperty(nameof(Dal.Reminder.ReminderCategory))]
        public virtual Reminder Reminder { get; set; }
    }
}
