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
    /// Represents the relationship between user and roles
    /// </summary>
    public sealed class UserRole : IdentityUserRole<int>, IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Represents the User Entity
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Represents the Role Entity
        /// </summary>
        public Role Role { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
