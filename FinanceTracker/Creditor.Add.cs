using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinanceTracker
{
    public partial class AddCreditorForm : Form
    {
        SQLiteConnection connection = MainWindow.dbConnection;

        public AddCreditorForm()
        {
            InitializeComponent();
        }

        //close the form
        private void FormClose(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddCreditorToDatabase(object sender, EventArgs e)
        {
            int testNum;
            string query = "";
            string originalName = MainWindow.creditorName; //needed for update query
            string successMessage = "";

            // check if the user is adding or updating
            if (addButton.Text == "Add")
            {
                // if creditor was flagged for deleted set them to active instead of adding a new creditor
                if(ISCreditorDeleted())
                {
                    query = "update creditors set username = @user, name = @cname, phone = @phone, accountnumber = @account, website = @web, daydue = @ddue, deleted = '0' where name = @cname and username = @user";
                }
                else
                {
                    query = "Insert into creditors (username, name, phone, accountnumber, website, daydue) values (@user, @cname, @phone, @account, @web, @ddue)";
                }
                successMessage = " successfully added!";
            }
            else if(addButton.Text == "Update")
            {
                query = "update creditors set username = @user, name = @cname, phone = @phone, accountnumber = @account, website = @web, daydue = @ddue, deleted = '0' where name = @oname and username = @user";
                successMessage = " successfully updated!";
            }

            //validate data
            if (nameTextBox.Text.Length < 1)
            {
                MessageBox.Show("You must enter a creditor name", "Missing Information!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dueTextBox.Text.Length < 1)
            {
                MessageBox.Show("You must enter a due date", "Missing Information!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!int.TryParse(dueTextBox.Text, out testNum))
            {
                MessageBox.Show("Due date must be a number between 1 and 28", "Invalid Data!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (Convert.ToInt32(dueTextBox.Text) < 1 || Convert.ToInt32(dueTextBox.Text) > 28)
            {
                MessageBox.Show("Due date must be a number between 1 and 28", "Invalid Data!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string username = MainWindow.username;
                string name = nameTextBox.Text;
                string accountNum = accountTextBox.Text;
                string phoneNum = phoneTextBox.Text;
                string website = websiteTextBox.Text;
                int daydue = Convert.ToInt32(dueTextBox.Text);
                SQLiteCommand cmd = new SQLiteCommand(query, MainWindow.dbConnection);
                cmd.Parameters.Add(new SQLiteParameter("user", username));
                cmd.Parameters.Add(new SQLiteParameter("cname", name));
                cmd.Parameters.Add(new SQLiteParameter("phone", phoneNum));
                cmd.Parameters.Add(new SQLiteParameter("account", accountNum));
                cmd.Parameters.Add(new SQLiteParameter("web", website));
                cmd.Parameters.Add(new SQLiteParameter("ddue", daydue));
                cmd.Parameters.Add(new SQLiteParameter("oname", originalName));
                connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show(name + successMessage);
                    this.Close();
                }
                catch (Exception addCreditorException)
                {
                    MessageBox.Show(addCreditorException.ToString());
                    connection.Close();
                }
            }

        }

        private bool ISCreditorDeleted()
        {
            bool answer = false;
            string name = nameTextBox.Text;
            string username = MainWindow.username;
            string query = "select deleted from creditors where name = @name and username = @username";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.Add(new SQLiteParameter("name", name));
            cmd.Parameters.Add(new SQLiteParameter("username", username));
            connection.Open();
            try
            {
                SQLiteDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    int num = Convert.ToInt32(reader["deleted"]);
                    if(num == 1)
                    {
                        answer = true;
                    }
                }
            }
            catch (Exception creditorDeletedException)
            {
                MessageBox.Show(creditorDeletedException.ToString());
                connection.Close();
            }


            connection.Close();
            return answer;
        }

    }
}
