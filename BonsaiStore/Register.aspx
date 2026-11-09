<%@ Page Title="Đăng ký tài khoản"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Register.aspx.cs"
    Inherits="BonsaiStore.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="register-section container">

        <div class="register-box">

            <div class="register-heading">
                <span>BONSAISTORE</span>
                <h1>Tạo tài khoản</h1>
                <p>Đăng ký để trải nghiệm mua sắm tại BonsaiStore.</p>
            </div>

            <div class="register-form">

                <label>Họ và tên</label>
                <asp:TextBox ID="txtFullName" runat="server"
                    placeholder="Nhập họ và tên"></asp:TextBox>

                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server"
                    TextMode="Email"
                    placeholder="Nhập địa chỉ email"></asp:TextBox>

                <label>Số điện thoại</label>
                <asp:TextBox ID="txtPhone" runat="server"
                    placeholder="Nhập số điện thoại"></asp:TextBox>

                <label>Mật khẩu</label>
                <asp:TextBox ID="txtPassword" runat="server"
                    TextMode="Password"
                    placeholder="Nhập mật khẩu"></asp:TextBox>

                <label>Xác nhận mật khẩu</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server"
                    TextMode="Password"
                    placeholder="Nhập lại mật khẩu"></asp:TextBox>

                <asp:Label ID="lblMessage" runat="server"></asp:Label>

                <asp:Button ID="btnRegister" runat="server"
                    Text="Đăng ký"
                    CssClass="btn-register"
                    OnClick="btnRegister_Click" />

                <p class="register-login">
                    Đã có tài khoản?
                    <a href="Login.aspx">Đăng nhập</a>
                </p>

            </div>

        </div>

    </section>

</asp:Content>