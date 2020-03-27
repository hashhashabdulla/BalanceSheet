using BalanceSheet.Dialogs;
using BalanceSheet.Helper;
using BalanceSheet.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BalanceSheet.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private Customer _selectedCustomer;
        private string _customerFirtLetter;
        private double _customerReceived;
        private double _customerPaid;
        private double _customerBalance;
        private double _transactionReceived;
        private double _transactionPaid;
        private double _transactionBalance;

        public bool DarkThemeEnabled { get; set; }
        public ICommand ThemeToggleCommand { get; set; }
        public ICommand CreateNewCustomerCommand { get; set; }
        public ICommand CreateNewCustomerEntry { get; set; }
        public ICommand CreateNewTransactionEntry { get; set; }
        public ICommand PrintClick { get; set; }
        public ObservableCollection<Customer> CustomerList { get; set; }
        public ObservableCollection<Transaction> TransactionsList { get; set; }
        public ObservableCollection<Transaction> CustomerTransactionsList { get; set; }
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (value != _selectedCustomer)
                {
                    _selectedCustomer = value;
                    OnPropertyChanged("SelectedCustomer");
                }
                if (value != null)
                {
                    SelectedCustomerOnPropertyChanged();
                }
            }
        }
        public string CustomerFirstLetter
        {
            get { return _customerFirtLetter; }
            set
            {
                if (value != _customerFirtLetter)
                {
                    _customerFirtLetter = value;
                    OnPropertyChanged("SelectedCustomer");
                }
            }
        }
        public double CustomerReceived
        {
            get { return _customerReceived; }
            set
            {
                if (value != _customerReceived)
                {
                    _customerReceived = value;
                    OnPropertyChanged("CustomerReceived");
                }
            }
        }
        public double CustomerPaid
        {
            get { return _customerPaid; }
            set
            {
                if (value != _customerPaid)
                {
                    _customerPaid = value;
                    OnPropertyChanged("CustomerPaid");
                }
            }
        }
        public double CustomerBalance
        {
            get { return _customerBalance; }
            set
            {
                if (value != _customerBalance)
                {
                    _customerBalance = value;
                    OnPropertyChanged("CustomerBalance");
                }
            }
        }
        public double TransactionReceived
        {
            get { return _transactionReceived; }
            set
            {
                if (value != _transactionReceived)
                {
                    _transactionReceived = value;
                    OnPropertyChanged("TransactionReceived");
                }
            }
        }
        public double TransactionPaid
        {
            get { return _transactionPaid; }
            set
            {
                if (value != _transactionPaid)
                {
                    _transactionPaid = value;
                    OnPropertyChanged("TransactionPaid");
                }
            }
        }
        public double TransactionBalance
        {
            get { return _transactionBalance; }
            set
            {
                if (value != _transactionBalance)
                {
                    _transactionBalance = value;
                    OnPropertyChanged("TransactionBalance");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            InitializeCommands();

            Initialize();
        }

        private void InitializeCommands()
        {
            ThemeToggleCommand = new RelayCommand<object>(p => OnThemeToggle());
            CreateNewCustomerCommand = new RelayCommand<object>(p => OnCreateNewCustomerClickedAsync());
            CreateNewCustomerEntry = new RelayCommand<object>(p => OnCreateNewCustomerEntryAsync());
            CreateNewTransactionEntry = new RelayCommand<object>(p => OnCreateNewTransactionEntryAsync());
            PrintClick = new RelayCommand<object>(p => OnPrintClick());
        }

        private void Initialize()
        {
            CustomerTransactionsList = new ObservableCollection<Transaction>();
            //Load Data
            LoadCustomerList();
            LoadTransactionList();
            //Assign first customer in the list by default to selected customer
            SelectedCustomer = CustomerList != null && CustomerList.Count > 0 ? CustomerList[0] : new Customer();
        }

        private void LoadCustomerList()
        {
            CustomerList = new ObservableCollection<Customer>(JsonHelper.LoadJsonFromFile<Customer>(@"Data\CustomerData.json"));
        }

        private void LoadTransactionList()
        {
            TransactionsList = new ObservableCollection<Transaction>(JsonHelper.LoadJsonFromFile<Transaction>(@"Data\TransactionData.json").OrderByDescending(o => o.CreateDate));
            TransactionsList.CollectionChanged += TransactionsList_CollectionChanged;
            //Call the event manually first time
            TransactionsList_CollectionChanged(null, null);
        }

        private void TransactionsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (TransactionsList != null && TransactionsList.Count > 0)
            {
                //First clear all the values
                TransactionReceived = 0;
                TransactionPaid = 0;
                TransactionBalance = 0;
                foreach (var item in TransactionsList)
                {
                    TransactionReceived += item.Received;
                    TransactionPaid -= item.Paid;
                    TransactionBalance += item.Amount;
                }
            }
        }

        private void SelectedCustomerOnPropertyChanged()
        {
            _customerFirtLetter = SelectedCustomer.CustomerName.Substring(0, 1);
            List<Transaction> transactions = TransactionsList.ToList().FindAll(o => o.CustomerName == SelectedCustomer.CustomerName);
            CustomerTransactionsList.Clear();
            CustomerReceived = 0;
            CustomerPaid = 0;
            CustomerBalance = 0;
            foreach (var item in transactions)
            {
                CustomerTransactionsList.Add(item);
                //Compute values
                CustomerReceived += item.Received;
                CustomerPaid -= item.Paid;
                CustomerBalance += item.Amount;
            }
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

            try
            {
                //show the dialog
                var result = await DialogHost.Show(createNewCustomerView, "RootDialog");

                //check the result...
                if (result != null && (bool)result)
                {
                    CreateNewCustomerViewModel customerViewModel = (CreateNewCustomerViewModel)createNewCustomerView.DataContext;
                    var existingCustomer = CustomerList.FirstOrDefault(o => o.CustomerName == customerViewModel.CustomerName);

                    //If existing customer is null, then go ahead with creation, else display alert
                    if (existingCustomer == null)
                    {
                        Customer customer = GeneralHelperClass.GetCustomerObject(new Customer(), customerViewModel.CustomerName, double.Parse(customerViewModel.OpeningBalance));
                        Transaction transaction = GeneralHelperClass.GetTransactionObject(new Transaction(), customerViewModel.CustomerName, "Opening Balance", double.Parse(customerViewModel.OpeningBalance));
                        //Add newly created customer to the customers list and transactions list
                        CustomerList.Add(customer);
                        TransactionsList.Add(transaction);
                        //Save Details to json file
                        JsonHelper.WriteToJson(CustomerList.ToList(), @"Data\CustomerData.json");
                        JsonHelper.WriteToJson(TransactionsList.ToList(), @"Data\TransactionData.json");

                        //Make newly created customer as the selected customer
                        SelectedCustomer = customer;
                    }
                    else
                    {
                        var alertDialog = new AlertDialog()
                        {
                            DataContext = new AlertDialogViewModel("Customer already exists!")
                        };

                        await DialogHost.Show(alertDialog, "RootDialog");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private async void OnCreateNewCustomerEntryAsync()
        {
            var createNewCustomerEntryDialog = new CreateNewTransaction()
            {
                DataContext = new CreateNewTransactionViewModel(CustomerList.ToList(), SelectedCustomer)
            };

            try
            {
                //Show dialog
                var result = await DialogHost.Show(createNewCustomerEntryDialog, "RootDialog");
                //check the result...
                if (result != null && (bool)result)
                {
                    CreateNewTransactionViewModel newTransactionViewModel = (CreateNewTransactionViewModel)createNewCustomerEntryDialog.DataContext;
                    //Fetch the customer and transaction
                    Customer existingCustomer = CustomerList.FirstOrDefault(o => o.CustomerName == newTransactionViewModel.SelectedCustomer.CustomerName);
                    Customer customer = GeneralHelperClass.GetCustomerObject(existingCustomer, newTransactionViewModel.SelectedCustomer.CustomerName, double.Parse(newTransactionViewModel.Amount));
                    Transaction transaction = GeneralHelperClass.GetTransactionObject(new Transaction(), newTransactionViewModel.SelectedCustomer.CustomerName, newTransactionViewModel.Description, double.Parse(newTransactionViewModel.Amount));
                    //Update Customer list and transaction list
                    int index = CustomerList.IndexOf(existingCustomer);
                    CustomerList[index] = customer;
                    TransactionsList.Add(transaction);
                    //Update selected customer
                    SelectedCustomer = CustomerList[index];
                    //Save details to json file
                    JsonHelper.WriteToJson(CustomerList.ToList(), @"Data\CustomerData.json");
                    JsonHelper.WriteToJson(TransactionsList.ToList(), @"Data\TransactionData.json");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private async void OnCreateNewTransactionEntryAsync()
        {
            if (CustomerList != null && CustomerList.Count > 0)
            {
                var createNewTransactionEntryDialog = new CreateNewTransaction()
                {
                    DataContext = new CreateNewTransactionViewModel(CustomerList.ToList())
                };

                try
                {
                    //Show dialog
                    var result = await DialogHost.Show(createNewTransactionEntryDialog, "RootDialog");
                    //check the result...
                    if (result != null && (bool)result)
                    {
                        CreateNewTransactionViewModel newTransactionViewModel = (CreateNewTransactionViewModel)createNewTransactionEntryDialog.DataContext;
                        //Fetch the customer and transaction
                        Customer existingCustomer = CustomerList.FirstOrDefault(o => o.CustomerName == newTransactionViewModel.SelectedCustomer.CustomerName);
                        Customer customer = GeneralHelperClass.GetCustomerObject(existingCustomer, newTransactionViewModel.SelectedCustomer.CustomerName, double.Parse(newTransactionViewModel.Amount));
                        Transaction transaction = GeneralHelperClass.GetTransactionObject(new Transaction(), newTransactionViewModel.SelectedCustomer.CustomerName, newTransactionViewModel.Description, double.Parse(newTransactionViewModel.Amount));
                        //Update Customer list and transaction list
                        int index = CustomerList.IndexOf(existingCustomer);
                        CustomerList[index] = customer;
                        TransactionsList.Add(transaction);
                        //Save details to json file
                        JsonHelper.WriteToJson(CustomerList.ToList(), @"Data\CustomerData.json");
                        JsonHelper.WriteToJson(TransactionsList.ToList(), @"Data\TransactionData.json");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private void OnPrintClick()
        {
            PrintHelper printHelper = new PrintHelper();
            printHelper.PrintDoc(TransactionsList.ToList());
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
