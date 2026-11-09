<%@ Page Title="Chi tiết sản phẩm"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ProductDetail.aspx.cs"
    Inherits="BonsaiStore.ProductDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Content/Site.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="container product-detail">

    <div class="detail-image">
        <asp:Image ID="imgProduct" runat="server" />
    </div>

    <div class="detail-info">

        <asp:Label ID="lblCategory" runat="server"
            CssClass="product-category"></asp:Label>

        <h1>
            <asp:Label ID="lblProductName" runat="server"></asp:Label>
        </h1>

        <p class="product-price">
            <asp:Label ID="lblPrice" runat="server"></asp:Label>
        </p>

        <p>
            <asp:Label ID="lblDescription" runat="server"></asp:Label>
        </p>

        <p class="product-stock">
            <asp:Label ID="lblStock" runat="server"></asp:Label>
        </p>

       <asp:Button ID="btnAddToCart" runat="server"
            Text="Thêm vào giỏ hàng"
            CssClass="btn-add-cart"
            OnClick="btnAddToCart_Click" />

        <asp:Label ID="lblCartMessage" runat="server"></asp:Label>

    </div>

</section>

</asp:Content>