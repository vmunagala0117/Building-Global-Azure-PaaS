using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AZ_Paas_Demo.Data.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderLines = new HashSet<OrderLines>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTimeOffset? Date { get; set; }
        public double? Price { get; set; }
        public string Status { get; set; }
        public int? StoreId { get; set; }

        public Stores Store { get; set; }
        public ICollection<OrderLines> OrderLines { get; set; }
    }
}
