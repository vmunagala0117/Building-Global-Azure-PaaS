using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace AZ_Paas_Demo.Data.Services
{
    public class JuiceService : IJuiceService
    {
        private IContextFactory _context;
        private IDistributedCache _cache;
        public JuiceService(IContextFactory context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public List<Juices> GetAllJuices(int storeId)
        {
            List<Juices> juices;
            //get the juices details from the cache
            var cachedJuices = _cache.GetString("juices");
            if (!String.IsNullOrEmpty(cachedJuices))
            {
                juices = JsonConvert.DeserializeObject<List<Juices>>(cachedJuices);
            }
            else
            {
                // no juices, get it from the database
                juices = _context.GetRoutedContext(2).Juices.ToList();
                _cache.SetString("juices", JsonConvert.SerializeObject(juices));
            }
            return juices;
        }
    }
}
