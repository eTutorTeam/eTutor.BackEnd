using System;
using System.Collections.Generic;
using System.Text;

namespace eTutor.Core.Models
{
    public interface IEntityBase
    {
        /// <summary>
        /// Represents the Entity's Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Represents the date of creation
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Represents the date it was updated
        /// </summary>
        DateTime UpdatedDate { get; set; }
    }
}
