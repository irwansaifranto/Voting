using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingUI.Models.Voting
{
    public class SubmitVoteParameter
    {
        public int UserId { get; set; }
        public int VotingProcessId { get; set; }
        public double? VotingValue { get; set; }
    }
}
