using Service.Models.Entities;
using Service.Models.Models;
using Service.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Service.Repository.Concrete
{
    public class CategoryRepository : ICategoryRepository
    {
        readonly VotingContext _context;

        public CategoryRepository(VotingContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse> GetCategories()
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var result = await (from category in _context.MasterVotingCategories
                                    select new
                                    {
                                        category.VotingCategoryId,
                                        category.VotingCategoryName
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
