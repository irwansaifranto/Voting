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
using Service.Models.Models.View;

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
                                    where category.IsDelete == false
                                    select new
                                    {
                                        category.VotingCategoryId,
                                        category.VotingCategoryName,
                                        category.CreatedBy,
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

        public async Task<BaseResponse> InsertCategory(Models.Models.View.ModelCategoryView model)
        {
            BaseResponse response = new BaseResponse();

            using (var dbcxtransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    MasterVotingCategories category = new MasterVotingCategories
                    {
                        VotingCategoryName = model.VotingCategoryName,
                        CreatedDate = DateTime.Now,
                        CreatedBy = "Admin",
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = "Admin",
                        IsDelete = false
                    };

                    await _context.MasterVotingCategories.AddAsync(category);
                    await _context.SaveChangesAsync();

                    dbcxtransaction.Commit();

                    response = ResponseConstant.SAVE_SUCCESS;
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Status = HttpStatusCode.InternalServerError.ToString();

                    dbcxtransaction.Rollback();
                }
            }

            return response;
        }

        public async Task<BaseResponse> UpdateCategory(ModelCategoryView model)
        {
            BaseResponse response = new BaseResponse();

            using (var dbcxtransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var category = await _context.MasterVotingCategories.FindAsync(model.VotingCategoryId);

                    category.VotingCategoryName = model.VotingCategoryName;
                    category.ModifiedDate = DateTime.Now;
                    category.ModifiedBy = "Admin";

                    _context.Entry(category).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    dbcxtransaction.Commit();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Status = HttpStatusCode.OK.ToString();
                    response.Message = "Update data success.";
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Status = HttpStatusCode.InternalServerError.ToString();

                    dbcxtransaction.Rollback();
                }
            }

            return response;
        }

        public async Task<BaseResponse> DeleteCategory(int id)
        {
            BaseResponse response = new BaseResponse();

            using (var dbcxtransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var category = await _context.MasterVotingCategories.FindAsync(id);

                    category.ModifiedDate = DateTime.Now;
                    category.ModifiedBy = "Admin";
                    category.IsDelete = true;

                    _context.Entry(category).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    dbcxtransaction.Commit();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Status = HttpStatusCode.OK.ToString();
                    response.Message = "Delete data success.";
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Status = HttpStatusCode.InternalServerError.ToString();

                    dbcxtransaction.Rollback();
                }
            }

            return response;
        }
    }
}
