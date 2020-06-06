using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models.Models.View
{
    public class ModelVotingView
    {
        public int VotingProcessId { get; set; }
        public string VotingProcessName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public double? SupportersCount { get; set; }
        public DateTime DueDate { get; set; }
        public string VotingCategoryName { get; set; }
        public int VotingCategoryId { get; set; }
    }
}
