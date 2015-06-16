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
        public List<string> years = new List<string>();

        //populate the savings year dropdown with the different years
        public void PopulateSavingsDropdown(List<string> years)
        {
            savingsYearComboBox.Items.Clear();
            savingsYearComboBox.Items.Add("All");
            List<string> y = new List<string>();
            y = years.Distinct().ToList();
            foreach (string item in y)
            {
                savingsYearComboBox.Items.Add(item);
            }
        }

        public struct SavingsData
        {
            public string amount { get; set; }
            public string date { get; set; }
            public string difference { get; set; }
        }

        //populate the savings table with default values
        public void QuerySavings(string whatToShow)
        {
            savingsGrid.Items.Clear();
            double lastMonth = 0;
            string query = "select * from savings where username=@un order by date";

            SQLiteCommand cmd = new SQLiteCommand(query, dbConnection);
            cmd.Parameters.Add(new SQLiteParameter("un", username));
            dbConnection.Open();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                double amount = Convert.ToDouble(reader["amount"]);
                DateTime date = Convert.ToDateTime(reader["date"]);
                double difference = amount - lastMonth;
                if (whatToShow == "All")
                {
                    savingsGrid.Items.Add(new SavingsData { amount = amount.ToString("C2"), date = date.ToString("yyyy/MM/dd"), difference = Math.Round(difference, 2).ToString("C2") });
                }
                else
                {
                    int ed = Convert.ToInt32(whatToShow) + 1;
                    string endDate = Convert.ToString(ed) + "-01-01";
                    string startDate = whatToShow + "-01-01";
                    if (date >= Convert.ToDateTime(startDate) && date < Convert.ToDateTime(endDate))
                    {
                        savingsGrid.Items.Add(new SavingsData { amount = amount.ToString("C2"), date = date.ToString("yyyy/MM/dd"), difference = Math.Round(difference, 2).ToString("C2") });
                    }
                }

                lastMonth = amount;
                years.Add(date.ToString("yyyy"));
            }
            dbConnection.Close();
            PopulateSavingsDropdown(years);
            savingsYearComboBox.Text = whatToShow;
        }

        // when the year dropdown is selected change which values are displayed in the datagrid
        private void savingsYearSelected(object sender, EventArgs e)
        {
            ComboBox selectedYear = sender as ComboBox;
            QuerySavings(selectedYear.Text);
        }

        // user submits their monthly savings amount
        private void savingsMonthlySubmit(object sender, RoutedEventArgs e)
        {
            double num;
            if (savingsAmountTextBox.Text.Length < 1 || savingsMonthCB.Text.Length < 1)
            {
                MessageBox.Show("Please enter a month and amount.", "Missing Information!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!double.TryParse(savingsAmountTextBox.Text, out num))
            {
                MessageBox.Show("Incorrect format on savings value.\nDo not include currency symbols or letters", "Invalid Data!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string month = savingsMonthCB.Text;
                double amount = Convert.ToDouble(savingsAmountTextBox.Text);
                addSavingsToDB(month, amount);
            }
        }

        // update the database with the users montly savings
        private void addSavingsToDB(string month, double amount)
        {
            int monthNum = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;
            string mn;
            string year = DateTime.Now.Year.ToString();

            // month number needs to be formatted with a leading 0 for months less than 10
            if (monthNum <= 9)
            {
                mn = 0 + monthNum.ToString();
            }
            else
            {
                mn = monthNum.ToString();
            }

            string date = year + "-" + mn + "-01";

            string testQuery = "select * from savings where date=@dt and username=@user";
            SQLiteCommand cmd = new SQLiteCommand(testQuery, dbConnection);
            cmd.Parameters.Add(new SQLiteParameter("user", username));
            cmd.Parameters.Add(new SQLiteParameter("dt", date));
            dbConnection.Open();
            SQLiteDataReader testReader = cmd.ExecuteReader();

            if (testReader.HasRows == true)
            {
                MessageBox.Show("You already have savings logged for " + month, "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
                dbConnection.Close();
            }
            else
            {
                string query = "insert into savings (amount, date, username) values (@amt, @dt, @user)";
                SQLiteCommand querycmd = new SQLiteCommand(query, dbConnection);
                querycmd.Parameters.Add(new SQLiteParameter("user", username));
                querycmd.Parameters.Add(new SQLiteParameter("dt", date));
                querycmd.Parameters.Add(new SQLiteParameter("amt", amount));
                try
                {
                    querycmd.ExecuteNonQuery();
                    dbConnection.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    dbConnection.Close();
                }
                QuerySavings(year);
                savingsAmountTextBox.Text = "";
                savingsYearComboBox.SelectedValue = year;
            }

        }

        // see a list of expenses for the month
        private void ViewMontlyExpenses(object sender, RoutedEventArgs e)
        {
            var row = (SavingsData)savingsGrid.SelectedItem;
            DateTime date = Convert.ToDateTime(row.date);
            string monthlyExpenses = "";
            double total = 0;

            string query = "select * from expenses where username=@username and date >= date(@date) and date < date(@date,'+1 month')";
            SQLiteCommand monthlyExpensesCmd = new SQLiteCommand(query, dbConnection);
            monthlyExpensesCmd.Parameters.Add(new SQLiteParameter("username", username));
            monthlyExpensesCmd.Parameters.Add(new SQLiteParameter("date", date));
            try
            {
                dbConnection.Open();
                SQLiteDataReader reader = monthlyExpensesCmd.ExecuteReader();
                while (reader.Read())
                {
                    monthlyExpenses += "\n" + Convert.ToString(reader["name"]) + ": " + Convert.ToString(reader["amount"]);
                    total += Convert.ToDouble(reader["amount"]);
                }
                monthlyExpenses += "\n---------------\nTotal: " + total.ToString("C2");
                MessageBox.Show(monthlyExpenses, "Expenses for " + date.ToString("MMMM-yyyy"));
                dbConnection.Close();
            }
            catch (Exception monthlyExpenseQueryException)
            {
                MessageBox.Show(monthlyExpenseQueryException.ToString());
                dbConnection.Close();
            }
        }
    }
}
