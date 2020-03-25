using BalanceSheet.Dialogs;
using BalanceSheet.Helper;
using BalanceSheet.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BalanceSheet.ViewModel
{
    class MainWindowViewModel
    {
        public bool DarkThemeEnabled { get; set; }
        public ICommand ThemeToggleCommand { get; set; }
        public ICommand CreateNewCustomerCommand { get; set; }

        public MainWindowViewModel()
        {
            ThemeToggleCommand = new RelayCommand<object>(p => OnThemeToggle());
            CreateNewCustomerCommand = new RelayCommand<object>(p => OnCreateNewCustomerClickedAsync());
        }

        private void OnThemeToggle()
        {
            if (DarkThemeEnabled)
            {
                var app = (App)Application.Current;
                Uri uri = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml");
                app.ChangeTheme(uri); 
            }
            else
            {
                var app = (App)Application.Current;
                Uri uri = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml");
                app.ChangeTheme(uri);
            }
        }

        private async void OnCreateNewCustomerClickedAsync()
        {
            var createNewCustomerView = new CreateNewCustomer()
            {
                DataContext = new CreateNewCustomerViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(createNewCustomerView, "RootDialog");

            //check the result...
            if(result != null && (bool)result)
            {
                //Save Customer details to json file
                List<Customer> customers = JsonHelper.LoadJsonFromFile<Customer>(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Data\CustomerData.json"));

                CreateNewCustomerViewModel customerViewModel = (CreateNewCustomerViewModel)createNewCustomerView.DataContext;
                Customer customer = GeneralHelperClass.GetCustomerObject(new Customer(), customerViewModel.CustomerName, double.Parse(customerViewModel.OpeningBalance));
                //Add newly created customer to the customers list
                customers.Add(customer);

                JsonHelper.WriteToJson<Customer>(customers, Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Data\CustomerData.json"));
            }
        }

        //Method to display alert messages
        private async void ShowAlertDialogAsync(string message)
        {
            var alertDialog = new AlertDialog()
            {
                DataContext = new AlertDialogViewModel(message)
            };

            //show alert dialog
            await DialogHost.Show(alertDialog, "RootDialog");
        }
    }
}
