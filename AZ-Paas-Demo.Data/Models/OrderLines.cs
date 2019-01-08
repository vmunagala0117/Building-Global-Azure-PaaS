using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AZ_Paas_Demo.Data.Models
{
    public partial class OrderLines
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public int? JuiceId { get; set; }
        public int? OrderId { get; set; }

        public Juices Juice { get; set; }
        public Orders Order { get; set; }
    }
}
