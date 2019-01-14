using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace AZ_Paas_Demo.Controllers
{
    [Authorize]
    public class JuicesController : Controller
    {
        private int _storeId;
        private IJuiceService _juiceService;
        private IDistributedCache _cache;
        //private IStoreService _storeService;
        public JuicesController(/*IStoreService storeService, */IJuiceService juiceService, IDistributedCache cache)
        {
            _juiceService = juiceService;
            _cache = cache;
            //_storeService = storeService;
            //_store = _storeService.GetDefaultStore();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _storeId = int.Parse(_cache.GetString(User.Identity.Name));
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            return View(_juiceService.GetAllJuices(_storeId));
        }
    }
}