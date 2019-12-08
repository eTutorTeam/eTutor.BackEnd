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

        /// <summary>
        /// Represents the user name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the LastName
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Represents the personal Id
        /// </summary>
        public string PersonalId { get; set; }

        /// <summary>
        /// Represents whether the user is active or not
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Represents whether the email is validated
        /// </summary>
        public bool IsEmailValidated { get; set; }

        /// <summary>
        /// Represents whether the user is temporary password
        /// </summary>
        public bool IsTemporaryPassword { get; set; }

        /// <summary>
        /// Represents the user's gender
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Represents the written address of the user
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Represents the user's home Longitude Coordinates
        /// </summary>
        public float? Longitude { get; set; }
        
        /// <summary>
        /// Represents the user's home Latitude Coordinates
        /// </summary>
        public float? Latitude { get; set; }
        
        public DateTime BirthDate { get; set; }

        public string FullName => Name + " " + LastName;

        public ISet<UserRole> UserRoles { get; set; }
        
        public ISet<UserClaim> UserClaims { get; set; }
        
        public ISet<UserLogin> UserLogins { get; set; }
        
        public ISet<UserToken> UserTokens { get; set; }

        public ISet<ParentStudent> Students { get; set; }
        
        public ISet<ParentStudent> Parents { get; set; }
        
        public ISet<TutorSubject> TutorSubjects { get; set; }
        
        public ISet<ParentAutorization> Autorizations { get; set; }
        
        public ISet<Invoice> Invoices { get; set; }
        
        public ISet<Rating> Ratings { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        
    }
}
