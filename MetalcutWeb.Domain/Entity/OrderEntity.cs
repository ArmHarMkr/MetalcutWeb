using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.Domain.Entity
{
    public class OrderEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CustomerName { get; set; }
        public string OrderDescription { get; set; }
        public string CustomerPhone { get; set; }
        public int OrderPrice { get; set; }
        public DateTime OrderCreatedDate { get; set; }
        public DateTime OrderFinishDate { get; set; }
    }
}
