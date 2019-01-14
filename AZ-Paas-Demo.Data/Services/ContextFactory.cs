using AZ_Paas_Demo.Data.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;

namespace AZ_Paas_Demo.Data.Services
{
    public class ContextFactory : IContextFactory
    {
        private IRoutingService _routingService;
        private IDistributedCache _cache;
        public ContextFactory(IRoutingService routingService, IDistributedCache cache)
        {
            _routingService = routingService;
            _cache = cache;
        }
        public azpaasdemodbContext GetRoutedContext(int storeId)
        {
             return _routingService.GetRoutedContext(storeId);
        }
    }
}
