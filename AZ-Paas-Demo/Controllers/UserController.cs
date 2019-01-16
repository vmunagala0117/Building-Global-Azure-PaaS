using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ_Paas_Demo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IAccountService _accountService;
        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<string> Post([FromBody]Register value, string storeId)
        {
            return await _accountService.CreateNewUser(value, storeId);
        }
    }
}