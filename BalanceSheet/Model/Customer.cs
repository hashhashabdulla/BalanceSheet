﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceSheet.Model
{
    public class Customer : INotifyPropertyChanged
    {
        private string _customerName;
        private double _totalReceived;
        private double _totalPaid;
        private double _totalBalance;
        private DateTime? _createDate;
        private DateTime? _modDate;
        
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
        public double TotalReceived
        {
            get { return _totalReceived; }
            set
            {
                if (value != _totalReceived)
                {
                    _totalReceived = value;
                    OnPropertyChanged("TotalReceived");
                }
            }
        }
        public double TotalPaid
        {
            get { return _totalPaid; }
            set
            {
                if (value != _totalPaid)
                {
                    _totalPaid = value;
                    OnPropertyChanged("TotalPaid");
                }
            }
        }
        public double TotalBalance
        {
            get { return _totalBalance; }
            set
            {
                if (value != _totalBalance)
                {
                    _totalBalance = value;
                    OnPropertyChanged("TotalBalance");
                }
            }
        }
        public DateTime? CreateDate
        {
            get { return _createDate; }
            set
            {
                if (value != _createDate)
                {
                    _createDate = value;
                    OnPropertyChanged("CreateDate");
                }
            }
        }
        public DateTime? ModDate
        {
            get { return _modDate; }
            set
            {
                if (value != _modDate)
                {
                    _modDate = value;
                    OnPropertyChanged("ModDate");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
