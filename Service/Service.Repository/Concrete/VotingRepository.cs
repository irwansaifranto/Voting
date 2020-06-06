using Service.Models.Entities;
using Service.Models.Models;
using Service.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Service.Repository.Concrete
{
    public class VotingRepository : IVotingRepository
    {
        readonly VotingContext _context;
        public VotingRepository(VotingContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse> GetVotings()
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var result = await (from voting in _context.MasterVotingProcess
                                    select new {
                                        Name = voting.VotingProcessName,
                                        voting.Description,
                                        voting.CreatedDate,
                                        voting.SupportersCount,
                                        voting.DueDate,
                                        voting.VotingCategory.VotingCategoryName
                                    }).ToListAsync();

                response.Status = HttpStatusCode.OK.ToString();
                response.Code = (int)HttpStatusCode.OK;

                if (result != null)
                {
                    response.Message = "Retrieve data success";
                }
                else
                {
                    response.Message = "No result";
                }

                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError.ToString();
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = ex.ToString();
            }

            return response;
        }
    }
}
