using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AZ_Paas_Demo.Data.Interfaces
{
    public interface IAccountService
    {
        Task<int> GetStoreIdFromUser(string userId);
    }
}
