using System;
using System.Collections.Generic;
using eTutor.Core.Enums;

namespace eTutor.Core.Models
{
    public sealed class Student : EntityBase, IEntityBase
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public string  Address { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public float Longitude { get; set; }
        
        public float Latitude { get; set; }
        
        public Gender Gender { get; set; }
        
        public ISet<ParentStudent> Parents { get; set; }
        
        public ISet<Rating> Ratings { get; set; }
        
        public ISet<Invoice> Invoices { get; set; }
        
        public ISet<Payment> Payments { get; set; }
    }
}