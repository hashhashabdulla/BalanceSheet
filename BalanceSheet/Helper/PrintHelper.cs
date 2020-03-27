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
        WebBrowser webBrowser = new WebBrowser();
        List<Transaction> transactionsList = new List<Transaction>();

        public void PrintDoc(List<Transaction> list)
        {
            string htmlContent = "";
            transactionsList = list;
            ////htmlContent += "<table class=\"minimalistBlack\"><tbody><tr><th>Date</th><th>Customer Name</th><th>Description</th><th>Amount</th></tr>";
            //htmlContent += "<table class=\"minimalistBlack\"> <thead> <tr> <th>Date</th> <th>Customer Name</th> <th>Description</th> <th>Amount</th> </tr> </thead> <tfoot> <tr> <td>foot1</td> <td>foot2</td> <td>foot3</td> <td>foot4</td> </tr> </tfoot> <tbody>";
            //foreach (var item in list)
            //{
            //    htmlContent += "<tr>";
            //    htmlContent += "<td>" + ((DateTime)item.CreateDate).ToShortDateString() + "</td>";
            //    htmlContent += "<td>" + item.CustomerName + "</td>";
            //    htmlContent += "<td>" + item.Description + "</td>";
            //    htmlContent += "<td>" + item.Amount.ToString() + "</td>";
            //    htmlContent += "</tr>";
            //}
            //htmlContent += "</tbody></table></html>";
            
            webBrowser.DocumentText = htmlContent;
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
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
            HtmlElement theadheaderElement3 = webBrowser.Document.CreateElement("th");
            theadheaderElement3.InnerHtml = "Description";
            theadrowElement.AppendChild(theadheaderElement3);
            HtmlElement theadheaderElement4 = webBrowser.Document.CreateElement("th");
            theadheaderElement4.InnerHtml = "Amount";
            theadrowElement.AppendChild(theadheaderElement4);

            theadElement.AppendChild(theadrowElement);
            tableElement.AppendChild(theadElement);
            #endregion

            #region TFOOT ELEMENT START
            HtmlElement tfootElement = webBrowser.Document.CreateElement("tfoot");

            HtmlElement tfootrowElement = webBrowser.Document.CreateElement("tr");

            HtmlElement tfootdataElement1 = webBrowser.Document.CreateElement("th");
            tfootdataElement1.InnerHtml = "Total";
            tfootrowElement.AppendChild(tfootdataElement1);
            HtmlElement tfootdataElement2 = webBrowser.Document.CreateElement("th");
            tfootdataElement2.InnerHtml = "foot2";
            tfootrowElement.AppendChild(tfootdataElement2);
            HtmlElement tfootdataElement3 = webBrowser.Document.CreateElement("th");
            tfootdataElement3.InnerHtml = "foot3";
            tfootrowElement.AppendChild(tfootdataElement3);
            HtmlElement tfootdataElement4 = webBrowser.Document.CreateElement("th");
            tfootdataElement4.InnerHtml = "foot4";
            tfootrowElement.AppendChild(tfootdataElement4);

            tfootElement.AppendChild(tfootrowElement);

            tableElement.AppendChild(tfootElement);
            #endregion TFOOT ELEMENT END

            HtmlElement tbodyElement = webBrowser.Document.CreateElement("tbody");
            foreach (var item in transactionsList)
            {
                HtmlElement tbodyrowElement = webBrowser.Document.CreateElement("tr");

                HtmlElement tbodydataElement1 = webBrowser.Document.CreateElement("td");
                tbodydataElement1.InnerHtml = ((DateTime)item.CreateDate).ToShortDateString();
                tbodyrowElement.AppendChild(tbodydataElement1);
                HtmlElement tbodydataElement2 = webBrowser.Document.CreateElement("td");
                tbodydataElement2.InnerHtml = item.CustomerName;
                tbodyrowElement.AppendChild(tbodydataElement2);
                HtmlElement tbodydataElement3 = webBrowser.Document.CreateElement("td");
                tbodydataElement3.InnerHtml = item.Description;
                tbodyrowElement.AppendChild(tbodydataElement3);
                HtmlElement tbodydataElement4 = webBrowser.Document.CreateElement("td");
                tbodydataElement4.SetAttribute("align", "right");
                tbodydataElement4.InnerHtml = String.Format("{0:0.00}", item.Amount);
                tbodyrowElement.AppendChild(tbodydataElement4);

                tbodyElement.AppendChild(tbodyrowElement);
            }
            tableElement.AppendChild(tbodyElement);

            AddStyles();

            webBrowser.Document.Body.Style = "font-size:13px;font-family:Microsoft San Serif;";
            webBrowser.Print();
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
