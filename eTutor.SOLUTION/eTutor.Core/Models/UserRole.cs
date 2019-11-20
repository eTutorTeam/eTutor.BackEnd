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
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public User User { get; set; }

        public Role Role { get; set; }
    }
}
