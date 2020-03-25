using BalanceSheet.Helper;
using System;
using System.Collections.Generic;
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

        public MainWindowViewModel()
        {
            ThemeToggleCommand = new RelayCommand<object>(p => this.OnThemeToggle());
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
    }
}
