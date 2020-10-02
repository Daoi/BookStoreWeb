using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Utilities;
using BookLibrary;
using System.Text.RegularExpressions;
using System.Web.UI;
//Kevin Lynch
//3342 9/30/2020 Section 002
//Project 2 - Temple Book Store

namespace BookStoreWeb
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        enum InputField
        {
            Order,
            Title,
            Authors,
            ISBN,
            Type,
            RentOrBuy,
            Quantity,
            Price
        }
        enum OrderField
        {
            Title,
            ISBN,
            BookType,
            PurchaseType,
            Price,
            Quantity,
            TotalCost
        }

        DBConnect dbc = new DBConnect();
        Customer customer;
        UpdatePanel up;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                customer = new Customer();
                DataSet ds = dbc.GetDataSet("Select * FROM Books");
                Session["DataSet"] = ds;
                Session["Customer"] = customer;
                UpdateInputBooksGridView();
                foreach(GridViewRow row in gvBooksInput.Rows)
                {
                    OrderedBook defaultPrice = OrderedBook.CreateOrder(row);
                    UpdatePrice(defaultPrice, row);
                }
            }
            if(Session["OrderComplete"] != null && (bool)Session["OrderComplete"] == true)
            {
                Session["OrderComplete"] = false;
                ClearInputs();
            }
        }

        public void UpdateInputBooksGridView()
        {
            gvBooksInput.DataSource = (DataSet)Session["DataSet"];
            gvBooksInput.DataBind();
        }


        protected void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            //Make sure user info inputs are valid
            foreach (TextBox txt in divInputs.Controls.OfType<TextBox>().ToList())
            {
                if (!Validation.Validate(txt.Text, Regex.Split(txt.ID, "Student")[1]))
                {
                    txt.BorderColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    txt.BorderColor = System.Drawing.Color.LightGray;
                }
            }

            //Make sure they selected a campus
            if (ddlCampus.SelectedIndex == 0)
            {
                ddlCampus.BorderColor = System.Drawing.Color.Red;
                return;
            }
            else
            {
                ddlCampus.BorderColor = System.Drawing.Color.LightGray;
            }
    
            //Get every ordered book, add them to list
            List<OrderedBook> booksOrdered = new List<OrderedBook>();
            List<GridViewRow> checkedBooks = gvBooksInput.Rows.OfType<GridViewRow>()
                .Select(r => r.FindControl("chkBoxOrder") as CheckBox) //Look at row's checkbox
                .Where(cb => cb.Checked == true) //If CheckBox is checked add to list
                .Select(cb => cb.NamingContainer as GridViewRow).ToList(); //For each checkbox in list get its row and add to final list
            //Did they order at least one book?
            if (checkedBooks.Count == 0)
            {
                lblSubmit.Text = "Must order at least one book to continue.";
                return;
            }
            else
                lblSubmit.Text = "";


            foreach (GridViewRow row in checkedBooks)
            {
                OrderedBook order = null;
                TextBox txt = (TextBox)row.FindControl("txtQuantity");
                if (Validation.Validate(txt.Text, Regex.Split(txt.ID, "txt")[1]))
                {
                    order = OrderedBook.CreateOrder(row);
                    booksOrdered.Add(order);
                    txt.BorderColor = System.Drawing.Color.LightGray;
                }
                else
                {
                    txt.BorderColor = System.Drawing.Color.Red;
                    lblSubmit.Text = "Must provide a quantity for books you wish to order.";
                    return;
                }              
            }
            Session["OrderComplete"] = true;
            //Bind ordered books to Order Gridview
            Customer customer = (Customer)Session["Customer"];
            if (!string.Equals(txtStudentID.Text, customer.StudentID))
            {
                GetCustomerInfo();
                customer = (Customer)Session["Customer"];
            }

            foreach (OrderedBook order in booksOrdered)
            {
                customer.Orders.Add(order);
            }
            gvBooksOrder.DataSource = customer.Orders;
            gvBooksOrder.DataBind();

            UpdateViewOrder();
            InventoryManagement.UpdateRecords(booksOrdered);
        }

        public void GetCustomerInfo()
        {
            Customer customer = new Customer();
            customer.Address = txtStudentAddress.Text;
            customer.StudentID = txtStudentID.Text;
            customer.Name = txtStudentName.Text;
            customer.PhoneNum = txtStudentPhoneNumber.Text;
            customer.Campus = ddlCampus.SelectedValue;
            Session["Customer"] = customer;
        }


        //After an order is submitted
        public void UpdateViewOrder()
        {
            foreach (TextBox txt in divInputs.Controls.OfType<TextBox>().ToList())
                txt.Enabled = false;
            ddlCampus.Enabled = false;
            lblSubmit.Visible = false;
            gvBooksOrder.Visible = true;
            gvBooksInput.Visible = false;
            btnSubmitOrder.Visible = false;

            decimal orderTotal = gvBooksOrder.Rows.OfType<GridViewRow>()
                .Select(r => r.Cells[(int)OrderField.TotalCost] as TableCell)
                .Where(cell => !string.IsNullOrEmpty(cell.Text))
                .Sum(cell => decimal.Parse(cell.Text.Split('$')[1]));

            gvBooksOrder.Columns[(int)OrderField.TotalCost].FooterText = orderTotal.ToString("c");
            gvBooksOrder.DataBind();

        }



        //Update price column for order table

        public void UpdatePrice(OrderedBook modified, GridViewRow row)
        {
            Label price = (Label)row.FindControl("upPrice").FindControl("lblPrice");
            price.Text = modified.TotalCost.ToString("c");
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            TextBox text = (TextBox)sender;
            GridViewRow row = (GridViewRow)text.NamingContainer;
            OrderedBook modified = OrderedBook.CreateOrder(row);
            UpdatePrice(modified, row);
        }

        protected void ddlBookType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            OrderedBook modified = OrderedBook.CreateOrder(row);
            UpdatePrice(modified, row);
        }

        protected void ddlRentOrBuy_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            OrderedBook modified = OrderedBook.CreateOrder(row);
            UpdatePrice(modified, row);
        }

        //Navigation

        public void ShowOrderPage()
        {
            foreach (TextBox txt in divInputs.Controls.OfType<TextBox>().ToList())
                txt.Enabled = true;
            ddlCampus.Enabled = true;
            lblSubmit.Text = "";
            lblSubmit.Visible = true;
            gvBooksOrder.Visible = false;
            gvBooksInput.Visible = true;
            gvReport.Visible = false;
            btnSubmitOrder.Visible = true;
            divInputs.Visible = true;
        }

        protected void LinkButtonOrderPage_Click(object sender, EventArgs e)
        {
            ShowOrderPage();
        }

        protected void LinkButtonMgmtReport_Click(object sender, EventArgs e)
        {
            ShowMgmtReportPage();
        }

        private void ClearInputs()
        {
            divInputs.Controls.OfType<TextBox>().ToList().ForEach(tb => tb.Text = "");
            ddlCampus.SelectedIndex = 0;
            gvBooksInput.DataSource = null;
            gvBooksInput.DataBind();
            gvBooksInput.DataSource = (DataSet)Session["DataSet"];
            gvBooksInput.DataBind();
        }

        private void ShowMgmtReportPage()
        {
            gvReport.Visible = true;
            gvBooksOrder.Visible = false;
            gvBooksInput.Visible = false;
            divInputs.Visible = false;
            btnSubmitOrder.Visible = false;

            DataSet ds = dbc.GetDataSet("SELECT Title, TotalSales, " +
                                        "TotalQuantitySold, TotalQuantityRented " +
                                        "FROM Books " +
                                        "ORDER BY TotalSales DESC");
            gvReport.DataSource = ds;
            gvReport.DataBind();



            
        }
    }
}
