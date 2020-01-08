using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using eTutor.Core.Helpers;

namespace eTutor.Core.Models
{
    /// <summary>
    /// Represents the Entity Base
    /// </summary>
    public class EntityBase : IEntityBase
    {
        /// <summary>
        /// Represents the Primary Key of every table
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Represents the date of creation
        /// </summary>

        public DateTime CreatedDate { get; set; } = DateTime.Now.GetNowInCorrectTimezone();

        /// <summary>
        /// Represents the date it was updated
        /// </summary>

        public DateTime UpdatedDate { get; set; } = DateTime.Now.GetNowInCorrectTimezone();
    }
}
