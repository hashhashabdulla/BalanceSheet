﻿using BalanceSheet.Helper;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BalanceSheet.ViewModel
{
    class CreateNewCustomerViewModel : INotifyPropertyChanged
    {
        private string _customerName;
        private string _transactionDate;
        private string _openingBalance;

        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                if (value != _customerName)
                {
                    _customerName = value;
                    OnPropertyChanged("CustomerName");
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

        public string OpeningBalance
        {
            get { return _openingBalance; }
            set
            {
                if (value != _openingBalance)
                {
                    _openingBalance = value;
                    OnPropertyChanged("OpeningBalance");
                }
            }
        }

        public bool HasError { get; set; }

        public ICommand OKClick { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateNewCustomerViewModel()
        {
            OKClick = new RelayCommand<object>(p => OnOkClick());
            TransactionDate = DateTime.Today.ToShortTimeString();
        }

        void OnOkClick()
        {
            if (!HasError && CustomerName != null && CustomerName != "" && TransactionDate != null && TransactionDate != "")
            {
                DialogHost.CloseDialogCommand.Execute(true, null);
            }
        }
    }
}
