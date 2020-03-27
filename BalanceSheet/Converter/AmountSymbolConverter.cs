using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BalanceSheet.Converter
{
    public class AmountSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double amount = 0.0;

            if (double.TryParse(value.ToString(), out amount))
            {
                string result = String.Format("{0:0.00}", amount);
                if (amount >= 0)
                {
                    return "+" + result;
                }
                else
                {
                    return result;
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
