
<%@ Page Title="Trang chủ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BonsaiStore._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <section class="hero">
        <div class="hero-content">
            <span class="hero-subtitle">WELCOME TO BONSAISTORE</span>

            <h1>Mang thiên nhiên<br />vào không gian sống</h1>

            <p>
                Khám phá thế giới cây bonsai, hoa và những sản phẩm
                chăm sóc cây cảnh chất lượng.
            </p>

            <a href="#categories" class="btn-primary">
                Khám phá ngay
            </a>
        </div>
    </section>

    <section class="category-section container" id="categories">

        <div class="section-heading">
            <span>KHÁM PHÁ CỬA HÀNG</span>
            <h2>Danh mục sản phẩm</h2>
            <p>Lựa chọn những sản phẩm phù hợp với khu vườn của bạn.</p>
        </div>

        <div class="category-grid">

            <a href="#" class="category-card">
                <div class="category-icon">01</div>
                <h3>Cây Bonsai</h3>
                <p>Những tác phẩm bonsai nghệ thuật</p>
            </a>

            <a href="#" class="category-card">
                <div class="category-icon">02</div>
                <h3>Hoa</h3>
                <p>Thêm sắc màu cho không gian sống</p>
            </a>

            <a href="#" class="category-card">
                <div class="category-icon">03</div>
                <h3>Dụng cụ</h3>
                <p>Công cụ chăm sóc cây chuyên dụng</p>
            </a>

            <a href="#" class="category-card">
                <div class="category-icon">04</div>
                <h3>Chậu</h3>
                <p>Đa dạng kiểu dáng và chất liệu</p>
            </a>

            <a href="#" class="category-card">
                <div class="category-icon">05</div>
                <h3>Phân bón</h3>
                <p>Dinh dưỡng cho cây phát triển</p>
            </a>

        </div>
    </section>

    <section class="featured-section">
        <div class="container">

            <div class="section-heading">
                <span>BONSAISTORE COLLECTION</span>
                <h2>Sản phẩm nổi bật</h2>
                <p>Những sản phẩm được tuyển chọn dành cho bạn.</p>
            </div>

            <asp:Repeater ID="rptProducts" runat="server">
    <HeaderTemplate>
        <div class="product-grid">
    </HeaderTemplate>

    <ItemTemplate>
    <a href='ProductDetail.aspx?id=<%# Eval("ProductID") %>'
       class="product-card"
       style="text-decoration: none; color: inherit; display: block;">

        <div class="product-image">
            <img src='<%# Eval("ImageURL") %>'
                 alt='<%# Eval("ProductName") %>' />
        </div>

        <div class="product-info">
            <span class="product-category">
                <%# Eval("CategoryName") %>
            </span>

            <h3><%# Eval("ProductName") %></h3>

            <p class="product-price">
                <%# String.Format("{0:N0} đ", Eval("Price")) %>
            </p>

            <p class="product-stock">
                Còn lại: <%# Eval("Stock") %> sản phẩm
            </p>
        </div>

    </a>
</ItemTemplate>

    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>

        </div>
    </section>

    <section class="about-section container">
        <div class="about-content">
            <span>VỀ CHÚNG TÔI</span>
            <h2>BonsaiStore - Không gian xanh của bạn</h2>
            <p>
                Chúng tôi mang đến các sản phẩm cây cảnh,
                bonsai và dụng cụ chăm sóc cây nhằm giúp bạn
                tạo nên một không gian sống xanh, đẹp và gần gũi với thiên nhiên.
            </p>
        </div>
    </section>

</asp:Content>
