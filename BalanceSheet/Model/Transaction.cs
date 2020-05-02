using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceSheet.Model
{
    public class Transaction : INotifyPropertyChanged
    {
        private string _transactionId;
        private string _customerName;
        private string _description;
        private double _received;
        private double _paid;
        private double _amount;
        private DateTime? _transactionDate;
        private DateTime? _createDate;
        private DateTime? _modDate;
        private bool _isSelected;

        public string TransactionId
        {
            get { return _transactionId; }
            set
            {
                if (value != _transactionId)
                {
                    _transactionId = value;
                    OnPropertyChanged("TransactionId");
                }
            }
        }
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
        public double Received
        {
            get { return _received; }
            set
            {
                if (value != _received)
                {
                    _received = value;
                    OnPropertyChanged("Received");
                }
            }
        }
        public double Paid
        {
            get { return _paid; }
            set
            {
                if (value != _paid)
                {
                    _paid = value;
                    OnPropertyChanged("Paid");
                }
            }
        }
        public double TransAmount
        {
            get { return _received + _paid; }
        }
        public double Amount
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
        public DateTime? TransactionDate
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
        [JsonIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
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
