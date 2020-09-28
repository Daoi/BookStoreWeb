<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="BookStoreWeb.WebForm1" EnableSessionState="true" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="style/StoreStyle.css" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" runat="server" />
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/popper.min.js"></script>
</head>

<body>

    <%--Jumbotron start--%>
    <div class="jumbotron">
        <div class="container">
            <h1 class="display-3">Temple Bookstore</h1>
        </div>
    </div>
    <%--Jumbotron End--%>    
    <%--Form start--%>
    
    <form id="form1" runat="server">
        <%--Nav start--%>
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
            <a class="navbar-brand" href="#">Temple Books</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item active">
                        <asp:LinkButton ID="lnkBtnOrderPage"
                            Text="Order Page"
                            OnClick="LinkButtonOrderPage_Click"
                            runat="server"
                            class="nav-link" />
                    </li>
                    <li class="nav-item active">
                        <asp:LinkButton ID="lnkBtnMgmtReport"
                            Text="Managment report"
                            OnClick="LinkButtonMgmtReport_Click"
                            runat="server"
                            class="nav-link" />
                    </li>
                </ul>
            </div>
        </nav>
        <%--Nav End--%>
        <div class="formContainer">
        <div class="gradient container-md inputForm">
            <div class="col-md-4 mb-3" id="divInputs" name="divInputs" runat="server">
                Name:
                <asp:TextBox ID="txtStudentName" runat="server" CssClass="form-control" required=""></asp:TextBox>
                <br />
                Student ID:
                <asp:TextBox ID="txtStudentID" runat="server" CssClass="form-control" required="" placeholder="tux#####"></asp:TextBox>
                <br />
                Address:
                <asp:TextBox ID="txtStudentAddress" runat="server" CssClass="form-control" required=""></asp:TextBox>
                <br />
                Phone Number:
                <asp:TextBox ID="txtStudentPhoneNumber" runat="server" CssClass="form-control" required="" placeholder="###-###-####"></asp:TextBox>
                <br />
                <asp:DropDownList ID="ddlCampus" runat="server">
                    <asp:ListItem>Select your Campus</asp:ListItem>
                    <asp:ListItem>Main Campus</asp:ListItem>
                    <asp:ListItem>TUCC</asp:ListItem>
                    <asp:ListItem>Ambler Campus</asp:ListItem>
                    <asp:ListItem>Tokyo Campus</asp:ListItem>
                    <asp:ListItem>Rome Campus</asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:GridView ID="gvBooksInput" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-hover"
                Width="50%">
                <Columns>
                    <asp:TemplateField HeaderText="Order?">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkBoxOrder" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Authors" HeaderText="Authors" />
                    <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                    <asp:TemplateField HeaderText="Type">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlBookType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBookType_SelectedIndexChanged">
                                <asp:ListItem>Hardcover</asp:ListItem>
                                <asp:ListItem>Paperback</asp:ListItem>
                                <asp:ListItem>E-Book</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rent or Buy">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlRentOrBuy" runat="server" OnSelectedIndexChanged="ddlRentOrBuy_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem>Rent</asp:ListItem>
                                <asp:ListItem>Buy</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQuantity" runat="server" AutoPostBack="True" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BasePrice" HeaderText="Price" DataFormatString="{0:c}"></asp:BoundField>
                    <asp:TemplateField HeaderText="BasePrice" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfBasePrice" runat="server" Value='<%# Bind("BasePrice") %>' Visible="False" />
                        </ItemTemplate>

                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>


            <asp:Button ID="btnSubmitOrder" runat="server" Text="Submit Order" OnClick="btnSubmitOrder_Click" CssClass="btn-primary" />


            <asp:Label ID="lblSubmit" runat="server"></asp:Label>

            <asp:GridView ID="gvBooksOrder" runat="server" Visible="False" CssClass="table table-condensed table-hover" Width="50%" AutoGenerateColumns="False" ShowFooter="True">
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Title" FooterText="Order Total" />
                    <asp:BoundField DataField="ISBN" HeaderText="ISBN #" />
                    <asp:BoundField DataField="Type" HeaderText="Type" />
                    <asp:BoundField DataField="Rental" HeaderText="Purchase Type" />
                    <asp:BoundField DataField="IndividualPrice" DataFormatString="{0:c}" HeaderText="Price" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="TotalCost" DataFormatString="{0:c}" HeaderText="Total Cost" />
                </Columns>
            </asp:GridView>

            <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="able table-condensed table-hover" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="TotalQuantityRented" HeaderText="Rental Sales" />
                    <asp:BoundField DataField="TotalQuantitySold" HeaderText="Purchase Sales" />
                    <asp:BoundField DataField="TotalSales" DataFormatString="{0:c}" HeaderText="Total Sales" />
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
                </>
        </div>
            </div>
        <%--Form End--%>
    </form>
</body>
</html>
