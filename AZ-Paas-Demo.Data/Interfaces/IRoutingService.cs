using System;
using System.Collections.Generic;
using System.Text;

namespace AZ_Paas_Demo.Data.Interfaces
{
    public interface IRoutingService
    {
        azpaasdemodbContext GetRoutedContext(int storeId);
        void SyncStoresToDatabases();
    }
}
