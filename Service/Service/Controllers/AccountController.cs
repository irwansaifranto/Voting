using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models.Models;
using Service.Repository.Abstract;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetUserByUsernameOrEmail")]
        public async Task<BaseResponse> GetUserByUsernameOrEmail(string key)
        {
            return await _userRepository.GetUserByUsernameOrEmail(key);
        }
    }
}