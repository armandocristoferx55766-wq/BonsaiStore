
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Net.Mail;
using BonsaiStore.DAL;

namespace BonsaiStore
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                lblMessage.Text = "Vui lòng nhập đầy đủ thông tin.";
                return;
            }

            if (password.Length < 8)
            {
                lblMessage.Text = "Mật khẩu phải có ít nhất 8 ký tự.";
                return;
            }

            if (password != confirmPassword)
            {
                lblMessage.Text = "Mật khẩu xác nhận không khớp.";
                return;
            }

            try
            {
                MailAddress mail = new MailAddress(email);
            }
            catch
            {
                lblMessage.Text = "Địa chỉ email không hợp lệ.";
                return;
            }

            string passwordHash = HashPassword(password);

            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Email", email);

                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            lblMessage.Text = "Email này đã được đăng ký.";
                            return;
                        }
                    }

                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        string insertUser = @"
                            INSERT INTO Users
                            (FullName, Email, PasswordHash, Phone, Role, CreatedAt)
                            VALUES
                            (@FullName, @Email, @PasswordHash, @Phone, 'Customer', GETDATE());

                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int userID;

                        using (SqlCommand command = new SqlCommand(insertUser, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@FullName", fullName);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                            command.Parameters.AddWithValue("@Phone", phone);

                            userID = (int)command.ExecuteScalar();
                        }

                        string insertCart = @"
                            INSERT INTO Carts (UserID, CreatedAt)
                            VALUES (@UserID, GETDATE())";

                        using (SqlCommand command = new SqlCommand(insertCart, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserID", userID);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        lblMessage.Text = "Đăng ký tài khoản thành công!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;

                        txtFullName.Text = "";
                        txtEmail.Text = "";
                        txtPhone.Text = "";
                        txtPassword.Text = "";
                        txtConfirmPassword.Text = "";
                    }
                    catch
                    {
                        transaction.Rollback();
                        lblMessage.Text = "Đăng ký thất bại. Vui lòng thử lại.";
                    }
                }
            }
            catch
            {
                lblMessage.Text = "Không thể kết nối hoặc lưu dữ liệu. Vui lòng thử lại.";
            }
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            const int iterations = 100000;

            byte[] hash;

            using (Rfc2898DeriveBytes pbkdf2 =
                new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(32);
            }

            return iterations + "." +
                Convert.ToBase64String(salt) + "." +
                Convert.ToBase64String(hash);
        }
    }
}
