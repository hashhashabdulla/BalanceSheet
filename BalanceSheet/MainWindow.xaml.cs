using BalanceSheet.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BalanceSheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        //private void OnPrintClick(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.FrameworkElement element = dataGridRecords as System.Windows.FrameworkElement;

        //    System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
        //    Printdlg.PrintTicket.PageMediaSize = new System.Printing.PageMediaSize(System.Printing.PageMediaSizeName.ISOA4);
        //    Printdlg.PrintTicket.PageOrientation = System.Printing.PageOrientation.Portrait;
        //    if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
        //    {
        //        dataGridRecords.Columns[0].Visibility = Visibility.Collapsed;

        //        //store original scale
        //        Transform originalScale = element.LayoutTransform;
        //        //get selected printer capabilities
        //        System.Printing.PrintCapabilities capabilities = Printdlg.PrintQueue.GetPrintCapabilities(Printdlg.PrintTicket);

        //        //get scale of the print wrt to screen of WPF visual
        //        double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / element.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
        //                       element.ActualHeight);

        //        //Transform the Visual to scale
        //        element.LayoutTransform = new ScaleTransform(scale, scale);

        //        //get the size of the printer page
        //        System.Windows.Size sz = new System.Windows.Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

        //        //update the layout of the visual to the printer page size.
        //        element.Measure(sz);
        //        element.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

        //        //now print the visual to printer to fit on the one page.
        //        Printdlg.PrintVisual(dataGridRecords, "My Print");

        //        //apply the original transform.
        //        element.LayoutTransform = originalScale;
        //        dataGridRecords.Columns[0].Visibility = Visibility.Visible;
        //    }
        //}
    }
}
