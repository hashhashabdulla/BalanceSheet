using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceSheet.ViewModel
{
    public class AlertDialogViewModel
    {
        public string AlertContent { get; set; }

        public AlertDialogViewModel(string alertContent)
        {
            AlertContent = alertContent;
        }
    }
}
