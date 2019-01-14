using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AZ_Paas_Demo.Data.Services
{
    public class StoreService : IStoreService
    {
        private IContextFactory _context;
        public StoreService(IContextFactory context)
        {
            _context = context;
        }

        /*
        public Stores GetDefaultStore()
        {
            return _context.Stores.FirstOrDefault();
        }
        */

        public Stores GetStoreById(int storeId)
        {
            return _context.GetRoutedContext(storeId).Stores.Single(s => s.Id == storeId);
        }
    }
}
