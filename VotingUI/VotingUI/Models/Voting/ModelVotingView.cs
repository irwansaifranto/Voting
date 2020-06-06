using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingUI.Models.Category;

namespace VotingUI.Models.Voting
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
        public List<SelectListItem> CategoryOptions { get; set; }

        public List<SelectListItem> ConstructBookOptions(List<ModelCategoryView> categories)
        {
            var result = new List<SelectListItem>();

            result.Add(new SelectListItem { Value = "", Text = "Select", Selected = true });
            foreach (var single in categories)
            {
                result.Add(new SelectListItem { Value = single.VotingCategoryId.ToString(), Text = single.VotingCategoryName });
            }

            return result;
        }
    }
}
