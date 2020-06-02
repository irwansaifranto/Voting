using System;
using System.Collections.Generic;

namespace Service.Models.Entities
{
    public partial class MasterRole
    {
        public MasterRole()
        {
            MasterUser = new HashSet<MasterUser>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDelete { get; set; }

        public virtual ICollection<MasterUser> MasterUser { get; set; }
    }
}
