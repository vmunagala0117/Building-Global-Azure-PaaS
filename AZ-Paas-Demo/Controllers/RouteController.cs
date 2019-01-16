using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZ_Paas_Demo.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZ_Paas_Demo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        IRoutingService _routingService;
        public RouteController(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        [HttpGet]
        public void Get()
        {
            _routingService.SyncStoresToDatabases();
        }
    }
}