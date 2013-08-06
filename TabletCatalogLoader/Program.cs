using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TabletCatalogLoader.Model;

namespace TabletCatalogLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"..\..\File\ProdCat.Samples.xls";

            try
            {
                Loader o = new Loader(file);
                DbLoader dc = new DbLoader("category");
                dc.LoadCategory(o.CategoryList, o.ProductData);

                foreach (KeyValuePair<int, List<Product>> kv in o.ProductData)
                {
                    DbLoader dp = new DbLoader(string.Format("product.{0}", kv.Key));
                    dp.LoadProduct(kv.Value);
                }

                //foreach (KeyValuePair<int, List<Product>> kv in o.ProductData)
                //{
                //    Console.WriteLine(kv.Key);
                //    Console.WriteLine("===");
                //    foreach (Product p in kv.Value)
                //    {
                //        Console.WriteLine(p.name + ", " + p.uom);
                //    }
                //    Console.WriteLine();
                //}
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
