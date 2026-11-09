using System;
using System.Data;
using System.Data.SqlClient;
using BonsaiStore.DAL;

namespace BonsaiStore
{
    public partial class ProductDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProduct();
            }
        }

        private void LoadProduct()
        {
            int productID;

            if (!int.TryParse(Request.QueryString["id"], out productID))
            {
                return;
            }

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = @"
            SELECT 
                Products.ProductName,
                Products.Description,
                Products.Price,
                Products.Stock,
                Products.ImageURL,
                Categories.CategoryName
            FROM Products
            INNER JOIN Categories
                ON Products.CategoryID = Categories.CategoryID
            WHERE Products.ProductID = @ProductID
                AND Products.IsActive = 1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblProductName.Text = reader["ProductName"].ToString();
                            lblCategory.Text = reader["CategoryName"].ToString();

                            lblPrice.Text = string.Format(
                                "{0:N0} đ", reader["Price"]);

                            lblDescription.Text = reader["Description"].ToString();

                            lblStock.Text = "Còn lại: " +
                                reader["Stock"].ToString() + " sản phẩm";

                            imgProduct.ImageUrl = reader["ImageURL"].ToString();
                            imgProduct.AlternateText = reader["ProductName"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                lblCartMessage.Text = "Vui lòng đăng nhập trước khi mua hàng.";
                return;
            }

            int productID;
            int userID = Convert.ToInt32(Session["UserID"]);

            if (!int.TryParse(Request.QueryString["id"], out productID))
            {
                lblCartMessage.Text = "Sản phẩm không hợp lệ.";
                return;
            }

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string stockQuery = @"
                SELECT Stock
                FROM Products
                WHERE ProductID = @ProductID
                AND IsActive = 1";

                    int stock;

                    using (SqlCommand command = new SqlCommand(stockQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ProductID", productID);

                        object result = command.ExecuteScalar();

                        if (result == null)
                        {
                            transaction.Rollback();
                            lblCartMessage.Text = "Sản phẩm không tồn tại.";
                            return;
                        }

                        stock = Convert.ToInt32(result);
                    }

                    if (stock <= 0)
                    {
                        transaction.Rollback();
                        lblCartMessage.Text = "Sản phẩm đã hết hàng.";
                        return;
                    }

                    string cartQuery = @"
                SELECT CartID
                FROM Carts
                WHERE UserID = @UserID";

                    int cartID;

                    using (SqlCommand command = new SqlCommand(cartQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);

                        object result = command.ExecuteScalar();

                        if (result == null)
                        {
                            transaction.Rollback();
                            lblCartMessage.Text = "Không tìm thấy giỏ hàng.";
                            return;
                        }

                        cartID = Convert.ToInt32(result);
                    }

                    string quantityQuery = @"
                SELECT Quantity
                FROM CartDetails
                WHERE CartID = @CartID
                AND ProductID = @ProductID";

                    int quantity = 0;

                    using (SqlCommand command = new SqlCommand(quantityQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@CartID", cartID);
                        command.Parameters.AddWithValue("@ProductID", productID);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            quantity = Convert.ToInt32(result);
                        }
                    }

                    if (quantity >= stock)
                    {
                        transaction.Rollback();
                        lblCartMessage.Text = "Số lượng sản phẩm đã đạt giới hạn tồn kho.";
                        return;
                    }

                    if (quantity > 0)
                    {
                        string updateQuery = @"
                    UPDATE CartDetails
                    SET Quantity = Quantity + 1
                    WHERE CartID = @CartID
                    AND ProductID = @ProductID";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@CartID", cartID);
                            command.Parameters.AddWithValue("@ProductID", productID);

                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertQuery = @"
                    INSERT INTO CartDetails (CartID, ProductID, Quantity)
                    VALUES (@CartID, @ProductID, 1)";

                        using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@CartID", cartID);
                            command.Parameters.AddWithValue("@ProductID", productID);

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();

                    lblCartMessage.Text = "Đã thêm sản phẩm vào giỏ hàng!";
                    lblCartMessage.ForeColor = System.Drawing.Color.Green;
                }
                catch
                {
                    transaction.Rollback();
                    lblCartMessage.Text = "Không thể thêm sản phẩm. Vui lòng thử lại.";
                }
            }
        }

    }

}