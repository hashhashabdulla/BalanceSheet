using BalanceSheet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceSheet.Helper
{
    public class GeneralHelperClass
    {
        public static Customer GetCustomerObject(Customer currCustomer, string customerName, double amount)
        {
            Customer resultCustomer = new Customer();
            //Customer Name
            resultCustomer.customerName = customerName;
            if(amount >= 0)
            {
                //Total Received
                resultCustomer.totalReceived += amount;
            }
            else
            {
                //Total Paid
                resultCustomer.totalPaid += Math.Abs(amount);
            }
            //Total Balance
            resultCustomer.totalBalance += amount;
            //Create Date
            if(currCustomer.createDate == null)
            {
                //If creating customer
                resultCustomer.createDate = DateTime.Now;
            }
            //Mod Date
            resultCustomer.modDate = DateTime.Now;

            return resultCustomer;
        }
    }
}
