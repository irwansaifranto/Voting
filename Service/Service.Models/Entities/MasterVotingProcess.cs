using System;
using System.Collections.Generic;

namespace Service.Models.Entities
{
    public partial class MasterVotingProcess
    {
        public MasterVotingProcess()
        {
            MappingVotingUsers = new HashSet<MappingVotingUsers>();
        }

        public int VotingProcessId { get; set; }
        public string VotingProcessName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public double? SupportersCount { get; set; }
        public DateTime DueDate { get; set; }
        public int VotingCategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }

        public virtual MasterVotingCategories VotingCategory { get; set; }
        public virtual ICollection<MappingVotingUsers> MappingVotingUsers { get; set; }
    }
}
