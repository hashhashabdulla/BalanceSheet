using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BalanceSheet.Converter
{
    public class AmountToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double amount = 0.0;
            
            if(double.TryParse(value.ToString(), out amount))
            {
                if(amount >= 0)
                {
                    return new BrushConverter().ConvertFromString("#0CB70C");
                }
                else
                {
                    return Brushes.Red;
                }
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
