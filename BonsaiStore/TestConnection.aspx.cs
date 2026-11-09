
using System;
using BonsaiStore.DAL;

namespace BonsaiStore
{
    public partial class TestConnection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    Response.Write("Kết nối database thành công!");
                }
            }
            catch (Exception ex)
            {
                Response.Write("Kết nối thất bại: " + ex.Message);
            }
        }
    }
}
