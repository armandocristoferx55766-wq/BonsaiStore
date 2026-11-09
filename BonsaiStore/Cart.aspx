<%@ Page Title="Giỏ hàng"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Cart.aspx.cs"
    Inherits="BonsaiStore.Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="container cart-section">

    <h1>Giỏ hàng của bạn</h1>

    <div class="cart-content">

        <div class="cart-items">

            <asp:Repeater ID="rptCart" runat="server">

                <HeaderTemplate>
                    <table class="cart-table">
                        <thead>
                            <tr>
                                <th>Sản phẩm</th>
                                <th>Đơn giá</th>
                                <th>Số lượng</th>
                                <th>Thành tiền</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>

                <ItemTemplate>
                    <tr>
                        <td>
                            <div class="cart-product">
                                <img src='<%# Eval("ImageURL") %>' alt="Sản phẩm" />
                                <span><%# Eval("ProductName") %></span>
                            </div>
                        </td>

                        <td>
                            <%# String.Format("{0:N0} đ", Eval("Price")) %>
                        </td>

                        
                    <td>
                        <div class="cart-quantity">

                            <asp:Button ID="btnDecrease" runat="server"
                                Text="-"
                                CommandArgument='<%# Eval("CartDetailID") %>'
                                OnClick="btnDecrease_Click" />

                            <span><%# Eval("Quantity") %></span>

                            <asp:Button ID="btnIncrease" runat="server"
                                Text="+"
                                CommandArgument='<%# Eval("CartDetailID") %>'
                                OnClick="btnIncrease_Click" />

                        </div>
                   </td>


                        <td>
                            <%# String.Format("{0:N0} đ", Eval("Total")) %>
                        </td>

                        <td>
                            <asp:Button ID="btnRemove" runat="server"
                                Text="Xóa"
                                CommandName="Remove"
                                CommandArgument='<%# Eval("CartDetailID") %>'
                                OnClick="btnRemove_Click" />
                        </td>
                        
                    </tr>
                </ItemTemplate>

                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>

            </asp:Repeater>

        </div>

        <div class="cart-summary">
            <h2>Tổng đơn hàng</h2>

            <p>
                Tổng tiền:
                <asp:Label ID="lblTotal" runat="server" Text="0 đ"></asp:Label>
            </p>

           <asp:Button ID="btnCheckout" runat="server"
                Text="Tiến hành đặt hàng"
                OnClick="btnCheckout_Click" />
        </div>

    </div>

</section>

</asp:Content>