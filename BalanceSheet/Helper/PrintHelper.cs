using BalanceSheet.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mshtml;
using System.IO;

namespace BalanceSheet.Helper
{
    public class PrintHelper
    {
        bool isCustomerPrint;
        bool isTransactionCollapsed;
        WebBrowser webBrowser = new WebBrowser();
        List<Transaction> transactionsList = new List<Transaction>();
        private List<Transaction> list;
        private bool v;

        public PrintHelper(List<Transaction> transactionsList, bool isCustomerPrint, bool isTransactionCollapsed)
        {
            this.transactionsList = transactionsList;
            this.isCustomerPrint = isCustomerPrint;
            this.isTransactionCollapsed = isTransactionCollapsed;
        }

        public void PrintDoc()
        {
            string htmlContent = "";
            
            webBrowser.DocumentText = htmlContent;
            webBrowser.Navigating += WebBrowser_Navigating;
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            webBrowser.Width = 0;
            webBrowser.Height = 0;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (isCustomerPrint)
            {
                CustomerPrint();
            }
            else
            {
                TransactionPrint();
            }

            AddStyles();
            
            webBrowser.ShowPrintDialog();
        }

        private void CustomerPrint()
        {
            #region CUSTOMER NAME START
            if (isCustomerPrint)
            {
                HtmlElement customerNameElement = webBrowser.Document.CreateElement("h4");
                customerNameElement.InnerHtml = "Customer: " + transactionsList[0].CustomerName;
                webBrowser.Document.Body.AppendChild(customerNameElement);
            }
            #endregion CUSTOMER NAME END

            HtmlElement tableElement = webBrowser.Document.CreateElement("table");
            tableElement.SetAttribute("class", "minimalistBlack");
            webBrowser.Document.Body.AppendChild(tableElement);

            #region THEAD ELEMENT START
            HtmlElement theadElement = webBrowser.Document.CreateElement("thead");

            HtmlElement theadrowElement = webBrowser.Document.CreateElement("tr");

            HtmlElement theadheaderElement1 = webBrowser.Document.CreateElement("th");
            theadheaderElement1.InnerHtml = "Date";
            theadrowElement.AppendChild(theadheaderElement1);
            HtmlElement theadheaderElement3 = webBrowser.Document.CreateElement("th");
            theadheaderElement3.InnerHtml = "Description";
            theadrowElement.AppendChild(theadheaderElement3);
            HtmlElement theadheaderElement4 = webBrowser.Document.CreateElement("th");
            theadheaderElement4.InnerHtml = "Received";
            theadrowElement.AppendChild(theadheaderElement4);
            HtmlElement theadheaderElement5 = webBrowser.Document.CreateElement("th");
            theadheaderElement5.InnerHtml = "Paid";
            theadrowElement.AppendChild(theadheaderElement5);
            HtmlElement theadheaderElement6 = webBrowser.Document.CreateElement("th");
            theadheaderElement6.InnerHtml = "Balance";
            theadrowElement.AppendChild(theadheaderElement6);

            theadElement.AppendChild(theadrowElement);
            tableElement.AppendChild(theadElement);
            #endregion

            double totalReceived = 0;
            double totalPaid = 0;
            double totalBalance = 0;
            #region TBODY ELEMENT START
            HtmlElement tbodyElement = webBrowser.Document.CreateElement("tbody");
            foreach (var item in transactionsList)
            {
                HtmlElement tbodyrowElement = webBrowser.Document.CreateElement("tr");

                HtmlElement tbodydataElement1 = webBrowser.Document.CreateElement("td");
                tbodydataElement1.InnerHtml = ((DateTime)item.TransactionDate).ToShortDateString();
                tbodyrowElement.AppendChild(tbodydataElement1);
                HtmlElement tbodydataElement3 = webBrowser.Document.CreateElement("td");
                tbodydataElement3.InnerHtml = item.Description;
                tbodyrowElement.AppendChild(tbodydataElement3);
                HtmlElement tbodydataElement4 = webBrowser.Document.CreateElement("td");
                tbodydataElement4.SetAttribute("align", "right");
                tbodydataElement4.InnerHtml = String.Format("+{0:0.00}", item.Received);
                totalReceived += item.Received;
                tbodyrowElement.AppendChild(tbodydataElement4);
                HtmlElement tbodydataElement5 = webBrowser.Document.CreateElement("td");
                tbodydataElement5.SetAttribute("align", "right");
                tbodydataElement5.InnerHtml = String.Format("-{0:0.00}", Math.Abs(item.Paid));
                totalPaid += item.Paid;
                tbodyrowElement.AppendChild(tbodydataElement5);
                HtmlElement tbodydataElement6 = webBrowser.Document.CreateElement("td");
                tbodydataElement6.SetAttribute("align", "right");
                tbodydataElement6.InnerHtml = String.Format("{0:0.00}", item.Amount);
                totalBalance += item.Amount;
                tbodyrowElement.AppendChild(tbodydataElement6);

                tbodyElement.AppendChild(tbodyrowElement);
            }
            tableElement.AppendChild(tbodyElement);
            #endregion TBODY ELEMENT END


            #region TFOOT ELEMENT START
            HtmlElement tfootElement = webBrowser.Document.CreateElement("tfoot");

            HtmlElement tfootrowElement = webBrowser.Document.CreateElement("tr");

            HtmlElement tfootdataElement1 = webBrowser.Document.CreateElement("th");
            tfootdataElement1.InnerHtml = "Total";
            tfootrowElement.AppendChild(tfootdataElement1);
            HtmlElement tfootdataElement3 = webBrowser.Document.CreateElement("th");
            tfootdataElement3.InnerHtml = "";
            tfootrowElement.AppendChild(tfootdataElement3);
            HtmlElement tfootdataElement4 = webBrowser.Document.CreateElement("th");
            tfootdataElement4.SetAttribute("align", "right");
            tfootdataElement4.InnerHtml = String.Format("+{0:0.00}", totalReceived);
            tfootrowElement.AppendChild(tfootdataElement4);
            HtmlElement tfootdataElement5 = webBrowser.Document.CreateElement("th");
            tfootdataElement5.SetAttribute("align", "right");
            tfootdataElement5.InnerHtml = String.Format("-{0:0.00}", Math.Abs(totalPaid));
            tfootrowElement.AppendChild(tfootdataElement5);
            HtmlElement tfootdataElement6 = webBrowser.Document.CreateElement("th");
            tfootdataElement6.SetAttribute("align", "right");
            tfootdataElement6.InnerHtml = String.Format("{0:0.00}", totalBalance);
            tfootrowElement.AppendChild(tfootdataElement6);

            tfootElement.AppendChild(tfootrowElement);

            tableElement.AppendChild(tfootElement);
            #endregion TFOOT ELEMENT END
        }

