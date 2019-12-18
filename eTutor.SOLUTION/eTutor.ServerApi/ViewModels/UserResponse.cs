using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Enums;

namespace eTutor.ServerApi.ViewModels
{
    public class UserResponse
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
        /// Represents the user's email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Represents the user's aboutMe
        /// </summary>
        public string AboutMe { get; set; }

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
        
        /// <summary>
        /// Represents the birthdate for the user
        /// </summary>
        public DateTime BirthDate { get; set; }
        
        /// <summary>
        /// Represents the url to the user's profile image
        /// </summary>
        public string ProfileImageUrl { get; set; }

        public string FullName => Name + " " + LastName;

    }
}
