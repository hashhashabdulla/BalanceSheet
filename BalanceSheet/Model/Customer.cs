using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceSheet.Model
{
    public class Customer
    {
        public string customerName;
        public double totalReceived;
        public double totalPaid;
        public double totalBalance;
        public DateTime? createDate;
        public DateTime? modDate;
    }
}
