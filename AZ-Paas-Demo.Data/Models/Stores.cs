using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AZ_Paas_Demo.Data.Models
{
    public partial class Stores
    {
        public Stores()
        {
            Orders = new HashSet<Orders>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
