using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models.Models;
using Service.Models.Models.View;
using Service.Repository.Abstract;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("GetCategories")]
        public async Task<BaseResponse> GetCategories()
        {
            return await _categoryRepository.GetCategories();
        }

        [HttpPost("InsertCategory")]
        public async Task<BaseResponse> InsertCategory(ModelCategoryView model)
        {
            return await _categoryRepository.InsertCategory(model);
        }

        [HttpPost("UpdateCategory")]
        public async Task<BaseResponse> UpdateCategory(ModelCategoryView model)
        {
            return await _categoryRepository.UpdateCategory(model);
        }

        [HttpPost("DeleteCategory")]
        public async Task<BaseResponse> DeleteCategory([FromQuery] int id)
        {
            return await _categoryRepository.DeleteCategory(id);
        }
    }
}