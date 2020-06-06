using System;
using System.Collections.Generic;

namespace Service.Models.Entities
{
    public partial class MappingVotingUsers
    {
        public int UserId { get; set; }
        public int VotingProcessId { get; set; }

        public virtual MasterUser User { get; set; }
        public virtual MasterVotingProcess VotingProcess { get; set; }
    }
}
