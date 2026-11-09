
using BonsaiStore.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace BonsaiStore
{
    public partial class Cart : System.Web.UI.Page
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

                LoadCart();
            }
        }

        private void LoadCart()
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        CartDetails.CartDetailID,
                        Products.ProductName,
                        Products.ImageURL,
                        Products.Price,
                        CartDetails.Quantity,
                        Products.Price * CartDetails.Quantity AS Total
                    FROM Carts
                    INNER JOIN CartDetails
                        ON Carts.CartID = CartDetails.CartID
                    INNER JOIN Products
                        ON CartDetails.ProductID = Products.ProductID
                    WHERE Carts.UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();

                        adapter.Fill(table);

                        rptCart.DataSource = table;
                        rptCart.DataBind();

                        decimal total = 0;

                        foreach (DataRow row in table.Rows)
                        {
                            total += Convert.ToDecimal(row["Total"]);
                        }

                        lblTotal.Text = string.Format("{0:N0} đ", total);
                    }
                }
            }
        }


        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int cartDetailID = Convert.ToInt32(btn.CommandArgument);
            int userID = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = @"
            DELETE CartDetails
            FROM CartDetails
            INNER JOIN Carts
                ON CartDetails.CartID = Carts.CartID
            WHERE CartDetails.CartDetailID = @CartDetailID
            AND Carts.UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CartDetailID", cartDetailID);
                    command.Parameters.AddWithValue("@UserID", userID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            LoadCart();
        }


        protected void btnIncrease_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int cartDetailID = Convert.ToInt32(btn.CommandArgument);
            int userID = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = @"
            UPDATE CartDetails
            SET Quantity = Quantity + 1
            FROM CartDetails
            INNER JOIN Carts
                ON CartDetails.CartID = Carts.CartID
            INNER JOIN Products
                ON CartDetails.ProductID = Products.ProductID
            WHERE CartDetails.CartDetailID = @CartDetailID
                AND Carts.UserID = @UserID
                AND CartDetails.Quantity < Products.Stock";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CartDetailID", cartDetailID);
                    command.Parameters.AddWithValue("@UserID", userID);

                    connection.Open();

                    int result = command.ExecuteNonQuery();

                    if (result == 0)
                    {
                        lblTotal.Text = lblTotal.Text;
                    }
                }
            }

            LoadCart();
        }

        protected void btnDecrease_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int cartDetailID = Convert.ToInt32(btn.CommandArgument);
            int userID = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = @"
            UPDATE CartDetails
            SET Quantity = Quantity - 1
            FROM CartDetails
            INNER JOIN Carts
                ON CartDetails.CartID = Carts.CartID
            WHERE CartDetails.CartDetailID = @CartDetailID
                AND Carts.UserID = @UserID
                AND CartDetails.Quantity > 1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CartDetailID", cartDetailID);
                    command.Parameters.AddWithValue("@UserID", userID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            LoadCart();
        }
        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Checkout.aspx");
        }

    }
}
