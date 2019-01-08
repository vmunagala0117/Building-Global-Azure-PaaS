using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AZ_Paas_Demo.Data.Models
{
    public partial class Juices
    {
        public Juices()
        {
            OrderLines = new HashSet<OrderLines>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double? Price { get; set; }

        public ICollection<OrderLines> OrderLines { get; set; }
    }
}
