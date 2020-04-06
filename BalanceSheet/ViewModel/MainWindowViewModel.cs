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
using System.Windows.Data;
using System.Windows.Input;

namespace BalanceSheet.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private Customer _selectedCustomer;
        private Transaction _selectedTransactionItem;
        private Transaction _selectedCustomerTransactionItem;
        private bool _darkThemeEnabled;
        private bool _isTransactionCollapsed;
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
        private ObservableCollection<Transaction> _transactionsList = new ObservableCollection<Transaction>();
        private ICollectionView _transactionsListDataWrapper;
        private ObservableCollection<Transaction> _customerTransactionsList = new ObservableCollection<Transaction>();
        private ICollectionView _customerTransactionsListDataWrapper;
        private List<Transaction> allTransactionsList = new List<Transaction>();
        List<Transaction> selectedTransactionsList = new List<Transaction>();
        List<Transaction> selectedCustomerTransactionsList = new List<Transaction>();

        public ICommand ThemeToggleCommand { get; set; }
        public ICommand TransactionCollapsedToggleCommand { get; set; }
        public ICommand CreateNewCustomerCommand { get; set; }
        public ICommand EditCustomerCommand { get; set; }
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
        public ObservableCollection<Transaction> TransactionsList
        {
            get { return this._transactionsList; }
            set
            {
                this._transactionsList = value;
                this.TransactionsListDataWrapper = CollectionViewSource.GetDefaultView(_transactionsList);
                OnPropertyChanged("TransactionsList");
            }
        }
        public ICollectionView TransactionsListDataWrapper
        {
            get { return this._transactionsListDataWrapper; }
            set
            {
                this._transactionsListDataWrapper = value;
                OnPropertyChanged("TransactionsListDataWrapper");
            }
        }
        public ObservableCollection<Transaction> CustomerTransactionsList
        {
            get { return this._customerTransactionsList; }
            set
            {
                this._customerTransactionsList = value;
                this.CustomerTransactionsListDataWrapper = CollectionViewSource.GetDefaultView(_customerTransactionsList);
                CustomerTransactionsListDataWrapper.CollectionChanged += OnCustomerTransactionsListDataWrapperChange;
                OnPropertyChanged("CustomerTransactionsList");
            }
        }

        public ICollectionView CustomerTransactionsListDataWrapper
        {
            get { return this._customerTransactionsListDataWrapper; }
            set
            {
                this._customerTransactionsListDataWrapper = value;
                OnPropertyChanged("CustomerTransactionsListDataWrapper");
            }
        }

        private void OnCustomerTransactionsListDataWrapperChange(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            double balance = 0;
            foreach (Transaction item in CustomerTransactionsListDataWrapper.Cast<Transaction>().ToList())
            {
                //Make paid value negative
                if (item.Paid > 0)
                {
                    item.Paid = -item.Paid; 
                }

                //Compute balance
                balance = balance + item.Received + item.Paid;
                item.Amount = balance;
            }
        }

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
                SelectedCustomerOnPropertyChanged();
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

        public bool DarkThemeEnabled
        {
            get { return _darkThemeEnabled; }
            set
            {
                if (value != _darkThemeEnabled)
                {
                    _darkThemeEnabled = value;
                    OnPropertyChanged("DarkThemeEnabled");
                }
                OnThemeToggle();
            }
        }
        public bool IsTransactionCollapsed
        {
            get { return _isTransactionCollapsed; }
            set
            {
                if (value != _isTransactionCollapsed)
                {
                    _isTransactionCollapsed = value;
                    OnPropertyChanged("IsTransactionCollapsed");
                }
                OnCustomerTransactionSelectAllChanged();
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
                    OnPropertyChanged("CustomerFirstLetter");
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
                TransactionCollapsedToggleCommand = new RelayCommand<object>(p => OnTransactionCollapsedToggle());
                CreateNewCustomerCommand = new RelayCommand<object>(p => OnCreateNewCustomerClickedAsync());
                EditCustomerCommand = new RelayCommand<object>(p => OnEditCustomerClickedAsync());
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
            DarkThemeEnabled = Properties.Settings.Default.DarkThemeEnabled;
            //Load Data
            LoadCustomerList();
            LoadTransactionList();
            //Assign first customer in the list by default to selected customer
            SelectedCustomer = CustomerList != null && CustomerList.Count > 0 ? CustomerList[0] : new Customer();
        }

        private void OnThemeToggle()
        {
            if (DarkThemeEnabled)
            {
                Properties.Settings.Default.DarkThemeEnabled = true;
                var app = (App)Application.Current;
                Uri uri = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml");
                app.ChangeTheme(uri);
            }
            else
            {
                Properties.Settings.Default.DarkThemeEnabled = false;
                var app = (App)Application.Current;
                Uri uri = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml");
                app.ChangeTheme(uri);
            }
        }

        private void OnTransactionCollapsedToggle()
        {
            if (IsTransactionCollapsed)
            {
                IsTransactionEditEnabled = false;
                IsTransactionDeleteEnabled = false;
            }
            OnTransactionFromDateChanged();
        }

        private void LoadCustomerList()
        {
            try
            {
                CustomerList = new ObservableCollection<Customer>(JsonHelper.LoadJsonFromFile<Customer>(@"Data\CustomerData.json"));

                CustomerTransactionsList = new ObservableCollection<Transaction>();
                CustomerTransactionsList.CollectionChanged += CustomerTransactionsList_CollectionChanged;
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
                allTransactionsList = JsonHelper.LoadJsonFromFile<Transaction>(@"Data\TransactionData.json").OrderByDescending(o => o.TransactionDate).ToList();
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
            //First clear all the values
            TransactionReceived = 0;
            TransactionPaid = 0;
            TransactionBalance = 0;
            if (TransactionsList != null && TransactionsList.Count > 0)
            {
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

            if (selectedTransactionsList != null && !IsTransactionCollapsed)
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
                CustomerFirstLetter = SelectedCustomer.CustomerName.Substring(0, 1);

                OnCustomerFromDateChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void CustomerTransactionsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int customerTransactionsCount = 0;
            if (CustomerFromDate != null && CustomerFromDate != "" && CustomerToDate != null && CustomerToDate != "")
            {
                customerTransactionsCount = GeneralHelperClass.GetTransactionsListCountByDate(SelectedCustomer, DateTime.Parse(CustomerFromDate), DateTime.Parse(CustomerToDate));
            }
            else
            {
                customerTransactionsCount = GeneralHelperClass.GetTransactionsListCountByDate(SelectedCustomer, null, null);
            }
            if (CustomerTransactionsList.Count >= customerTransactionsCount)
            {
                //First clear all the values
                CustomerReceived = 0;
                CustomerPaid = 0;
                CustomerBalance = 0;
                if (CustomerTransactionsList != null && CustomerTransactionsList.Count > 0)
                {
                    foreach (var item in CustomerTransactionsList)
                    {
                        CustomerReceived += item.Received;
                        CustomerPaid += item.Paid;
                        item.PropertyChanged += SelectedCustomerTransactionItemOnPropertyChanged;
                    }
                    CustomerBalance = CustomerReceived + CustomerPaid;
                }

                CustomerTransactionsListDataWrapper.SortDescriptions.Add(new SortDescription("TransactionDate", ListSortDirection.Ascending));
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
                        string description = AppConstants.OPENING_BALANCE;
                        DateTime transactionDate = ((DateTime)customerViewModel.TransactionDate).Date;
                        transactionDate = transactionDate.Date + ((DateTime)customerViewModel.TransactionTime).TimeOfDay;
                        double amount = double.Parse(customerViewModel.OpeningBalance);

                        allTransactionsList = GeneralHelperClass.CreateDetails(CustomerList, allTransactionsList, customerName, description, transactionDate, amount);

                        //Make newly created customer as the selected customer
                        SelectedCustomer = CustomerList[CustomerList.Count - 1];
                        //Refresh the transactions list
                        OnTransactionFromDateChanged();
                    }
                    else
                    {
                        DialogHelper.ShowAlertDialogAsync(AppConstants.ERR_CUSTOMER_ALREADY_EXISTS);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private async void OnEditCustomerClickedAsync()
        {
            if (SelectedCustomer != null)
            {
                //Get the old Customer name
                string oldCustomerName = SelectedCustomer.CustomerName;

                var editCustomerView = new EditCustomer()
                {
                    DataContext = new EditCustomerViewModel(oldCustomerName)
                };

                try
                {
                    //show the dialog
                    var result = await DialogHost.Show(editCustomerView, "RootDialog");

                    //check the result...
                    if (result != null && (bool)result)
                    {
                        EditCustomerViewModel customerViewModel = (EditCustomerViewModel)editCustomerView.DataContext;
                        var existingCustomer = CustomerList.FirstOrDefault(o => o.CustomerName == customerViewModel.CustomerName);

                        //If existing customer is null, then go ahead with editing, else display alert
                        if (existingCustomer == null)
                        {
                            string newCustomerName = customerViewModel.CustomerName;

                            allTransactionsList = GeneralHelperClass.EditCustomerName(CustomerList, allTransactionsList, oldCustomerName, newCustomerName);

                            //Make edited customer as the selected customer
                            int customerIndex = CustomerList.ToList().FindIndex(o => o.CustomerName == newCustomerName);
                            SelectedCustomer = CustomerList[customerIndex];
                            //Refresh the transactions list
                            OnTransactionFromDateChanged();
                        }
                        else
                        {
                            DialogHelper.ShowAlertDialogAsync(AppConstants.ERR_CUSTOMER_ALREADY_EXISTS);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                } 
            }
        }

        private async void OnRemoveCustomerClickedAsync()
        {
            if(SelectedCustomer != null)
            {
                try
                {
                    //Show dialog
                    bool result = await DialogHelper.ShowYesNoDialogAsync(AppConstants.MSG_DELETE_CONFIRM);
                    //check the result...
                    if (result)
                    {
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
                    DataContext = new CreateNewTransactionViewModel(CustomerList.ToList(), SelectedCustomer, AppConstants.NEW_ENTRY)
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
                        DateTime transactionDate = ((DateTime)newTransactionViewModel.TransactionDate).Date;
                        transactionDate = transactionDate.Date + ((DateTime)newTransactionViewModel.TransactionTime).TimeOfDay;
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
                    DataContext = new CreateNewTransactionViewModel(CustomerList.ToList(), SelectedCustomerTransactionItem, AppConstants.EDIT_ENTRY, false)
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
                        DateTime transactionDate = ((DateTime)editTransactionViewModel.TransactionDate).Date;
                        transactionDate = transactionDate.Date + ((DateTime)editTransactionViewModel.TransactionTime).TimeOfDay;
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
                    bool result = await DialogHelper.ShowYesNoDialogAsync(AppConstants.MSG_DELETE_CONFIRM);
                    //check the result...
                    if ((bool)result)
                    {
                        int selectedCustomerIndex = CustomerList.IndexOf(SelectedCustomer);

                        string transactionId = SelectedCustomerTransactionItem.TransactionId;
                        string customerName = SelectedCustomer.CustomerName;
                        List<Transaction> selectedList = new List<Transaction>();
                        foreach (var item in selectedCustomerTransactionsList)
                        {
                            selectedList.Add(allTransactionsList.First(o => o.TransactionId == item.TransactionId));
                        }

                        allTransactionsList = GeneralHelperClass.DeleteDetails(CustomerList, allTransactionsList, selectedList, customerName, false);

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
                    DataContext = new CreateNewTransactionViewModel(CustomerList.ToList(), AppConstants.NEW_ENTRY)
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
                        DateTime transactionDate = ((DateTime)newTransactionViewModel.TransactionDate).Date;
                        transactionDate = transactionDate.Date + ((DateTime)newTransactionViewModel.TransactionTime).TimeOfDay;
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
                    DataContext = new CreateNewTransactionViewModel(CustomerList.ToList(), SelectedTransactionItem, AppConstants.EDIT_ENTRY, true)
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
                        DateTime transactionDate = ((DateTime)editTransactionViewModel.TransactionDate).Date;
                        transactionDate = transactionDate.Date + ((DateTime)editTransactionViewModel.TransactionTime).TimeOfDay;
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
                    bool result = await DialogHelper.ShowYesNoDialogAsync(AppConstants.MSG_DELETE_CONFIRM);
                    //check the result...
                    if ((bool)result)
                    {
                        int selectedCustomerIndex = CustomerList.IndexOf(SelectedCustomer);
                        
                        string customerName = SelectedCustomer.CustomerName;
                        List<Transaction> selectedList = new List<Transaction>();
                        foreach (var item in selectedTransactionsList)
                        {
                            selectedList.Add(allTransactionsList.First(o => o.TransactionId == item.TransactionId));
                        }


                        allTransactionsList = GeneralHelperClass.DeleteDetails(CustomerList, allTransactionsList, selectedList, customerName, false);

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

                    GeneralHelperClass.UpdateTransactionsListByDate(allTransactionsList, CustomerTransactionsList, SelectedCustomer, DateTime.Parse(CustomerFromDate), DateTime.Parse(CustomerToDate), false);
                }
                else
                {
                    GeneralHelperClass.UpdateTransactionsListByDate(allTransactionsList, CustomerTransactionsList, SelectedCustomer, null, null, false);
                } 
            }
            else
            {
                CustomerTransactionsList.Clear();
            }
        }

        private void OnCustomerToDateChanged()
        {
            if (SelectedCustomer != null)
            {
                if (CustomerToDate != null && CustomerToDate != "" && CustomerFromDate != null && CustomerFromDate != "")
                {
                    GeneralHelperClass.UpdateTransactionsListByDate(allTransactionsList, CustomerTransactionsList, SelectedCustomer, DateTime.Parse(CustomerFromDate), DateTime.Parse(CustomerToDate), false);
                }
                else
                {
                    GeneralHelperClass.UpdateTransactionsListByDate(allTransactionsList, CustomerTransactionsList, SelectedCustomer, null, null, false);
                } 
            }
            else
            {
                CustomerTransactionsList.Clear();
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

                GeneralHelperClass.UpdateTransactionsListByDate(allTransactionsList, TransactionsList, null, DateTime.Parse(TransactionFromDate), DateTime.Parse(TransactionToDate), IsTransactionCollapsed);
            }
            else
            {
                GeneralHelperClass.UpdateTransactionsListByDate(allTransactionsList, TransactionsList, null, null, null, IsTransactionCollapsed);
            }
        }

        private void OnTransactionToDateChanged()
        {
            if (TransactionToDate != null && TransactionToDate != "" && TransactionFromDate != null && TransactionFromDate != "")
            {
                GeneralHelperClass.UpdateTransactionsListByDate(allTransactionsList, TransactionsList, null, DateTime.Parse(TransactionFromDate), DateTime.Parse(TransactionToDate), IsTransactionCollapsed);
            }
            else
            {
                GeneralHelperClass.UpdateTransactionsListByDate(allTransactionsList, TransactionsList, null, null, null, IsTransactionCollapsed);
            }
        }
        
        private void OnTransactionClearFilterAsync()
        {
            TransactionFromDate = "";
            TransactionToDate = "";
        }

        private void OnPrintClick()
        {
            PrintHelper printHelper = new PrintHelper(TransactionsListDataWrapper.Cast<Transaction>().ToList(), false, IsTransactionCollapsed);
            printHelper.PrintDoc();
        }

        private void OnCustomerPrintClick()
        {
            PrintHelper printHelper = new PrintHelper(CustomerTransactionsListDataWrapper.Cast<Transaction>().ToList(), true, false);
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
