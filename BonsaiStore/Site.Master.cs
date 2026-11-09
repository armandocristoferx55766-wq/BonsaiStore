
using System;

namespace BonsaiStore
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                phGuest.Visible = false;
                phUser.Visible = true;

                lblWelcome.Text = "Xin chào, " +
                    Server.HtmlEncode(Session["FullName"].ToString());
            }
            else
            {
                phGuest.Visible = true;
                phUser.Visible = false;
            }
        }
    }
}
