using BalanceSheet.Helper;
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
    class EditCustomerViewModel : INotifyPropertyChanged
    {
        private string _customerName;

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

        public ICommand OKClick { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EditCustomerViewModel(string oldCustomerName)
        {
            this.CustomerName = oldCustomerName;
            OKClick = new RelayCommand<object>(p => OnOkClick());
        }

        void OnOkClick()
        {
            if (CustomerName != null && CustomerName != "")
            {
                DialogHost.CloseDialogCommand.Execute(true, null);
            }
        }
    }
}
