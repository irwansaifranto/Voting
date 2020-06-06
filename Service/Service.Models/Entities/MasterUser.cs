using System;
using System.Collections.Generic;

namespace Service.Models.Entities
{
    public partial class MasterUser
    {
        public MasterUser()
        {
            MappingVotingUsers = new HashSet<MappingVotingUsers>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Gender { get; set; }
        public decimal Age { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDelete { get; set; }

        public virtual MasterRole Role { get; set; }
        public virtual ICollection<MappingVotingUsers> MappingVotingUsers { get; set; }
    }
}
