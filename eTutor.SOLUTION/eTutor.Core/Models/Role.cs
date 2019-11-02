using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eTutor.Core.Models
{
    /// <summary>
    /// Represents a system role
    /// </summary>
    public sealed class Role : IEntityBase
    {
        /// <summary>
        /// Represents the Id of the Role
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Represents the User's Role
        /// </summary>
        public string Name { get; set; }

        ///<inheritdoc cref="IEntityBase.CreatedDate"/>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        ///<inheritdoc cref="IEntityBase.UpdatedDate"/>
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Represents the user roles
        /// </summary>
        public ISet<UserRole> UserRoles { get; set; }
    }
}
