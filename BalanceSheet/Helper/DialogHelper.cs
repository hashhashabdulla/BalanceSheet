using BalanceSheet.Dialogs;
using BalanceSheet.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceSheet.Helper
{
    public class DialogHelper
    {
        //Method to display alert messages
        public static async void ShowAlertDialogAsync(string message)
        {
            var alertDialog = new AlertDialog()
            {
                DataContext = new AlertDialogViewModel(message)
            };

            //show alert dialog
            await DialogHost.Show(alertDialog, "RootDialog");
        }

        //Method to display Yes No Dialog
        public static async Task<bool> ShowYesNoDialogAsync(string message)
        {
            var yesNoDialog = new YesNoDialog()
            {
                DataContext = new YesNoDialogViewModel(message)
            };

            //show alert dialog
            var result = await DialogHost.Show(yesNoDialog, "RootDialog");

            return (result != null ? (bool)result : false);
        }
    }
}