        private void TransactionPrint()
        {
            HtmlElement tableElement = webBrowser.Document.CreateElement("table");
            tableElement.SetAttribute("class", "minimalistBlack");
            webBrowser.Document.Body.AppendChild(tableElement);

            #region THEAD ELEMENT START
            HtmlElement theadElement = webBrowser.Document.CreateElement("thead");

            HtmlElement theadrowElement = webBrowser.Document.CreateElement("tr");

            HtmlElement theadheaderElement1 = webBrowser.Document.CreateElement("th");
            theadheaderElement1.InnerHtml = "Date";
            theadrowElement.AppendChild(theadheaderElement1);
            HtmlElement theadheaderElement2 = webBrowser.Document.CreateElement("th");
            theadheaderElement2.InnerHtml = "Customer Name";
            theadrowElement.AppendChild(theadheaderElement2);
            if (!isTransactionCollapsed)
            {
                //Display description if transaction is not collapsed
                HtmlElement theadheaderElement3 = webBrowser.Document.CreateElement("th");
                theadheaderElement3.InnerHtml = "Description";
                theadrowElement.AppendChild(theadheaderElement3);
            }
            HtmlElement theadheaderElement4 = webBrowser.Document.CreateElement("th");
            theadheaderElement4.InnerHtml = "Amount";
            theadrowElement.AppendChild(theadheaderElement4);

            theadElement.AppendChild(theadrowElement);
            tableElement.AppendChild(theadElement);
            #endregion

            double totalBalance = 0;
            #region TBODY ELEMENT START
            HtmlElement tbodyElement = webBrowser.Document.CreateElement("tbody");
            foreach (var item in transactionsList)
            {
                HtmlElement tbodyrowElement = webBrowser.Document.CreateElement("tr");

                HtmlElement tbodydataElement1 = webBrowser.Document.CreateElement("td");
                tbodydataElement1.InnerHtml = ((DateTime)item.TransactionDate).ToShortDateString();
                tbodyrowElement.AppendChild(tbodydataElement1);
                HtmlElement tbodydataElement2 = webBrowser.Document.CreateElement("td");
                tbodydataElement2.InnerHtml = item.CustomerName;
                tbodyrowElement.AppendChild(tbodydataElement2);
                if (!isTransactionCollapsed)
                {
                    HtmlElement tbodydataElement3 = webBrowser.Document.CreateElement("td");
                    tbodydataElement3.InnerHtml = item.Description;
                    tbodyrowElement.AppendChild(tbodydataElement3); 
                }
                HtmlElement tbodydataElement4 = webBrowser.Document.CreateElement("td");
                tbodydataElement4.SetAttribute("align", "right");
                tbodydataElement4.InnerHtml = String.Format("{0:0.00}", item.Amount);
                totalBalance += item.Amount;
                tbodyrowElement.AppendChild(tbodydataElement4);

                tbodyElement.AppendChild(tbodyrowElement);
            }
            tableElement.AppendChild(tbodyElement);
            #endregion TBODY ELEMENT END


            #region TFOOT ELEMENT START
            HtmlElement tfootElement = webBrowser.Document.CreateElement("tfoot");

            HtmlElement tfootrowElement = webBrowser.Document.CreateElement("tr");

            HtmlElement tfootdataElement1 = webBrowser.Document.CreateElement("th");
            tfootdataElement1.InnerHtml = "Total";
            tfootrowElement.AppendChild(tfootdataElement1);
            HtmlElement tfootdataElement2 = webBrowser.Document.CreateElement("th");
            tfootdataElement2.InnerHtml = "";
            tfootrowElement.AppendChild(tfootdataElement2);
            if (!isTransactionCollapsed)
            {
                HtmlElement tfootdataElement3 = webBrowser.Document.CreateElement("th");
                tfootdataElement3.InnerHtml = "";
                tfootrowElement.AppendChild(tfootdataElement3); 
            }
            HtmlElement tfootdataElement4 = webBrowser.Document.CreateElement("th");
            tfootdataElement4.SetAttribute("align", "right");
            tfootdataElement4.InnerHtml = String.Format("{0:0.00}", totalBalance);
            tfootrowElement.AppendChild(tfootdataElement4);

            tfootElement.AppendChild(tfootrowElement);

            tableElement.AppendChild(tfootElement);
            #endregion TFOOT ELEMENT END
        }

        private void AddStyles()
        {
            try
            {
                if (webBrowser.Document != null)
                {
                    IHTMLDocument2 currentDocument = (IHTMLDocument2)webBrowser.Document.DomDocument;

                    int length = currentDocument.styleSheets.length;
                    IHTMLStyleSheet styleSheet = currentDocument.createStyleSheet(@"", length + 1);
                    //length = currentDocument.styleSheets.length;
                    //styleSheet.addRule("body", "background-color:blue");
                    TextReader reader = new StreamReader(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Styles/table_style.css"));
                    string style = reader.ReadToEnd();
                    styleSheet.cssText = style;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
