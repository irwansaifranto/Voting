using System;
using System.Collections.Generic;

namespace Service.Models.Entities
{
    public partial class MasterVotingCategories
    {
        public MasterVotingCategories()
        {
            MasterVotingProcess = new HashSet<MasterVotingProcess>();
        }

        public int VotingCategoryId { get; set; }
        public string VotingCategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<MasterVotingProcess> MasterVotingProcess { get; set; }
    }
}
