using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace TabletCatalogLoader.Model
{
    public class DbLoader
    {
        private SQLiteConnection conn;

        public DbLoader(string db)
        {
            string file = string.Format("..\\..\\Db\\{0}.db", db);
            if (File.Exists(file))
                File.Delete(file);

            conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;New=False;Compress=True;", file));
        }

        public void LoadProduct(List<Product> lp)
        {
            string q = "insert into product (id, sku, name, price, image, uom) values (@id, @sku, @name, @price, @image, @uom)";
            string s = "CREATE TABLE \"product\" (\"id\" INTEGER PRIMARY KEY  NOT NULL ,\"sku\" VARCHAR,\"name\" VARCHAR,\"price\" DOUBLE DEFAULT (0) ,\"image\" VARCHAR,\"uom\" VARCHAR DEFAULT (null) )";

            try
            {
                conn.Open();
                SQLiteCommand cmdCreate = conn.CreateCommand();
                SQLiteCommand cmd = conn.CreateCommand();
                cmdCreate.CommandText = s;
                cmd.CommandText = q;

                cmdCreate.ExecuteNonQuery();

                for (int i = 0; i < lp.Count; i++)
                {
                    Product o = lp[i];

                    cmd.Parameters.AddWithValue("@id", i + 1);
                    cmd.Parameters.AddWithValue("@sku", o.sku);
                    cmd.Parameters.AddWithValue("@name", o.name);
                    cmd.Parameters.AddWithValue("@price", o.price);
                    cmd.Parameters.AddWithValue("@image", o.image);
                    cmd.Parameters.AddWithValue("@uom", o.uom);

                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                conn.Close();
            }
        }

        public void LoadCategory(List<Category> lc)
        {
            string q = "insert into category (id, name) values (@id, @name)";
            string s = "CREATE TABLE \"category\" (\"id\" INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , \"name\" VARCHAR UNIQUE )";

            try
            {
                conn.Open();
                SQLiteCommand cmdCreate = conn.CreateCommand();
                SQLiteCommand cmd = conn.CreateCommand();
                cmdCreate.CommandText = s;
                cmd.CommandText = q;

                cmdCreate.ExecuteNonQuery();

                for (int i = 0; i < lc.Count; i++)
                {
                    Category o = lc[i];

                    cmd.Parameters.AddWithValue("@id", o.id);
                    cmd.Parameters.AddWithValue("@name", o.name);

                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                conn.Close();
            }
        }
    }
}
