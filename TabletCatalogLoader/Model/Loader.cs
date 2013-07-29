using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using CSharpJExcel.Jxl;
using CSharpJExcel.Jxl.Format;

using TabletCatalogLoader.Model;

namespace TabletCatalogLoader.Model
{
    public class Loader
    {
        public List<Category> CategoryList { get; set; }
        public Dictionary<int, List<Product>> ProductData { get; set; }

        public Loader(string file)
        {
            CategoryList = new List<Category>();
            ProductData = new Dictionary<int, List<Product>>();
            Load(file);
        }

        private void Load(string file)
        {
            Workbook wb = null;
            Sheet sh = null;

            try
            {
                FileInfo f = new FileInfo(file);
                wb = Workbook.getWorkbook(f);
                sh = wb.getSheet(0);

                int catid = 1;
                List<Product> lp = null;
                Product product = null;
                List<UOM> lu = null;

                int rows = 340;
                int cols = 7;

                for (int j = 3; j < rows; j++)
                {
                    Cell cell = sh.getCell(2, j);
                    string catname = cell.getContents();

                    if (!string.IsNullOrEmpty(catname))
                    {
                        CategoryList.Add(new Category { id = catid, name = catname });
                        ProductData.Add(catid, new List<Product>());
                        ++catid;
                    }

                    lp = ProductData[catid - 1];

                    UOM uom = new UOM();
                    bool add = false;

                    for (int i = 3; i < cols; i++)
                    {
                        cell = sh.getCell(i, j);

                        switch (i)
                        {
                            case 3:
                                string sku = cell.getContents();

                                if (!string.IsNullOrEmpty(sku))
                                {
                                    add = false;

                                    if (product == null)
                                    {
                                        product = new Product();
                                        product.sku = sku;
                                        lu = new List<UOM>();
                                    }
                                }

                                else
                                {
                                    add = true;
                                    i = cols;
                                }
                                break;

                            case 4:
                                string name = cell.getContents();
                                int k = name.IndexOf("(");
                                string _name = name.Substring(0, k);
                                if (string.IsNullOrEmpty(product.name))
                                    product.name = _name.Trim();

                                uom.label = name.Substring(k);
                                break;

                            case 5:
                                uom.code = cell.getContents();
                                break;

                            case 6:
                                double x = Convert.ToDouble(cell.getContents());
                                uom.price = x;
                                lu.Add(uom);
                                break;
                        }
                    }

                    if (add)
                    {
                        double v = 0;
                        for (int n = 0; n < lu.Count; n++)
                        {
                            if (n == 0)
                            {
                                v = lu[n].price;
                                lu[n].unit = 1;
                                product.price = v;
                            }

                            else
                            {
                                lu[n].unit = (int)(lu[n].price / v);
                            }
                        }

                        product.uom = JsonConvert.SerializeObject(lu);

                        lp.Add(product);
                        ProductData[catid - 1] = lp;

                        product = null;
                        lu = null;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (wb != null)
                    wb.close();
            }
        }
    }
}
