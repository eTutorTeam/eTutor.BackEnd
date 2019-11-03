using System;
using System.Collections.Generic;
using eTutor.Core.Enums;

namespace eTutor.Core.Models
{
    public sealed class Parent : EntityBase, IEntityBase
    {
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        public string Address { get; set; }
        
        public string Name { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime BirthDate { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public Gender Gender { get; set; }
        
        public ISet<ParentStudent> Students { get; set; }
        
        public ISet<ParentAutorization> Autorizations { get; set; }
    }
}