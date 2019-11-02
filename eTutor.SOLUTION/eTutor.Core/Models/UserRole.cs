using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Enums;

namespace eTutor.Core.Models
{
    /// <summary>
    /// Represents the relationship between user and roles
    /// </summary>
    public sealed class UserRole : EntityBase, IEntityBase
    {
        /// <summary>
        /// Represents the Id of the User
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Represents the User Entity
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Represents the Role ID
        /// </summary>
        public RoleTypes RoleId { get; set; }

        /// <summary>
        /// Represents the Role Entity
        /// </summary>
        public Role Role { get; set; }
    }
}
