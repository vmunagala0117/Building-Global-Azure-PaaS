using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AZ_Paas_Demo.Data.Services
{
    public class RoutingService : IRoutingService
    {
        private IAccountService _accountService;
        private ConnectionStrings _connectionStrings;
        private IDistributedCache _cache;
        private azpaasdemodbContext _currentContext;

        public RoutingService(IAccountService accountService, IOptions<ConnectionStrings> connectionStrings,
            IDistributedCache cache, azpaasdemodbContext context)
        {
            _accountService = accountService;
            _connectionStrings = connectionStrings.Value;
            _cache = cache;
            _currentContext = context;
        }

        public azpaasdemodbContext GetRoutedContext(int storeId)
        {
            string connectionString = GetDataStoreForStoreId(storeId);

            var optionsBuilder = new DbContextOptionsBuilder<azpaasdemodbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new azpaasdemodbContext(optionsBuilder.Options);
        }

        private string GetDataStoreForStoreId(int storeId)
        {
            string server = string.Empty;
            string database = string.Empty;
            string connectionString = string.Empty;

            if (string.IsNullOrEmpty(_cache.GetString("connectionString" + storeId.ToString())))
            {
                var store = _currentContext.Stores.Where(s => s.Id == storeId)
                    .Include(s => s.DatabaseServer)
                    .FirstOrDefault();

                if (store != null)
                {
                    connectionString = string.Format(_connectionStrings.JuiceDBConnectionTemplate, store.DatabaseServer.DatabaseServer, store.DatabaseServer.DatabaseName);

                    _cache.SetString("connectionString" + storeId.ToString(), connectionString);
                }
            }
            else
            {
                connectionString = _cache.GetString("connectionString" + storeId.ToString());
            }

            return connectionString;
        }

        public void SyncStoresToDatabases()
        {
            var storesToSync = _currentContext.Stores.ToList();

            var servers = _currentContext.DatabaseServers.ToList();

            foreach (var server in servers)
            {
                var optionsBuilder = new DbContextOptionsBuilder<azpaasdemodbContext>();
                optionsBuilder.UseSqlServer(GetDataStoreForDatabaseServer(server));
                var context = new azpaasdemodbContext(optionsBuilder.Options);
                try
                {
                    var stores = context.Stores.ToList();
                    //loop over the stores to sync
                    foreach (var store in storesToSync)
                    {
                        //if the store is not in the current context's stores list
                        if (stores.Where(s => s.Name == store.Name).FirstOrDefault() == null)
                        {
                            //build a new storeobject, so that the connection to the old DBContext is gone
                            Stores storeToAdd = new Stores
                            {
                                Name = store.Name,
                                Country = store.Country,
                                DatabaseServer = context.DatabaseServers.Where(d => d.Id == store.DatabaseServer.Id).FirstOrDefault(),
                                Orders = store.Orders
                            };
                            context.Stores.Add(storeToAdd);
                            context.SaveChanges();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    //database cannot be openened for some reason
                }
            }
        }                                        
        private string GetDataStoreForDatabaseServer(DatabaseServers server)
        {
            return string.Format(_connectionStrings.JuiceDBConnectionTemplate, server.DatabaseServer, server.DatabaseName);
        }
    }
}
