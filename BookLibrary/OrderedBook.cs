using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BookLibrary
{
    public class OrderedBook
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

        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Type { get; set; }
        public string Rental { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; } = 0;
        public decimal IndividualPrice { get; set; }
        private Dictionary<string, decimal> PriceMultiplier = new Dictionary<string, decimal>
        {
            { "Hardcover", 1.75m },
            { "Paperback", 1.25m },
            {"E-Book", 0.75m },
            {"Rent", -0.35m },
            {"Buy", 0.55m }
        };

        public OrderedBook(string Title, string ISBN, string Type, string Rental, decimal Price, int Quantity)
        {
            this.Title = Title;
            this.ISBN = ISBN;
            this.Type = Type;
            this.Rental = Rental;
            this.Price = Price;
            this.Quantity = Quantity;
            string[] keys = new string[] { Type, Rental };
            OrderCost(keys);
        }
        //Used for object
        public string OrderCost(string[] keys)
        {
            TotalCost += (Price * keys.ToList().Where(PriceMultiplier.ContainsKey).Sum(k => PriceMultiplier[k])) * Quantity;
            IndividualPrice = TotalCost / Quantity;
            return TotalCost.ToString("c");
            
        }
        //Used for student input table when configuring order

        public static OrderedBook CreateOrder(GridViewRow row)
        {
            OrderedBook book;

            DropDownList ddlType = (DropDownList)row.FindControl("ddlBookType");
            DropDownList ddlRent = (DropDownList)row.FindControl("ddlRentOrBuy");
            TextBox textQuantity = (TextBox)row.FindControl("txtQuantity");
            int quantity = 1;
            if (!string.IsNullOrEmpty(textQuantity.Text))
            {
                quantity = int.Parse(textQuantity.Text);
            }

            book = new OrderedBook(
                row.Cells[(int)InputField.Title].Text,
                row.Cells[(int)InputField.ISBN].Text,
                ddlType.SelectedValue,
                ddlRent.SelectedValue,
                decimal.Parse((row.FindControl("hfBasePrice") as HiddenField).Value.Split('$')[0]),
                quantity
                );

            return book;
        }
    }
}
