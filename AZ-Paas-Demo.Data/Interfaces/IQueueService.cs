using System;
using System.Collections.Generic;
using System.Text;

namespace AZ_Paas_Demo.Data.Interfaces
{
    public interface IQueueService
    {
        void QueueNewStoreCreation(string userAndStoreData);
    }
}
