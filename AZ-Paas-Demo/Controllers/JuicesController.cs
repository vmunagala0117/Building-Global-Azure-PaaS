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
    public class JuicesController : Controller
    {
        private Stores _store;
        private IJuiceService _juiceService;
        private IStoreService _storeService;
        public JuicesController(IStoreService storeService, IJuiceService juiceService)
        {
            _juiceService = juiceService;
            _storeService = storeService;
            _store = _storeService.GetDefaultStore();
        }
        public IActionResult Index()
        {
            return View(_juiceService.GetAllJuices());
        }
    }
}