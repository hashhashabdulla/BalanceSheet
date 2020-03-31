using BalanceSheet.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public static Transaction GetTransactionObject(Transaction currTransaction, string customerName, string description, DateTime transactionDate, double amount)
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
            //Transaction Date
            resultTransaction.TransactionDate = transactionDate;
            //Create Date
            if (currTransaction.CreateDate == null)
            {
                //If creating new transaction
                resultTransaction.CreateDate = DateTime.Now;
                //Generate Id from create date
                DateTime crtDate = (DateTime)resultTransaction.CreateDate;
                resultTransaction.TransactionId = crtDate.Year.ToString() + crtDate.Month + crtDate.Day + crtDate.Hour + crtDate.Minute + crtDate.Second + crtDate.Millisecond;
            }
            //Mod Date
            resultTransaction.ModDate = DateTime.Now;

            return resultTransaction;
        }

        public static void UpdateTransactionsListByDate(ObservableCollection<Transaction> transactionsList, Customer customer, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                List<Transaction> transactions = JsonHelper.LoadJsonFromFile<Transaction>(@"Data\TransactionData.json");
                transactionsList.Clear();

                if(customer != null)
                {
                    //Filter transactions list by customer
                    transactions = transactions.FindAll(o => o.CustomerName == customer.CustomerName);
                }
                if (fromDate != null && toDate != null)
                {
                    //Filter transactions list based on fromDate and toDate
                    transactions = transactions.FindAll(o => ((DateTime)o.TransactionDate).Date >= fromDate && ((DateTime)o.TransactionDate).Date <= toDate);
                }

                foreach (Transaction transaction in transactions)
                {
                    transactionsList.Add(transaction);
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static Customer UpdateCustomer(Customer customer, List<Transaction> transactionsList)
        {
            //Get all transactions belonging to the customer
            List<Transaction> customerTransactionList = transactionsList.FindAll(o => o.CustomerName == customer.CustomerName);

            if(customerTransactionList != null && customerTransactionList.Count > 0)
            {
                //First, clear all values of customer
                customer.TotalReceived = 0;
                customer.TotalPaid = 0;
                customer.TotalBalance = 0;
                foreach (var customerTransaction in customerTransactionList)
                {
                    customer.TotalReceived += customerTransaction.Received;
                    customer.TotalPaid += customerTransaction.Paid;
                    customer.TotalBalance += customerTransaction.Amount;
                }
                customer.ModDate = DateTime.Now;
            }

            return customer;
        }

        public static List<Transaction> CreateDetails(ObservableCollection<Customer> customerList, List<Transaction> allTransactionsList, string customerName, string description, DateTime transactionDate, double amount)
        {
            //New transaction
            Transaction transaction = GetTransactionObject(new Transaction(), customerName, description, transactionDate, amount);
            allTransactionsList.Add(transaction);
            //Update Customer list
            Customer customer = customerList.FirstOrDefault(o => o.CustomerName == customerName);
            if (customer == null)
            {
                //If new Customer
                customer = GetCustomerObject(new Customer(), customerName, amount);
                customerList.Add(customer);
            }
            else
            {
                //If customer is already existing
                customer = UpdateCustomer(customer, allTransactionsList);
            }

            SaveDetails(customerList.ToList(), allTransactionsList);

            return allTransactionsList;
        }

        public static List<Transaction> EditDetails(ObservableCollection<Customer> customerList, List<Transaction> allTransactionsList, string transactionId, string customerName, string description, DateTime transactionDate, double amount)
        {
            Transaction transaction = allTransactionsList.FirstOrDefault(o => o.TransactionId == transactionId);
            Customer customer = customerList.FirstOrDefault(o => o.CustomerName == customerName);
            //Update Transaction details
            int transactionIndex = allTransactionsList.FindIndex(o => o.TransactionId == transactionId);
            allTransactionsList[transactionIndex] = GetTransactionObject(transaction, customerName, description, transactionDate, amount);
            //If a transactions customer name is being changed, customerName of transaction object will be different from the customerName variable being passed
            if (transaction.CustomerName != customerName)
            {
                //Update the old Customers details
                int oldCustomerIndex = customerList.ToList().FindIndex(o => o.CustomerName == transaction.CustomerName);
                customerList[oldCustomerIndex] = UpdateCustomer(customerList[oldCustomerIndex], allTransactionsList);
            }
            //Update customer details
            int customerIndex = customerList.ToList().FindIndex(o => o.CustomerName == customerName);
            customerList[customerIndex] = UpdateCustomer(customerList[customerIndex], allTransactionsList);

            SaveDetails(customerList.ToList(), allTransactionsList);

            return allTransactionsList;
        }

        public static List<Transaction> DeleteDetails(ObservableCollection<Customer> customerList, List<Transaction> allTransactionsList, List<Transaction> transactionsToDelete, string customerName, bool isCustomerDelete)
        {
            Customer customer = customerList.FirstOrDefault(o => o.CustomerName == customerName);
            //If isCustomerDelete is true, then all details of that customer will be deleted
            if (isCustomerDelete)
            {
                //Delete all transactions from allTransactionsList
                allTransactionsList.RemoveAll(o => o.CustomerName == customerName);
                //Remove the customer from the list
                customerList.Remove(customer);
            }
            else
            {
                //Delete the transactions
                allTransactionsList = allTransactionsList.Except(transactionsToDelete).ToList();
                //Update customer
                customer = UpdateCustomer(customer, allTransactionsList);
            }

            SaveDetails(customerList.ToList(), allTransactionsList);

            return allTransactionsList;
        }

        public static void SaveDetails(List<Customer> customerList, List<Transaction> transactionList)
        {
            JsonHelper.WriteToJson(customerList, @"Data\CustomerData.json");
            JsonHelper.WriteToJson(transactionList, @"Data\TransactionData.json");
        }
    }
}
