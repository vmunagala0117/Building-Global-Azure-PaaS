using System;
using System.Collections.Generic;
using System.Text;

namespace AZ_Paas_Demo.Data.Interfaces
{
    public interface IContextFactory
    {
        azpaasdemodbContext GetRoutedContext(int storeId);
    }
}
