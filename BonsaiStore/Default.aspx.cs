
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BonsaiStore.DAL;

namespace BonsaiStore
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        Products.ProductID,
                        Products.ProductName,
                        Products.Description,
                        Products.Price,
                        Products.Stock,
                        Products.ImageURL,
                        Categories.CategoryName
                    FROM Products
                    INNER JOIN Categories
                    ON Products.CategoryID = Categories.CategoryID
                    WHERE Products.IsActive = 1
                    ORDER BY Products.CreatedAt DESC";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    rptProducts.DataSource = table;
                    rptProducts.DataBind();
                }
            }
        }
    }
}
