
<%@ Page Title="Thanh toán"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Checkout.aspx.cs"
    Inherits="BonsaiStore.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="container cart-section">

        <h1>Xác nhận đặt hàng</h1>

        <div class="cart-content">

            <div class="cart-items">

                <div class="register-box">

                    <div class="register-heading">
                        <h2>Thông tin giao hàng</h2>
                        <p>Vui lòng nhập thông tin nhận hàng của bạn.</p>
                    </div>

                    <div class="register-form">

                        <label>Họ và tên người nhận</label>
                        <asp:TextBox ID="txtReceiverName" runat="server"
                            placeholder="Nhập họ và tên"></asp:TextBox>

                        <label>Số điện thoại</label>
                        <asp:TextBox ID="txtPhone" runat="server"
                            placeholder="Nhập số điện thoại"></asp:TextBox>

                        <label>Địa chỉ chi tiết</label>
                        <asp:TextBox ID="txtAddress" runat="server"
                            placeholder="Số nhà, tên đường"></asp:TextBox>

                        <label>Phường / Xã</label>
                        <asp:TextBox ID="txtWard" runat="server"
                            placeholder="Nhập phường / xã"></asp:TextBox>

                        <label>Quận / Huyện</label>
                        <asp:TextBox ID="txtDistrict" runat="server"
                            placeholder="Nhập quận / huyện"></asp:TextBox>

                        <label>Tỉnh / Thành phố</label>
                        <asp:TextBox ID="txtCity" runat="server"
                            placeholder="Nhập tỉnh / thành phố"></asp:TextBox>

                    </div>

                </div>

            </div>

            <div class="cart-summary">

                <h2>Đơn hàng của bạn</h2>

                <p>
                    Số lượng sản phẩm:
                    <asp:Label ID="lblQuantity" runat="server" Text="0"></asp:Label>
                </p>

                <p>
                    Tổng tiền:
                    <asp:Label ID="lblTotal" runat="server" Text="0 đ"></asp:Label>
                </p>

                <asp:Label ID="lblMessage" runat="server"></asp:Label>

                <asp:Button ID="btnPlaceOrder" runat="server"
                    Text="Xác nhận đặt hàng"
                    CssClass="btn-register"
                    OnClick="btnPlaceOrder_Click" />

            </div>

        </div>

    </section>

</asp:Content>
