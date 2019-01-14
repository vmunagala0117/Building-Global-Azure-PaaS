using System;
using System.Collections.Generic;

namespace AZ_Paas_Demo.Data.Models
{
    public partial class DatabaseServers
    {
        public DatabaseServers()
        {
            Stores = new HashSet<Stores>();
        }

        public int Id { get; set; }
        public string DatabaseServer { get; set; }
        public string DatabaseName { get; set; }
        public string Region { get; set; }

        public ICollection<Stores> Stores { get; set; }
    }
}
