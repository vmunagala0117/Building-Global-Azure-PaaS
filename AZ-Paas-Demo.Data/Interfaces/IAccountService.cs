using AZ_Paas_Demo.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AZ_Paas_Demo.Data.Interfaces
{
    public interface IAccountService
    {
        Task<int> GetStoreIdFromUser(string userId);
        void RegisterNewStoreAndUser(Register model);
        Task<string> CreateNewUser(Register userInfo, string storeId);
    }
}
