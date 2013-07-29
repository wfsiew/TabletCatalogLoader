using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletCatalogLoader.Model
{
    public class Product
    {
        public int id { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string image { get; set; }
        public string uom { get; set; }
    }
}
