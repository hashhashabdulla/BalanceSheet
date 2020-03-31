using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceSheet.ViewModel
{
    public class YesNoDialogViewModel
    {
        public string YesNoContent { get; set; }

        public YesNoDialogViewModel(string yesNoContent)
        {
            YesNoContent = yesNoContent;
        }
    }
}
