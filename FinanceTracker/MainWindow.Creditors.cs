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

        public static string creditorName;

        // remove the creditor from the db based on the line chosen
        private void creditorRemove(object sender, RoutedEventArgs e)
        {
            var row = (CreditorInfo)creditorGrid.SelectedItem;
            string cname = row.creditorName;

            string query = "Update creditors set deleted = '1' where name = @cname and username = @user";
            SQLiteCommand deleteCmd = new SQLiteCommand(query, dbConnection);
            deleteCmd.Parameters.Add(new SQLiteParameter("cname", cname));
            deleteCmd.Parameters.Add(new SQLiteParameter("user", username));
            try
            {
                dbConnection.Open();
                deleteCmd.ExecuteNonQuery();
                dbConnection.Close();
            }
            catch (Exception deleteCreditorException)
            {
                MessageBox.Show(deleteCreditorException.ToString());
                dbConnection.Close();
            }         
            PopulateCreditorGrid();
        }

        // open the dialog to update the creditor information
        private void creditorUpdate(object sender, RoutedEventArgs e)
        {
            AddCreditorForm addCreditorForm = new AddCreditorForm();
            addCreditorForm.Text = "Update Creditor";
            addCreditorForm.addButton.Text = "Update";
            var row = (CreditorInfo)creditorGrid.SelectedItem;
            string cname = row.creditorName;
            string account = row.accountNumber;
            string phone = row.phone;
            string website = row.website;
            int due = row.dayDue;
            addCreditorForm.nameTextBox.Text = cname;
            addCreditorForm.accountTextBox.Text = account;
            addCreditorForm.phoneTextBox.Text = phone;
            addCreditorForm.websiteTextBox.Text = website;
            addCreditorForm.dueTextBox.Text = due.ToString();
            creditorName = cname; // needed for the update script
            addCreditorForm.ShowDialog();
            PopulateCreditorGrid();
        }


        // add a new creditor to the list
        private void addNewCreditor(object sender, RoutedEventArgs e)
        {
            AddCreditorForm addCreditorForm = new AddCreditorForm();
            addCreditorForm.ShowDialog();
            PopulateCreditorGrid();
        }

        public struct CreditorInfo
        {
            public string creditorName { get; set; }
            public string phone { get; set; }
            public string accountNumber { get; set; }
            public string website { get; set; }
            public int dayDue { get; set; }
        }

        public void PopulateCreditorGrid()
        {
            creditorGrid.Items.Clear();
            string creditorQuery = "select * from creditors where username = @un and deleted != '1' order by name asc";
            SQLiteCommand creditorCmd = new SQLiteCommand(creditorQuery, dbConnection);
            creditorCmd.Parameters.Add(new SQLiteParameter("un", username));
            try
            {
                dbConnection.Open();
                SQLiteDataReader creditorReader = creditorCmd.ExecuteReader();
                while (creditorReader.Read())
                {
                    string creditorName = Convert.ToString(creditorReader["name"]);
                    string phone = Convert.ToString(creditorReader["phone"]);
                    string accountNumber = Convert.ToString(creditorReader["accountnumber"]);
                    string website = Convert.ToString(creditorReader["website"]);
                    int dayDue = Convert.ToInt32(creditorReader["daydue"]);

                    creditorGrid.Items.Add(new CreditorInfo { creditorName = creditorName, phone = phone, accountNumber = accountNumber, website = website, dayDue = dayDue });
                }
                dbConnection.Close();
            }
            catch (Exception creditorQueryException)
            {
                MessageBox.Show(creditorQueryException.ToString());
                dbConnection.Close();
            }
        }

    }
}
