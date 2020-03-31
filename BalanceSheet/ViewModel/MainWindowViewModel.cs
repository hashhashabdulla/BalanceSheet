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
        private Transaction _selectedTransactionItem;
        private Transaction _selectedCustomerTransactionItem;
        private string _customerFirtLetter;
        private double _customerReceived;
        private double _customerPaid;
        private double _customerBalance;
        private double _transactionReceived;
        private double _transactionPaid;
        private double _transactionBalance;
        private bool _isTransactionEditEnabled;
        private bool _isTransactionDeleteEnabled;
        private bool _isCustomerEditEnabled;
        private bool _isCustomerTransactionDeleteEnabled;
        private string _customerFromDate;
        private string _customerToDate;
        private string _transactionFromDate;
        private string _transactionToDate;
        private bool _customerTransactionSelectAll;
        private bool _transactionSelectAll;
        private List<Transaction> allTransactionsList = new List<Transaction>();
        List<Transaction> selectedTransactionsList = new List<Transaction>();
        List<Transaction> selectedCustomerTransactionsList = new List<Transaction>();

        public bool DarkThemeEnabled { get; set; }
        public ICommand ThemeToggleCommand { get; set; }
        public ICommand CreateNewCustomerCommand { get; set; }
        public ICommand RemoveCustomerCommand { get; set; }
        public ICommand CreateNewCustomerEntry { get; set; }
        public ICommand CreateNewTransactionEntry { get; set; }
        public ICommand PrinterSettingsBtn { get; set; }
        public ICommand CustomerPrintClick { get; set; }
        public ICommand PrintClick { get; set; }
        public ICommand TransactionEntryEditBtn { get; set; }
        public ICommand TransactionEntryDeleteBtn { get; set; }
        public ICommand CustomerEntryEditBtn { get; set; }
        public ICommand CustomerTransactionEntryDeleteBtn { get; set; }
        public ICommand CustomerClearFilterBtn { get; set; }
        public ICommand TransactionClearFilterBtn { get; set; }
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
        public Transaction SelectedTransactionItem
        {
            get { return _selectedTransactionItem; }
            set
            {
                if (value != _selectedTransactionItem)
                {
                    _selectedTransactionItem = value;
                    OnPropertyChanged("SelectedTransactionItem");
                }
            }
        }
        public Transaction SelectedCustomerTransactionItem
        {
            get { return _selectedCustomerTransactionItem; }
            set
            {
                if (value != _selectedCustomerTransactionItem)
                {
                    _selectedCustomerTransactionItem = value;
                    OnPropertyChanged("SelectedCustomerTransactionItem");
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
        public bool IsTransactionEditEnabled
        {
            get { return _isTransactionEditEnabled; }
            set
            {
                if (value != _isTransactionEditEnabled)
                {
                    _isTransactionEditEnabled = value;
                    OnPropertyChanged("IsTransactionEditEnabled");
                }
            }
        }
        public bool IsTransactionDeleteEnabled
        {
            get { return _isTransactionDeleteEnabled; }
            set
            {
                if (value != _isTransactionDeleteEnabled)
                {
                    _isTransactionDeleteEnabled = value;
                    OnPropertyChanged("IsTransactionDeleteEnabled");
                }
            }
        }
        public bool IsCustomerEditEnabled
        {
            get { return _isCustomerEditEnabled; }
            set
            {
                if (value != _isCustomerEditEnabled)
                {
                    _isCustomerEditEnabled = value;
                    OnPropertyChanged("IsCustomerEditEnabled");
                }
            }
        }
        public bool IsCustomerTransactionDeleteEnabled
        {
            get { return _isCustomerTransactionDeleteEnabled; }
            set
            {
                if (value != _isCustomerTransactionDeleteEnabled)
                {
                    _isCustomerTransactionDeleteEnabled = value;
                    OnPropertyChanged("IsCustomerTransactionDeleteEnabled");
                }
            }
        }
        public string CustomerFromDate
        {
            get { return _customerFromDate; }
            set
            {
                if (value != _customerFromDate)
                {
                    _customerFromDate = value;
                    OnPropertyChanged("CustomerFromDate");
                }
                OnCustomerFromDateChanged();
            }
        }
        public string CustomerToDate
        {
            get { return _customerToDate; }
            set
            {
                if (value != _customerToDate)
                {
                    _customerToDate = value;
                    OnPropertyChanged("CustomerToDate");
                }
                OnCustomerToDateChanged();
            }
        }
        public string TransactionFromDate
        {
            get { return _transactionFromDate; }
            set
            {
                if (value != _transactionFromDate)
                {
                    _transactionFromDate = value;
                    OnPropertyChanged("TransactionFromDate");
                }
                OnTransactionFromDateChanged();
            }
        }
        public bool CustomerTransactionSelectAll
        {
            get { return _customerTransactionSelectAll; }
            set
            {
                if (value != _customerTransactionSelectAll)
                {
                    _customerTransactionSelectAll = value;
                    OnPropertyChanged("CustomerTransactionSelectAll");
                }
                OnCustomerTransactionSelectAllChanged();
            }
        }
        public bool TransactionSelectAll
        {
            get { return _transactionSelectAll; }
            set
            {
                if (value != _transactionSelectAll)
                {
                    _transactionSelectAll = value;
                    OnPropertyChanged("TransactionSelectAll");
                }
                OnTransactionSelectAllChanged();
            }
        }

        public string TransactionToDate
        {
            get { return _transactionToDate; }
            set
            {
                if (value != _transactionToDate)
                {
                    _transactionToDate = value;
                    OnPropertyChanged("TransactionToDate");
                }
                OnTransactionToDateChanged();
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
            try
            {
                ThemeToggleCommand = new RelayCommand<object>(p => OnThemeToggle());
                CreateNewCustomerCommand = new RelayCommand<object>(p => OnCreateNewCustomerClickedAsync());
                RemoveCustomerCommand = new RelayCommand<object>(p => OnRemoveCustomerClickedAsync());
                CreateNewCustomerEntry = new RelayCommand<object>(p => OnCreateNewCustomerEntryAsync());
                CreateNewTransactionEntry = new RelayCommand<object>(p => OnCreateNewTransactionEntryAsync());
                PrinterSettingsBtn = new RelayCommand<object>(p => OnPrinterSettingsClick());
                CustomerPrintClick = new RelayCommand<object>(p => OnCustomerPrintClick());
                PrintClick = new RelayCommand<object>(p => OnPrintClick());
                TransactionEntryEditBtn = new RelayCommand<object>(p => OnTransactionEntryEditClickAsync());
                TransactionEntryDeleteBtn = new RelayCommand<object>(p => OnTransactionEntryDeleteClickAsync());
                CustomerEntryEditBtn = new RelayCommand<object>(p => OnCustomerEntryEditClickAsync());
                CustomerTransactionEntryDeleteBtn = new RelayCommand<object>(p => OnCustomerTransactionEntryDeleteClickAsync());
                CustomerClearFilterBtn = new RelayCommand<object>(p => OnCustomerClearFilterAsync());
                TransactionClearFilterBtn = new RelayCommand<object>(p => OnTransactionClearFilterAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void Initialize()
        {
            //Load Data
            LoadCustomerList();
            LoadTransactionList();
            //Assign first customer in the list by default to selected customer
            SelectedCustomer = CustomerList != null && CustomerList.Count > 0 ? CustomerList[0] : new Customer();
        }

        private void LoadCustomerList()
        {
            try
            {
                CustomerList = new ObservableCollection<Customer>(JsonHelper.LoadJsonFromFile<Customer>(@"Data\CustomerData.json"));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void LoadTransactionList()
        {
            try
            {
                allTransactionsList = JsonHelper.LoadJsonFromFile<Transaction>(@"Data\TransactionData.json").OrderByDescending(o => o.CreateDate).ToList();
                TransactionsList = new ObservableCollection<Transaction>(allTransactionsList);
                TransactionsList.CollectionChanged += TransactionsList_CollectionChanged;
                //Call the event manually first time
                TransactionsList_CollectionChanged(null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void OnCustomerTransactionSelectAllChanged()
        {
            if (CustomerTransactionsList != null)
            {
                foreach (Transaction item in CustomerTransactionsList)
                {
                    item.IsSelected = CustomerTransactionSelectAll;
                } 
            }
        }

        private void OnTransactionSelectAllChanged()
        {
            if (TransactionsList != null)
            {
                foreach (Transaction item in TransactionsList)
                {
                    item.IsSelected = TransactionSelectAll;
                }
            }
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
                    item.PropertyChanged += SelectedTransactionItemOnPropertyChanged;
                }
            }
        }

        private void SelectedTransactionItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            selectedTransactionsList = TransactionsList.ToList().FindAll(o => o.IsSelected == true);
            if (selectedTransactionsList != null)
            {
                if (selectedTransactionsList.Count == 0)
                {
                    //No rows selected. Disable edit and delete button
                    IsTransactionDeleteEnabled = false;
                    IsTransactionEditEnabled = false;

                }
                else if (selectedTransactionsList.Count == 1)
                {
                    //Single row selected. Enable edit button, enable delete button
                    IsTransactionDeleteEnabled = true;
                    IsTransactionEditEnabled = true;
                }
                else
                {
                    //Multiple rows selected. Disable edit button, enable delete button
                    IsTransactionDeleteEnabled = true;
                    IsTransactionEditEnabled = false;
                }
            }
            else
            {
                IsTransactionDeleteEnabled = false;
                IsTransactionEditEnabled = false;
            }
        }

        private void SelectedCustomerOnPropertyChanged()
        {
            try
            {
                _customerFirtLetter = SelectedCustomer.CustomerName.Substring(0, 1);
                List<Transaction> transactions = allTransactionsList.FindAll(o => o.CustomerName == SelectedCustomer.CustomerName).OrderByDescending(o => o.CreateDate).ToList();

                if(CustomerTransactionsList == null)
                {
                    CustomerTransactionsList = new ObservableCollection<Transaction>(transactions);
                    CustomerTransactionsList.CollectionChanged += CustomerTransactionsList_CollectionChanged;
                    //Call the event manually first time
                    CustomerTransactionsList_CollectionChanged(null, null);
                }
                else
                {
                    OnCustomerFromDateChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void CustomerTransactionsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (CustomerTransactionsList != null && CustomerTransactionsList.Count > 0)
            {
                //First clear all the values
                CustomerReceived = 0;
                CustomerPaid = 0;
                CustomerBalance = 0;
                foreach (var item in CustomerTransactionsList)
                {
                    CustomerReceived += item.Received;
                    CustomerPaid -= item.Paid;
                    CustomerBalance += item.Amount;
                    item.PropertyChanged += SelectedCustomerTransactionItemOnPropertyChanged;
                }
            }
        }

        private void SelectedCustomerTransactionItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            selectedCustomerTransactionsList = CustomerTransactionsList.ToList().FindAll(o => o.IsSelected == true);
            if (selectedCustomerTransactionsList != null)
            {
                if (selectedCustomerTransactionsList.Count == 0)
                {
                    //No rows selected. Disable edit and delete button
                    IsCustomerTransactionDeleteEnabled = false;
                    IsCustomerEditEnabled = false;
                }
                else if (selectedCustomerTransactionsList.Count == 1)
                {
                    //Single row selected. Enable edit button, enable delete button
                    IsCustomerTransactionDeleteEnabled = true;
                    IsCustomerEditEnabled = true;
                }
                else
                {
                    //Multiple rows selected. Disable edit button, enable delete button
                    IsCustomerTransactionDeleteEnabled = true;
                    IsCustomerEditEnabled = false;
                }
            }
            else
            {
                IsCustomerTransactionDeleteEnabled = false;
                IsCustomerEditEnabled = false;
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
                        string customerName = customerViewModel.CustomerName;
                        string description = "Opening Balance";
                        DateTime transactionDate = DateTime.Parse(customerViewModel.TransactionDate);
                        double amount = double.Parse(customerViewModel.OpeningBalance);

                        allTransactionsList = GeneralHelperClass.CreateDetails(CustomerList, allTransactionsList, customerName, description, transactionDate, amount);

                        //Make newly created customer as the selected customer
                        SelectedCustomer = CustomerList[CustomerList.Count - 1];
                        //Refresh the transactions list
                        OnTransactionFromDateChanged();
                    }
                    else
                    {
                        DialogHelper.ShowAlertDialogAsync("Customer already exists!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private async void OnRemoveCustomerClickedAsync()
        {
            if(SelectedCustomer != null)
            {
                try
                {
                    //Show dialog
                    bool result = await DialogHelper.ShowYesNoDialogAsync("Are you sure you want to Delete?");
                    //check the result...
                    if (result)
                    {
                        string transactionId = SelectedCustomerTransactionItem.TransactionId;
                        string customerName = SelectedCustomer.CustomerName;

                        allTransactionsList = GeneralHelperClass.DeleteDetails(CustomerList, allTransactionsList, null, customerName, true);

                        //Select the first customer in the list after removing
                        SelectedCustomer = CustomerList.Count > 0 ? CustomerList[0] : null;
                        //Refresh the transactions list
                        OnTransactionFromDateChanged();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private async void OnCreateNewCustomerEntryAsync()
        {
            if (CustomerList != null && CustomerList.Count > 0)
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

                        int selectedCustomerIndex = CustomerList.IndexOf(SelectedCustomer);

                        string customerName = newTransactionViewModel.SelectedCustomer.CustomerName;
                        string description = newTransactionViewModel.Description;
                        DateTime transactionDate = DateTime.Parse(newTransactionViewModel.TransactionDate);
                        double amount = double.Parse(newTransactionViewModel.Amount);

                        allTransactionsList = GeneralHelperClass.CreateDetails(CustomerList, allTransactionsList, customerName, description, transactionDate, amount);

                        //Reselect the customer because it will get unselected while modifying the CustomerList
                        SelectedCustomer = CustomerList[selectedCustomerIndex];
                        //Refresh the transactions list
                        OnTransactionFromDateChanged();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                } 
            }
        }

        private async void OnCustomerEntryEditClickAsync()
        {
            if (SelectedCustomerTransactionItem != null)
            {
                var editCustomerTransactionDialog = new CreateNewTransaction()
                {
                    DataContext = new CreateNewTransactionViewModel(CustomerList.ToList(), SelectedCustomerTransactionItem, false)
                };

                try
                {
                    //Show dialog
                    var result = await DialogHost.Show(editCustomerTransactionDialog, "RootDialog");
                    //check the result...
                    if (result != null && (bool)result)
                    {
                        CreateNewTransactionViewModel editTransactionViewModel = (CreateNewTransactionViewModel)editCustomerTransactionDialog.DataContext;

                        int selectedCustomerIndex = CustomerList.IndexOf(SelectedCustomer);

                        string transactionId = SelectedCustomerTransactionItem.TransactionId;
                        string customerName = editTransactionViewModel.SelectedCustomer.CustomerName;
                        string description = editTransactionViewModel.Description;
                        DateTime transactionDate = DateTime.Parse(editTransactionViewModel.TransactionDate);
                        double amount = double.Parse(editTransactionViewModel.Amount);

                        allTransactionsList = GeneralHelperClass.EditDetails(CustomerList, allTransactionsList, transactionId, customerName, description, transactionDate, amount);

                        //Reselect the customer because it will get unselected while modifying the CustomerList
                        SelectedCustomer = CustomerList[selectedCustomerIndex];
                        //Refresh the transactions list
                        OnTransactionFromDateChanged();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private async void OnCustomerTransactionEntryDeleteClickAsync()
        {
            if (selectedCustomerTransactionsList != null && selectedCustomerTransactionsList.Count > 0)
            {
                try
                {
                    //Show dialog
                    bool result = await DialogHelper.ShowYesNoDialogAsync("Are you sure you want to Delete?");
                    //check the result...
                    if ((bool)result)
                    {
                        int selectedCustomerIndex = CustomerList.IndexOf(SelectedCustomer);

                        string transactionId = SelectedCustomerTransactionItem.TransactionId;
                        string customerName = SelectedCustomer.CustomerName;

                        allTransactionsList = GeneralHelperClass.DeleteDetails(CustomerList, allTransactionsList, selectedCustomerTransactionsList, customerName, false);

                        //Reselect the customer because it will get unselected while modifying the CustomerList
                        SelectedCustomer = CustomerList[selectedCustomerIndex];
                        //Refresh the transactions list
                        OnTransactionFromDateChanged();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
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

                        int selectedCustomerIndex = CustomerList.IndexOf(SelectedCustomer);

                        string customerName = newTransactionViewModel.SelectedCustomer.CustomerName;
                        string description = newTransactionViewModel.Description;
                        DateTime transactionDate = DateTime.Parse(newTransactionViewModel.TransactionDate);
                        double amount = double.Parse(newTransactionViewModel.Amount);

                        allTransactionsList = GeneralHelperClass.CreateDetails(CustomerList, allTransactionsList, customerName, description, transactionDate, amount);

                        //Reselect the customer because it will get unselected while modifying the CustomerList
                        SelectedCustomer = CustomerList[selectedCustomerIndex];
                        //Refresh the transactions list
                        OnTransactionFromDateChanged();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private async void OnTransactionEntryEditClickAsync()
        {
            if(SelectedTransactionItem != null)
            {
                var editTransactionDialog = new CreateNewTransaction()
                {
                    DataContext = new CreateNewTransactionViewModel(CustomerList.ToList(), SelectedTransactionItem, true)
                };

                try
                {
                    //Show dialog
                    var result = await DialogHost.Show(editTransactionDialog, "RootDialog");
                    //check the result...
                    if (result != null && (bool)result)
                    {
                        CreateNewTransactionViewModel editTransactionViewModel = (CreateNewTransactionViewModel)editTransactionDialog.DataContext;
                        
                        int selectedCustomerIndex = CustomerList.IndexOf(SelectedCustomer);

                        string transactionId = SelectedTransactionItem.TransactionId;
                        string customerName = editTransactionViewModel.SelectedCustomer.CustomerName;
                        string description = editTransactionViewModel.Description;
                        DateTime transactionDate = DateTime.Parse(editTransactionViewModel.TransactionDate);
                        double amount = double.Parse(editTransactionViewModel.Amount);

                        allTransactionsList = GeneralHelperClass.EditDetails(CustomerList, allTransactionsList, transactionId, customerName, description, transactionDate, amount);

                        //Reselect the customer because it will get unselected while modifying the CustomerList
                        SelectedCustomer = CustomerList[selectedCustomerIndex];
                        //Refresh the transactions list
                        OnTransactionFromDateChanged();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private async void OnTransactionEntryDeleteClickAsync()
        {
            if(selectedTransactionsList != null && selectedTransactionsList.Count > 0)
            {
                try
                {
                    //Show dialog
                    bool result = await DialogHelper.ShowYesNoDialogAsync("Are you sure you want to Delete?");
                    //check the result...
                    if ((bool)result)
                    {
                        int selectedCustomerIndex = CustomerList.IndexOf(SelectedCustomer);

                        string transactionId = SelectedCustomerTransactionItem.TransactionId;
                        string customerName = SelectedCustomer.CustomerName;

                        allTransactionsList = GeneralHelperClass.DeleteDetails(CustomerList, allTransactionsList, selectedTransactionsList, customerName, false);

                        //Reselect the customer because it will get unselected while modifying the CustomerList
                        SelectedCustomer = CustomerList[selectedCustomerIndex];
                        //Refresh the transactions list
                        OnTransactionFromDateChanged();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private void OnCustomerFromDateChanged()
        {
            if (SelectedCustomer != null)
            {
                if (CustomerFromDate != null && CustomerFromDate != "")
                {
                    CustomerToDate = CustomerFromDate;

                    GeneralHelperClass.UpdateTransactionsListByDate(CustomerTransactionsList, SelectedCustomer, DateTime.Parse(CustomerFromDate), DateTime.Parse(CustomerToDate));
                }
                else
                {
                    GeneralHelperClass.UpdateTransactionsListByDate(CustomerTransactionsList, SelectedCustomer, null, null);
                } 
            }
        }

        private void OnCustomerToDateChanged()
        {
            if (SelectedCustomer != null)
            {
                if (CustomerToDate != null && CustomerToDate != "" && CustomerFromDate != null && CustomerFromDate != "")
                {
                    GeneralHelperClass.UpdateTransactionsListByDate(CustomerTransactionsList, SelectedCustomer, DateTime.Parse(CustomerFromDate), DateTime.Parse(CustomerToDate));
                }
                else
                {
                    GeneralHelperClass.UpdateTransactionsListByDate(CustomerTransactionsList, SelectedCustomer, null, null);
                } 
            }
        }

        private void OnCustomerClearFilterAsync()
        {
            CustomerFromDate = "";
            CustomerToDate = "";
        }

        private void OnTransactionFromDateChanged()
        {
            if (TransactionFromDate != null && TransactionFromDate != "")
            {
                TransactionToDate = TransactionFromDate;

                GeneralHelperClass.UpdateTransactionsListByDate(TransactionsList, null, DateTime.Parse(TransactionFromDate), DateTime.Parse(TransactionToDate));
            }
            else
            {
                GeneralHelperClass.UpdateTransactionsListByDate(TransactionsList, null, null, null);
            }
        }

        private void OnTransactionToDateChanged()
        {
            if (TransactionToDate != null && TransactionToDate != "" && TransactionFromDate != null && TransactionFromDate != "")
            {
                GeneralHelperClass.UpdateTransactionsListByDate(TransactionsList, null, DateTime.Parse(TransactionFromDate), DateTime.Parse(TransactionToDate));
            }
            else
            {
                GeneralHelperClass.UpdateTransactionsListByDate(TransactionsList, null, null, null);
            }
        }
        
        private void OnTransactionClearFilterAsync()
        {
            TransactionFromDate = "";
            TransactionToDate = "";
        }

        private void OnPrintClick()
        {
            PrintHelper printHelper = new PrintHelper(TransactionsList.ToList().OrderByDescending(o => o.TransactionDate).ToList(), false);
            printHelper.PrintDoc();
        }

        private void OnCustomerPrintClick()
        {
            PrintHelper printHelper = new PrintHelper(CustomerTransactionsList.ToList().OrderByDescending(o => o.TransactionDate).ToList(), true);
            printHelper.PrintDoc();
        }

        private void OnPrinterSettingsClick()
        {
            System.Windows.Forms.WebBrowser webBrowser = new System.Windows.Forms.WebBrowser();
            webBrowser.DocumentText = "";
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        private void WebBrowser_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            ((System.Windows.Forms.WebBrowser)sender).ShowPageSetupDialog();
        }
    }
}
