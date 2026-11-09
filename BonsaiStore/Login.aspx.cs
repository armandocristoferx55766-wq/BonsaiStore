
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using BonsaiStore.DAL;

namespace BonsaiStore
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text = "Vui lòng nhập email và mật khẩu.";
                return;
            }

            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    string query = @"
                        SELECT UserID, FullName, PasswordHash, Role
                        FROM Users
                        WHERE Email = @Email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userID = Convert.ToInt32(reader["UserID"]);
                                string fullName = reader["FullName"].ToString();
                                string passwordHash = reader["PasswordHash"].ToString();
                                string role = reader["Role"].ToString();

                                if (VerifyPassword(password, passwordHash))
                                {
                                    Session["UserID"] = userID;
                                    Session["FullName"] = fullName;
                                    Session["Role"] = role;

                                    Response.Redirect("Default.aspx");
                                }
                                else
                                {
                                    lblMessage.Text = "Email hoặc mật khẩu không chính xác.";
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Email hoặc mật khẩu không chính xác.";
                            }
                        }
                    }
                }
            }
            catch
            {
                lblMessage.Text = "Đã xảy ra lỗi. Vui lòng thử lại.";
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                string[] parts = storedHash.Split('.');

                int iterations = Convert.ToInt32(parts[0]);

                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] originalHash = Convert.FromBase64String(parts[2]);

                byte[] newHash;

                using (Rfc2898DeriveBytes pbkdf2 =
                    new Rfc2898DeriveBytes(
                        password,
                        salt,
                        iterations,
                        HashAlgorithmName.SHA256))
                {
                    newHash = pbkdf2.GetBytes(originalHash.Length);
                }

                if (originalHash.Length != newHash.Length)
                {
                    return false;
                }

                int difference = 0;

                for (int i = 0; i < originalHash.Length; i++)
                {
                    difference |= originalHash[i] ^ newHash[i];
                }

                return difference == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
