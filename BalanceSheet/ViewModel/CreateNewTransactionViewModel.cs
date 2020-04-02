using BalanceSheet.Helper;
using BalanceSheet.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BalanceSheet.ViewModel
{
    public class CreateNewTransactionViewModel : INotifyPropertyChanged
    {
        private string _titleText;
        private string _description;
        private string _transactionDate;
        private string _amount;
        private Customer _selectedCustomer;
        private bool _customerNameEnabled;

        public string TitleText
        {
            get { return _titleText; }
            set
            {
                if (value != _titleText)
                {
                    _titleText = value;
                    OnPropertyChanged("TitleText");
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string TransactionDate
        {
            get { return _transactionDate; }
            set
            {
                if (value != _transactionDate)
                {
                    _transactionDate = value;
                    OnPropertyChanged("TransactionDate");
                }
            }
        }

        public string Amount
        {
            get { return _amount; }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                }
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
            }
        }

        public bool CustomerNameEnabled
        {
            get { return _customerNameEnabled; }
            set
            {
                if (value != _customerNameEnabled)
                {
                    _customerNameEnabled = value;
                    OnPropertyChanged("CustomerNameEnabled");
                }
            }
        }

        public ObservableCollection<Customer> CustomerList { get; set; }

        public bool HasError { get; set; }

        public ICommand OKClick { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateNewTransactionViewModel(List<Customer> customers, string titleText)
        {
            //Create new entry with customer selection enabled
            OKClick = new RelayCommand<object>(p => OnOkClick());
            CustomerList = new ObservableCollection<Customer>(customers);
            SelectedCustomer = CustomerList[0];
            CustomerNameEnabled = true;

            TransactionDate = DateTime.Today.ToShortDateString();
            TitleText = titleText;
        }

        public CreateNewTransactionViewModel(List<Customer> customers, Customer selectedCustomer, string titleText)
        {
            //Create new entry with customer selection disabled
            OKClick = new RelayCommand<object>(p => OnOkClick());
            CustomerList = new ObservableCollection<Customer>(customers);
            SelectedCustomer = selectedCustomer;
            CustomerNameEnabled = false;

            TransactionDate = DateTime.Today.ToShortDateString();
            TitleText = titleText;
        }

        public CreateNewTransactionViewModel(List<Customer> customers, Transaction selectedTransactionItem, string titleText, bool customerSelectionEnabled)
        {
            //Edit transaction entry
            OKClick = new RelayCommand<object>(p => OnOkClick());
            CustomerList = new ObservableCollection<Customer>(customers);
            SelectedCustomer = customers.FirstOrDefault(o => o.CustomerName == selectedTransactionItem.CustomerName);
            Description = selectedTransactionItem.Description;
            Amount = selectedTransactionItem.Amount.ToString();
            CustomerNameEnabled = customerSelectionEnabled;

            TransactionDate = ((DateTime)selectedTransactionItem.TransactionDate).ToShortDateString();
            TitleText = titleText;
        }

        private void OnOkClick()
        {
            if (!HasError && TransactionDate != null && TransactionDate != "")
            {
                DialogHost.CloseDialogCommand.Execute(true, null);
            }
        }
    }
}
