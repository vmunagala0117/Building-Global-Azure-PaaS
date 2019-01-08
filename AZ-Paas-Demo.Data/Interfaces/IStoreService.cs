using AZ_Paas_Demo.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AZ_Paas_Demo.Data.Interfaces
{
    public interface IStoreService
    {
        Stores GetStoreById(int storeId);
        Stores GetDefaultStore();
    }
}
