using BalanceSheet.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceSheet.Helper
{
    public class SQLiteHelper
    {
        public static List<Customer> LoadCustomerList()
        {
            using(IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = conn.Query<Customer>("select * from Customer", new DynamicParameters());
                return output.ToList();
            }
        }

        public static List<Transaction> LoadTransactionList()
        {
            using(IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = conn.Query<Transaction>("select * from [Transaction]", new DynamicParameters());
                return output.ToList();
            }
        }

        public static List<Transaction> LoadCustomerTransactionList(Customer customer, string fromDate=null, string toDate=null)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "select * from [Transaction] where CustomerName = '" + customer.CustomerName + "'";
                if (fromDate != null && toDate != null)
                {
                    query += " AND TransactionDate >= '" + DateTime.Parse(fromDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz") + "' AND TransactionDate <= '" + DateTime.Parse(toDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz") + "'";
                }

                var output = conn.Query<Transaction>(query, new DynamicParameters());
                return output.ToList();
            }
        }

        public static List<Transaction> LoadTransactionListByDate(Boolean isTransactionCollapsed, string fromDate = null, string toDate = null)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "";

                if (isTransactionCollapsed)
                {
                    query += "select MAX(TransactionId) AS TransactionId, CustomerName, NULL as Description, CAST(SUM(Received) AS REAL) AS Received, CAST(SUM(Paid) AS REAL) AS Paid, CAST(SUM(TransAmount) AS REAL) AS TransAmount, CAST(SUM(Amount) AS REAL) AS Amount, MAX(TransactionDate) AS TransactionDate, MAX(CreateDate) AS CreateDate, MAX(ModDate) AS ModDate FROM [Transaction]";

                    if (fromDate != null && toDate != null)
                    {
                        query += " WHERE TransactionDate >= '" + DateTime.Parse(fromDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz") + "' AND TransactionDate <= '" + DateTime.Parse(toDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz") + "'";
                    }

                    query += " GROUP BY CustomerName";

                }
                else
                {
                    query += "select * from [Transaction]";

                    if (fromDate != null && toDate != null)
                    {
                        query += " WHERE TransactionDate >= '" + DateTime.Parse(fromDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz") + "' AND TransactionDate <= '" + DateTime.Parse(toDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz") + "'";
                    }
                }

                var output = conn.Query<Transaction>(query, new DynamicParameters());
                return output.ToList();
            }
        }

        public static void UpdateCustomerName(string oldName, string newName)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                // Update Customer name in Customer table
                string query = "UPDATE Customer SET CustomerName = '" + newName + "' WHERE CustomerName = '" + oldName + "'";
                conn.Execute(query);

                // Update Customer name in all transactions
                query = "UPDATE [Transaction] SET CustomerName = '" + newName + "' WHERE CustomerName = '" + oldName + "'";
                conn.Execute(query);
            }
        }

        public static void UpdateCustomer(string customerName)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                string modDate = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");

                string selectQuery = "SELECT SUM(Received) AS TotalReceived, SUM(Paid) AS TotalPaid, SUM(TransAmount) AS TotalTransAmount from [Transaction] WHERE CustomerName = '" + customerName + "'";
                var output = conn.Query<Object>(selectQuery, new DynamicParameters());
                var data = (IDictionary<string, object>)output.ElementAt(0);

                double totalReceived = Double.Parse(data["TotalReceived"] != null ? data["TotalReceived"].ToString(): "0");
                double totalPaid = Double.Parse(data["TotalPaid"] != null ? data["TotalPaid"].ToString(): "0");
                double totalBalance = totalReceived + totalPaid;

                Customer updatedCustomer = new Customer()
                {
                    CustomerName = customerName,
                    TotalReceived = totalReceived,
                    TotalPaid = totalPaid,
                    TotalBalance = totalBalance
                };

                // Update Customer name in Customer table
                string query = "UPDATE [Customer] SET TotalReceived = @TotalReceived, TotalPaid = @TotalPaid, TotalBalance = @TotalBalance, ModDate = '" + modDate + "' WHERE CustomerName = @CustomerName";
                
                conn.Execute(query, updatedCustomer);
            }
        }

        public static void InsertCustomerDetails(Customer customer)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                string createDate = ((DateTime)customer.CreateDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                string modDate = ((DateTime)customer.ModDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");

                string query = "INSERT INTO Customer VALUES (@CustomerName, @TotalReceived, @TotalPaid, @TotalBalance, '" + createDate + "', '" + modDate + "')";
                conn.Execute(query, customer);
            }
        }

        public static void InsertTransactionDetails(Transaction transaction)
        {
            using(IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                string transactionDate = ((DateTime)transaction.TransactionDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                string createDate = ((DateTime)transaction.CreateDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                string modDate = ((DateTime)transaction.ModDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");

                string query = "INSERT INTO [Transaction] VALUES (@TransactionId, @CustomerName, @Description, @Received, @Paid, @TransAmount, @Amount, '" + transactionDate + "', '" + createDate + "', '" + modDate + "')";
                conn.Execute(query, transaction);
            }
        }

        public static void UpdateTransactionDetails(Transaction transaction)
        {
            using(IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                string transactionDate = ((DateTime)transaction.TransactionDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                string modDate = ((DateTime)transaction.ModDate).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");

                string query = "UPDATE [Transaction] SET CustomerName = @CustomerName, Description = @Description, Received = @Received, Paid = @Paid, TransAmount = @TransAmount, Amount = @Amount, TransactionDate = '" + transactionDate + "', ModDate = '" + modDate + "' WHERE TransactionId = @TransactionId";
                var result = conn.Execute(query, transaction);
            }
        }

        public static void DeleteTransactionDetails(List<Transaction> transactions, string customerName, Boolean isCustomerDelete)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                if (isCustomerDelete)
                {
                    // Delete all the transaction details belonging to the customer and then delete the customer as well
                    string deleteTransactionQuery = "DELETE FROM [Transaction] WHERE CustomerName = '" + customerName + "'";
                    conn.Execute(deleteTransactionQuery);

                    string deleteCustomerQuery = "DELETE FROM Customer WHERE CustomerName = '" + customerName + "'";
                    conn.Execute(deleteCustomerQuery);
                } 
                else
                {
                    // Get transaction Ids and customer names of the deleted transactions
                    List<string> transactionIds = new List<string>();
                    List<string> customerNames = new List<string>();
                    foreach (Transaction transactionItem in transactions)
                    {
                        transactionIds.Add(transactionItem.TransactionId);

                        if (!customerNames.Contains(transactionItem.CustomerName))
                        {
                            customerNames.Add(transactionItem.CustomerName);
                        }
                    }

                    string transactionsStr = string.Join(",", transactionIds);

                    // Delete all the transaction details
                    string deleteTransactionQuery = "DELETE FROM [Transaction] WHERE TransactionId IN (" + transactionsStr + ")";
                    conn.Execute(deleteTransactionQuery);

                    // Update all the customers whose transactions were deleted
                    foreach(string name in customerNames)
                    {
                        UpdateCustomer(name);
                    }
                }
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
