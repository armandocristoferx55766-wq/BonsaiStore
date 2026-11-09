
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BonsaiStore.DAL;

namespace BonsaiStore
{
    public partial class Checkout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                LoadOrderSummary();
            }
        }

        private void LoadOrderSummary()
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        ISNULL(SUM(CartDetails.Quantity), 0) AS TotalQuantity,
                        ISNULL(SUM(Products.Price * CartDetails.Quantity), 0) AS TotalAmount
                    FROM Carts
                    INNER JOIN CartDetails
                        ON Carts.CartID = CartDetails.CartID
                    INNER JOIN Products
                        ON CartDetails.ProductID = Products.ProductID
                    WHERE Carts.UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int quantity = Convert.ToInt32(reader["TotalQuantity"]);
                            decimal total = Convert.ToDecimal(reader["TotalAmount"]);

                            if (quantity == 0)
                            {
                                Response.Redirect("Cart.aspx");
                                return;
                            }

                            lblQuantity.Text = quantity.ToString();
                            lblTotal.Text = string.Format("{0:N0} đ", total);
                        }
                    }
                }
            }
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string receiverName = txtReceiverName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string ward = txtWard.Text.Trim();
            string district = txtDistrict.Text.Trim();
            string city = txtCity.Text.Trim();

            if (receiverName == "" || phone == "" ||
                address == "" || ward == "" ||
                district == "" || city == "")
            {
                lblMessage.Text = "Vui lòng nhập đầy đủ thông tin giao hàng.";
                return;
            }

            int userID = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int cartID = 0;

                        string cartQuery = @"
                            SELECT CartID
                            FROM Carts WITH (UPDLOCK, HOLDLOCK)
                            WHERE UserID = @UserID";

                        using (SqlCommand command = new SqlCommand(
                            cartQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserID", userID);

                            object result = command.ExecuteScalar();

                            if (result == null)
                            {
                                throw new Exception("Không tìm thấy giỏ hàng.");
                            }

                            cartID = Convert.ToInt32(result);
                        }

                        string productQuery = @"
                            SELECT
                                CartDetails.CartDetailID,
                                CartDetails.ProductID,
                                Products.ProductName,
                                CartDetails.Quantity,
                                Products.Price,
                                Products.Stock,
                                Products.IsActive
                            FROM CartDetails
                            INNER JOIN Products
                                ON CartDetails.ProductID = Products.ProductID
                            WHERE CartDetails.CartID = @CartID";

                        DataTable products = new DataTable();

                        using (SqlCommand command = new SqlCommand(
                            productQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@CartID", cartID);

                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.SelectCommand.Transaction = transaction;
                                adapter.Fill(products);
                            }
                        }

                        if (products.Rows.Count == 0)
                        {
                            throw new Exception("Giỏ hàng hiện không có sản phẩm.");
                        }

                        decimal totalAmount = 0;

                        foreach (DataRow row in products.Rows)
                        {
                            int quantity = Convert.ToInt32(row["Quantity"]);
                            int stock = Convert.ToInt32(row["Stock"]);
                            bool isActive = Convert.ToBoolean(row["IsActive"]);

                            if (!isActive)
                            {
                                throw new Exception(
                                    "Sản phẩm " + row["ProductName"] +
                                    " hiện không còn được kinh doanh.");
                            }

                            if (quantity > stock)
                            {
                                throw new Exception(
                                    "Sản phẩm " + row["ProductName"] +
                                    " không đủ số lượng trong kho.");
                            }

                            decimal price = Convert.ToDecimal(row["Price"]);
                            totalAmount += price * quantity;
                        }

                        string addressQuery = @"
                            INSERT INTO Addresses
                            (
                                UserID, ReceiverName, Phone,
                                AddressDetail, Ward, District, City
                            )
                            VALUES
                            (
                                @UserID, @ReceiverName, @Phone,
                                @AddressDetail, @Ward, @District, @City
                            );

                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int addressID;

                        using (SqlCommand command = new SqlCommand(
                            addressQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserID", userID);
                            command.Parameters.AddWithValue("@ReceiverName", receiverName);
                            command.Parameters.AddWithValue("@Phone", phone);
                            command.Parameters.AddWithValue("@AddressDetail", address);
                            command.Parameters.AddWithValue("@Ward", ward);
                            command.Parameters.AddWithValue("@District", district);
                            command.Parameters.AddWithValue("@City", city);

                            addressID = Convert.ToInt32(command.ExecuteScalar());
                        }

                        string orderQuery = @"
                            INSERT INTO Orders
                            (
                                UserID, AddressID, OrderDate,
                                TotalAmount, OrderStatus
                            )
                            VALUES
                            (
                                @UserID, @AddressID, GETDATE(),
                                @TotalAmount, 'Pending'
                            );

                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int orderID;

                        using (SqlCommand command = new SqlCommand(
                            orderQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserID", userID);
                            command.Parameters.AddWithValue("@AddressID", addressID);
                            command.Parameters.AddWithValue("@TotalAmount", totalAmount);

                            orderID = Convert.ToInt32(command.ExecuteScalar());
                        }

                        foreach (DataRow row in products.Rows)
                        {
                            int productID = Convert.ToInt32(row["ProductID"]);
                            int quantity = Convert.ToInt32(row["Quantity"]);
                            decimal price = Convert.ToDecimal(row["Price"]);

                            string detailQuery = @"
                                INSERT INTO OrderDetails
                                (
                                    OrderID, ProductID, Quantity, UnitPrice
                                )
                                VALUES
                                (
                                    @OrderID, @ProductID, @Quantity, @UnitPrice
                                )";

                            using (SqlCommand command = new SqlCommand(
                                detailQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@OrderID", orderID);
                                command.Parameters.AddWithValue("@ProductID", productID);
                                command.Parameters.AddWithValue("@Quantity", quantity);
                                command.Parameters.AddWithValue("@UnitPrice", price);

                                command.ExecuteNonQuery();
                            }

                            string stockQuery = @"
                                UPDATE Products
                                SET Stock = Stock - @Quantity
                                WHERE ProductID = @ProductID
                                    AND Stock >= @Quantity
                                    AND IsActive = 1";

                            using (SqlCommand command = new SqlCommand(
                                stockQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@Quantity", quantity);
                                command.Parameters.AddWithValue("@ProductID", productID);

                                int updated = command.ExecuteNonQuery();

                                if (updated == 0)
                                {
                                    throw new Exception(
                                        "Số lượng tồn kho đã thay đổi. Vui lòng thử lại.");
                                }
                            }
                        }

                        string paymentQuery = @"
                            INSERT INTO Payments
                            (
                                OrderID, PaymentMethod,
                                PaymentStatus, PaymentDate
                            )
                            VALUES
                            (
                                @OrderID, 'COD', 'Unpaid', NULL
                            )";

                        using (SqlCommand command = new SqlCommand(
                            paymentQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderID", orderID);
                            command.ExecuteNonQuery();
                        }

                        string deleteCartQuery = @"
                            DELETE FROM CartDetails
                            WHERE CartID = @CartID";

                        using (SqlCommand command = new SqlCommand(
                            deleteCartQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@CartID", cartID);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        lblMessage.Text =
                            "Đặt hàng thành công! Mã đơn hàng của bạn là: " +
                            orderID.ToString();

                        lblQuantity.Text = "0";
                        lblTotal.Text = "0 đ";

                        btnPlaceOrder.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        lblMessage.Text = Server.HtmlEncode(ex.Message);
                    }
                }
            }
        }
    }
}
