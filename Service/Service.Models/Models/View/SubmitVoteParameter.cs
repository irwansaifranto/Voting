using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models.Models.View
{
    public class SubmitVoteParameter
    {
        public int UserId { get; set; }
        public int VotingProcessId { get; set; }
        public double? VotingValue { get; set; }
        public DateTime VotingDate { get; set; }
    }
}
