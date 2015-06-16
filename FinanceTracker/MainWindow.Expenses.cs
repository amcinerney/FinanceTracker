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
        double expenseTotal;

        public struct ExpensesInfo
        {
            public string name { get; set; }
            public string date { get; set; }
            public string amount { get; set; }
        }

        private void PopulateExpensesGrid()
        {
            expensesGrid.Items.Clear();
            expenseTotal = 0;
            string currentMonth = DateTime.Now.Month.ToString();
            string currentYear = DateTime.Now.Year.ToString();
            if (Convert.ToInt32(currentMonth) < 10)
            {
                currentMonth = "0" + currentMonth;
            }
            string monthBegin = currentYear + "-" + currentMonth + "-01";
            string query = "select * from expenses where username = @user and date >= date(@beginDate)";
            SQLiteCommand expensesCmd = new SQLiteCommand(query, dbConnection);
            expensesCmd.Parameters.Add(new SQLiteParameter("user", username));
            expensesCmd.Parameters.Add(new SQLiteParameter("beginDate", monthBegin));
            try
            {
                dbConnection.Open();
                SQLiteDataReader expensesReader = expensesCmd.ExecuteReader();
                while (expensesReader.Read())
                {
                    string name = Convert.ToString(expensesReader["name"]);
                    DateTime date = Convert.ToDateTime(expensesReader["date"]);
                    double amount = Convert.ToDouble(expensesReader["amount"]);
                    expensesGrid.Items.Add(new ExpensesInfo { name = name, date = date.ToString("MMMM/dd/yyyy"), amount = amount.ToString("C2") });
                    expenseTotal += amount;
                }
                dbConnection.Close();
                expenseTotalLabel.Content = expenseTotal.ToString("C2");
            }
            catch (Exception expensesPopulateDBException)
            {
                MessageBox.Show(expensesPopulateDBException.ToString());
                dbConnection.Close();
            }
        }

        private void AddNewExpense(object sender, RoutedEventArgs e)
        {
            double amountTest;
            var amount = expensesDollarTextBox.Text;
            var name = expensesNameTextBox.Text;
            var date = expensesDatePicker.Text;
            if (amount.Length < 1 || name.Length < 1 || date.Length < 1)
            {
                MessageBox.Show("You must enter a dollar amount, expense name, and date.", "Missing Information!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Double.TryParse(amount, out amountTest))
            {
                MessageBox.Show("Dollar amounts must be numbers only", "Invalid Data!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string query = "insert into expenses (username, name, date, amount) values (@user, @name, @date, @amount)";
                SQLiteCommand addExCmd = new SQLiteCommand(query, dbConnection);
                addExCmd.Parameters.Add(new SQLiteParameter("user", username));
                addExCmd.Parameters.Add(new SQLiteParameter("name", name));
                addExCmd.Parameters.Add(new SQLiteParameter("date", Convert.ToDateTime(date)));
                addExCmd.Parameters.Add(new SQLiteParameter("amount", Convert.ToDouble(amount)));
                try
                {
                    dbConnection.Open();
                    addExCmd.ExecuteNonQuery();
                    dbConnection.Close();
                    PopulateExpensesGrid();
                    expensesDollarTextBox.Text = "";
                    expensesNameTextBox.Text = "";
                }
                catch (Exception addExpenseException)
                {
                    MessageBox.Show(addExpenseException.ToString());
                    dbConnection.Close();
                }
            }
        }

    }
}
