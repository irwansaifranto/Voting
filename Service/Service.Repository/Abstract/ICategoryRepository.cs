using Service.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository.Abstract
{
    public interface ICategoryRepository
    {
        Task<BaseResponse> GetCategories();
        Task<BaseResponse> InsertCategory(Models.Models.View.ModelCategoryView model);
        Task<BaseResponse> UpdateCategory(Models.Models.View.ModelCategoryView model);
        Task<BaseResponse> DeleteCategory(int id);
    }
}
