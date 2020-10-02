using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace BookLibrary
{
    public class InventoryManagement
    {
        static DBConnect dbc = new DBConnect();

        public static void UpdateRecords(List<OrderedBook> orders)
        {
            //Rentals
            orders.Where(o => string.Equals(o.Rental, "Rent")).ToList()
                  .ForEach(rental => dbc.DoUpdate(
                      $"UPDATE Books SET TotalQuantityRented += {rental.Quantity}, " +
                      $"TotalSales += {rental.TotalCost} WHERE ISBN = '{rental.ISBN}'")
                      );

            //Purchases
            orders.Where(o => string.Equals(o.Rental, "Buy")).ToList()
                  .ForEach(purchase => dbc.DoUpdate(
                      $"UPDATE Books SET TotalQuantitySold += {purchase.Quantity}, " +
                      $"TotalSales += {purchase.TotalCost} WHERE ISBN = '{purchase.ISBN}'")
                      );
        }

    }
}
