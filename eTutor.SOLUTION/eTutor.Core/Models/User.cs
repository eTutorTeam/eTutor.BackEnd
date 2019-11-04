using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using eTutor.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace eTutor.Core.Models
{
    /// <summary>
    /// Represents an App User
    /// </summary>
    public sealed class User : IdentityUser<int>, IEntityBase
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        
        /// <summary>
        /// Represents the user name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the LastName
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Represents whether the email is validated
        /// </summary>
        public bool IsEmailValidated { get; set; }

        /// <summary>
        /// Represents whether the user is temporary password
        /// </summary>
        public bool IsTemporaryPassword { get; set; }

        /// <summary>
        /// Represents the user's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Represents the user's gender
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Represente the users roles
        /// </summary>
        public ISet<UserRole> UserRoles { get; set; }
        
        public Student Student { get; set; }
        
        public Parent Parent { get; set; }
        
        public Tutor Tutor { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
