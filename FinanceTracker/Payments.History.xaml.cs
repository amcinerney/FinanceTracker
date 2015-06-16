using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
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

namespace FinanceTracker
{
    /// <summary>
    /// Interaction logic for Payments.xaml
    /// </summary>
    public partial class PaymentsHistoryForm : Window
    {
        List<string> years = new List<string>();
        List<string> creditors = new List<string>();

        SQLiteConnection dbConnection = MainWindow.dbConnection;

        public PaymentsHistoryForm(SQLiteDataReader results)
        {
            InitializeComponent();
            PopulateDropdowns(results);
        }

        private void PHCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public class PaymentHistory
        {
            public string creditor { get; set; }
            public string amount { get; set; }
            public string date { get; set; }
        }

        private void PopulateDropdowns(SQLiteDataReader results)
        {
            phYearCB.Items.Clear();
            phCreditorCB.Items.Clear();

            while (results.Read())
            {
                DateTime date = Convert.ToDateTime(results["datePaid"]);
                string creditor = Convert.ToString(results["name"]);
                years.Add(date.ToString("yyyy"));
                creditors.Add(creditor.ToString());
            }

            List<string> y = years.Distinct().ToList();
            List<string> c = creditors.Distinct().ToList();

            phYearCB.Items.Add("All");
            foreach (string item in y)
            {
                phYearCB.Items.Add(item);
            }

            phCreditorCB.Items.Add("All");
            foreach (string item in c)
            {
                phCreditorCB.Items.Add(item);
            }

            years.Clear();
            creditors.Clear();
        }

        private void OptionSelected(object sender, System.EventArgs e)
        {
            string year = phYearCB.Text;
            string month = phMonthCB.Text;
            string creditor = phCreditorCB.Text;

            if (year != "" && month != "" && creditor != "")
            {
                DisplayData(year, month, creditor);
            }
        }

        private void DisplayData(string year, string month, string creditor)
        {
            paymentHistoryGrid.Items.Clear();
            string query = "";
            double total = 0;
            int dividend = 0;
            double average = 0;

            if (year == "All" && month == "All" && creditor == "All")
            {
                query = "select creditorId, datePaid, amount, name from payments INNER JOIN creditors on payments.creditorId = creditors.id WHERE username = @username ORDER BY name";
            }
            else if (year == "All" && month != "All" && creditor == "All")
            {
                string monthNum = DetermineMonthNumber(month);
                query = "select creditorId, datePaid, amount, name from payments INNER JOIN creditors on payments.creditorId = creditors.id WHERE username = @username AND strftime('%m', datePaid) = '" + monthNum + "' ORDER BY name";

            }
            else if (year == "All" && month != "All" && creditor != "All")
            {
                string monthNum = DetermineMonthNumber(month);
                query = "select creditorId, datePaid, amount, name from payments INNER JOIN creditors on payments.creditorId = creditors.id WHERE username = @username AND strftime('%m', datePaid) = '" + monthNum + "' AND name = '" + creditor + "' ORDER BY name";
            }
            else if (year == "All" && month == "All" && creditor != "All")
            {
                query = "select creditorId, datePaid, amount, name from payments INNER JOIN creditors on payments.creditorId = creditors.id WHERE username = @username AND name = '" + creditor + "' ORDER BY name";
            }
            else if (year != "All" && month == "All" && creditor == "All")
            {
                query = "select creditorId, datePaid, amount, name from payments INNER JOIN creditors on payments.creditorId = creditors.id WHERE username = @username AND strftime('%Y', datePaid) = '" + year + "' ORDER BY name";
            }
            else if (year != "All" && month != "All" && creditor == "All")
            {
                string monthNum = DetermineMonthNumber(month);
                query = "select creditorId, datePaid, amount, name from payments INNER JOIN creditors on payments.creditorId = creditors.id WHERE username = @username AND strftime('%Y-%m', datePaid) = '" + year + "-" + monthNum + "' ORDER BY name";
            }
            else if (year != "All" && month == "All" && creditor != "All")
            {
                query = "select creditorId, datePaid, amount, name from payments INNER JOIN creditors on payments.creditorId = creditors.id WHERE username = @username AND strftime('%Y', datePaid) = '" + year + "' AND name = '" + creditor + "' ORDER BY name";

            }
            else if (year != "All" && month != "All" && creditor != "All")
            {
                string monthNum = DetermineMonthNumber(month);
                query = "select creditorId, datePaid, amount, name from payments INNER JOIN creditors on payments.creditorId = creditors.id WHERE username = @username AND strftime('%Y-%m', datePaid) = '" + year + "-" + monthNum + "' AND name = '" + creditor + "' ORDER BY name";
            }
            else
            {
                MessageBox.Show("Unhandled Case found");
            }

            SQLiteDataReader results = MainWindow.QueryPaymentsTable(query);

            while (results.Read())
            {
                string c = Convert.ToString(results["name"]);
                double a = Convert.ToDouble(results["amount"]);
                DateTime d = Convert.ToDateTime(results["datePaid"]);
                total += a;
                dividend += 1;
                paymentHistoryGrid.Items.Add(new PaymentHistory { creditor = c, amount = a.ToString("C2"), date = d.ToString("MM/dd/yyyy") });
            }

            if (dividend == 0)
            {
                average = 0;
            }
            else
            {
                average = total / dividend;
            }

            phTotalLabel.Content = total.ToString("C2");
            phAverageLabel.Content = average.ToString("C2");
            MainWindow.dbConnection.Close();
        }
        
        private string DetermineMonthNumber(string month)
        {
            int monthNum = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;
            string mn;

            // month number needs to be formatted with a leading 0 for months less than 10
            if (monthNum <= 9)
            {
                mn = 0 + monthNum.ToString();
            }
            else
            {
                mn = monthNum.ToString();
            }

            return mn;
        }

    }
}
