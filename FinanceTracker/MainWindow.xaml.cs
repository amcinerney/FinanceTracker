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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SQLiteConnection dbConnection = new SQLiteConnection("Data Source=..\\..\\Finances.s3db;Version=3;");
        public static string username = "Aaron";

        public MainWindow()
        {
            InitializeComponent();
            QuerySavings(DateTime.Now.Year.ToString());
            PopulateCreditorGrid();
            PopulateExpensesGrid();
            PopulatePaymentGrid("All");
        }

    }
}
