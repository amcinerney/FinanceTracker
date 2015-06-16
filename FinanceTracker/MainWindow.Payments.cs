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
    public partial class MainWindow
    {
        // used to populate the payments grid
        public class PaymentInfo
        {
            public string imagePath { get; set; }
            public string creditor { get; set; }
            public string dateDue { get; set; }
            public double amount { get; set; }
            public DateTime date { get; set; }
        }

        // populate the payments grid
        private void PopulatePaymentGrid(string display)
        {
            paymentDisplayCB.Text = display;
            DateTime currentDate = DateTime.Now;
            string currentMonth = currentDate.ToString("MM");
            string currentYear = currentDate.ToString("yyyy");
            string imagePath = "";
            List<PaymentInfo> paymentInfo = new List<PaymentInfo>();

            string query = "select name, daydue, dateLastPaid from creditors where username=@user";
            SQLiteCommand creditorCmd = new SQLiteCommand(query, dbConnection);
            creditorCmd.Parameters.Add(new SQLiteParameter("user", username));
            try
            {
                dbConnection.Open();
                SQLiteDataReader cReader = creditorCmd.ExecuteReader();
                while (cReader.Read())
                {
                    string creditor = Convert.ToString(cReader["name"]);
                    string dayDue = Convert.ToString(cReader["daydue"]);
                    DateTime lastPaid = Convert.ToDateTime(cReader["dateLastpaid"]);


                    DateTime dtDue = CalculateDueDate(currentDate, lastPaid, dayDue, currentYear);

                    //either show all bills or just the upcoming
                    if (display == "All")
                    {
                        imagePath = DetermineImage(currentDate, dtDue);
                        paymentInfo.Add(new PaymentInfo() { imagePath = imagePath, creditor = creditor, dateDue = dtDue.Date.ToString("MM/dd/yyyy"), date = DateTime.Now });
                        
                    }
                    else if (display == "Upcoming")
                    {
                        if ((dtDue - currentDate).TotalDays  < 30)
                        {
                            imagePath = DetermineImage(currentDate, dtDue);
                            paymentInfo.Add(new PaymentInfo() { imagePath = imagePath, creditor = creditor, dateDue = dtDue.Date.ToString("MM/dd/yyyy"), date = DateTime.Now });
                        }
                    }
                    paymentsGrid.ItemsSource = null;
                    paymentsGrid.Items.Clear();
                    paymentsGrid.ItemsSource = paymentInfo;
                }
                dbConnection.Close();
            }
            catch (Exception paymentQueryException)
            {
                MessageBox.Show(paymentQueryException.ToString());
                dbConnection.Close();
            }
            
        }

        private string DetermineImage(DateTime currentDate, DateTime dtDue)
        {
            if (currentDate > dtDue)
            {
                return System.IO.Path.GetFullPath("..\\..\\images\\late.png");
            }
            else if (dtDue <= currentDate.Date.AddDays(7))
            {
                return System.IO.Path.GetFullPath("..\\..\\images\\due.png");
            }
            else
            {
                return System.IO.Path.GetFullPath("..\\..\\images\\normal.png");
            }
        }

        private DateTime CalculateDueDate(DateTime currentDate, DateTime lastPaid, string dayDue, string currentYear)
        {
            string lastDueDate = Convert.ToString(lastPaid.Month + 1) + "/" + dayDue + "/" + Convert.ToString(lastPaid.Year);
            DateTime ldd = Convert.ToDateTime(lastDueDate);
            string dateDue = "";

            // if current date is less than the last due date then the next due date is 1 month away
            if (lastPaid.AddMonths(1) < ldd.AddDays(1))
            {
               dateDue = Convert.ToString(ldd.Month) + "/" + dayDue + "/" + currentYear;
            }
            else //else if the current date is greater than the last due date the due date is two months away
            {
                if (lastPaid.AddMonths(1) < ldd)
                {
                    dateDue = Convert.ToString(ldd.AddMonths(2).Month) + "/" + dayDue + "/" + lastPaid.AddMonths(2).ToString("yyyy");
                }
                else
                {
                    dateDue = Convert.ToString(ldd.AddMonths(1).Month) + "/" + dayDue + "/" + lastPaid.AddMonths(1).ToString("yyyy");
                }
            
            }

            DateTime dateToReturn = Convert.ToDateTime(dateDue);
            
           // if ()
            return dateToReturn;
        }

        //log the payment to the database
        private void LogPayment(object sender, RoutedEventArgs e)
        {
            string viewDropdown = paymentDisplayCB.Text;
            PaymentInfo row = ((FrameworkElement)sender).DataContext as PaymentInfo;
            string creditorName = row.creditor;
            double rowAmount = row.amount;
            DateTime rowDatePaid = row.date;

            if (rowAmount <= 0 || rowDatePaid == DateTime.MinValue)
            {
                MessageBox.Show("You must enter a payment amount, and date paid.", "Missing Information!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                dbConnection.Open();
                int creditorID = GetCreditorId(creditorName);
                InsertPayment(creditorID, rowDatePaid, rowAmount);
                dbConnection.Close();
                MessageBox.Show("Payment Logged");
                row.amount = 0;
                PopulatePaymentGrid(viewDropdown);
            }
        }

        //return the creditor ID
        private int GetCreditorId(string creditorName)
        {
            int creditorID = 0;
            string query = "select id from creditors where name=@cname and username=@user";
            SQLiteCommand creditIdCmd = new SQLiteCommand(query, dbConnection);
            creditIdCmd.Parameters.Add(new SQLiteParameter("cname", creditorName));
            creditIdCmd.Parameters.Add(new SQLiteParameter("user", username));

            try
            {
                
                SQLiteDataReader reader = creditIdCmd.ExecuteReader();
                while (reader.Read())
                {
                    creditorID = Convert.ToInt32(reader["id"]);
                }

            }
            catch (Exception getCreditorIdException)
            {
                MessageBox.Show(getCreditorIdException.ToString());
                dbConnection.Close();
            }
            return creditorID;
            
        }

        private void InsertPayment(int creditorId, DateTime datePaid, double amount)
        {
            // insert the new payment line into payments table
            string query = "insert into payments (creditorId, datePaid, amount) values (@creditorID, @datePaid, @amount)";
            SQLiteCommand insertPaymentCmd = new SQLiteCommand(query, dbConnection);
            insertPaymentCmd.Parameters.Add(new SQLiteParameter("creditorID", creditorId));
            insertPaymentCmd.Parameters.Add(new SQLiteParameter("datePaid", datePaid));
            insertPaymentCmd.Parameters.Add(new SQLiteParameter("amount", amount));

            // update the last paid date to the creditors table
            string updateLastPaid = "update creditors set dateLastPaid=@dateLastPaid where id=@creditorId";
            SQLiteCommand lastPaidCmd = new SQLiteCommand(updateLastPaid, dbConnection);
            lastPaidCmd.Parameters.Add(new SQLiteParameter("dateLastpaid", datePaid));
            lastPaidCmd.Parameters.Add(new SQLiteParameter("creditorId", creditorId));

            //get the last date paid to make sure you don't overwrite with older payments
            string getLastPaidQuery = "select dateLastpaid FROM creditors where id=@creditorId";
            SQLiteCommand getLastPaidCmd = new SQLiteCommand(getLastPaidQuery, dbConnection);
            getLastPaidCmd.Parameters.Add(new SQLiteParameter("creditorId", creditorId));

            try
            {
                insertPaymentCmd.ExecuteNonQuery();
                SQLiteDataReader reader = getLastPaidCmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime date = Convert.ToDateTime(reader["dateLastPaid"]);
                    if (datePaid >= date )
                    {
                        lastPaidCmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception insertPaymentException)
            {
                MessageBox.Show(insertPaymentException.ToString());
                dbConnection.Close();
            }

        }
        //update the payments grid based on the combobox selection
        private void PaymentDropdownSelected(object sender, EventArgs e)
        {
            string paymentCBSelected = paymentDisplayCB.Text;
            PopulatePaymentGrid(paymentCBSelected);
        }

        // user clicks the view payment history button
        private void PaymentHistoryButtonClick(object sender, RoutedEventArgs e)
        {
            string query = "select creditorId, datePaid, amount, name from payments INNER JOIN creditors on payments.creditorId = creditors.id WHERE username = @username ORDER BY name";
            SQLiteDataReader results = QueryPaymentsTable(query);

            if (results == null)
            {
                MessageBox.Show("No payment information found", "No Payments!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                PaymentsHistoryForm paymentsHistoryForm = new PaymentsHistoryForm(results);
                paymentsHistoryForm.Show();
            }
            dbConnection.Close();
            
        }

        public static SQLiteDataReader QueryPaymentsTable(string query)
        {
            SQLiteDataReader reader = null;
            dbConnection.Open();
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(query, dbConnection);
                cmd.Parameters.Add(new SQLiteParameter("username", username));
                reader = cmd.ExecuteReader();
            }
            catch (Exception paymentsTableQueryException)
            {
                MessageBox.Show(paymentsTableQueryException.ToString());
                dbConnection.Close();
            }
            return reader;
        }
    }
}
