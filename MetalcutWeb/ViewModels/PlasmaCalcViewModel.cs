using MetalcutWeb.Domain.Entity;
using MetalcutWeb.Models;

namespace MetalcutWeb.ViewModels
{
    public class PlasmaCalcViewModel
    {
        public PlasmaCalcEntity PlasmaCalcEntity { get; set; }
        public ProductEntity ProductEntity { get; set; }
        public Delivery Delivery { get; set; }
    }
}
