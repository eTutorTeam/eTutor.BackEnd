using eTutor.Core.Enums;

namespace eTutor.Core.Models
{
    public class ParentStudent : EntityBase, IEntityBase
    {
        public int StudentId { get; set; }

        public User Student { get; set; }

        public int ParentId { get; set; }

        public User Parent { get; set; }
        
        public ParentRelationship Relationship { get; set; }
    }
}