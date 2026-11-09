
<%@ Page Title="Đăng nhập"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="BonsaiStore.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="register-section container">

        <div class="register-box">

            <div class="register-heading">
                <span>BONSAISTORE</span>
                <h1>Đăng nhập</h1>
                <p>Chào mừng bạn quay trở lại với BonsaiStore.</p>
            </div>

            <div class="register-form">

                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server"
                    TextMode="Email"
                    placeholder="Nhập địa chỉ email"></asp:TextBox>

                <label>Mật khẩu</label>
                <asp:TextBox ID="txtPassword" runat="server"
                    TextMode="Password"
                    placeholder="Nhập mật khẩu"></asp:TextBox>

                <asp:Label ID="lblMessage" runat="server"></asp:Label>

                <asp:Button ID="btnLogin" runat="server"
                    Text="Đăng nhập"
                    CssClass="btn-register"
                    OnClick="btnLogin_Click" />

                <p class="register-login">
                    Chưa có tài khoản?
                    <a href="Register.aspx">Đăng ký ngay</a>
                </p>

            </div>

        </div>

    </section>

</asp:Content>
