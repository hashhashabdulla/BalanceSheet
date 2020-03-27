using BalanceSheet.Model;
using System;
using System.Collections.Generic;

namespace BalanceSheet.Helper
{
    public class GeneralHelperClass
    {
        public static Customer GetCustomerObject(Customer currCustomer, string customerName, double amount)
        {
            Customer resultCustomer = currCustomer;
            
            //Customer Name
            resultCustomer.CustomerName = customerName;
            if(amount >= 0)
            {
                //Total Received
                resultCustomer.TotalReceived += amount;
            }
            else
            {
                //Total Paid
                resultCustomer.TotalPaid += Math.Abs(amount);
            }
            //Total Balance
            resultCustomer.TotalBalance += amount;
            //Create Date
            if(currCustomer.CreateDate == null)
            {
                //If creating customer
                resultCustomer.CreateDate = DateTime.Now;
            }
            //Mod Date
            resultCustomer.ModDate = DateTime.Now;

            return resultCustomer;
        }

        public static Transaction GetTransactionObject(Transaction currTransaction, string customerName, string description, double amount)
        {
            Transaction resultTransaction = currTransaction;

            //Customer Name
            resultTransaction.CustomerName = customerName;
            //Description
            resultTransaction.Description = description;
            if (amount >= 0)
            {
                //Total Received
                resultTransaction.Received = amount;
            }
            else
            {
                //Total Paid
                resultTransaction.Paid = Math.Abs(amount);
            }
            //Total Balance
            resultTransaction.Amount = amount;
            //Create Date
            if (currTransaction.CreateDate == null)
            {
                //If creating new transaction
                resultTransaction.CreateDate = DateTime.Now;
            }
            //Mod Date
            resultTransaction.ModDate = DateTime.Now;

            return resultTransaction;
        }

        public static List<Transaction> GetTransactionByDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<Transaction> transactions = JsonHelper.LoadJsonFromFile<Transaction>(@"Data\TransactionData.json");

                if (transactions != null && transactions.Count > 0)
                {
                    return transactions.FindAll(o => o.CreateDate >= fromDate && o.CreateDate <= toDate);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}
