using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using eTutor.Core.Enums;

namespace eTutor.Core.Models
{
    /// <summary>
    /// Represents an App User
    /// </summary>
    public sealed class User : EntityBase, IEntityBase
    {
        /// <summary>
        /// Represents the user name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents the user email
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Represents whether the email is validated
        /// </summary>
        public bool IsEmailValidated { get; set; }

        /// <summary>
        /// Represents the user's username
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Represents whether the user is temporary password
        /// </summary>
        public bool IsTemporaryPassword { get; set; }

        /// <summary>
        /// Represents the user's gender
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Represente the users roles
        /// </summary>
        public ISet<UserRole> UserRoles { get; set; }
    }
}
