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
using System.Data.OleDb;


namespace Everything4Rent
{

    public partial class MainWindow : Window
    {
        OleDbConnection m_connection;
        public MainWindow()
        {
            InitializeComponent();

        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (userExist())
            {
                string password = box_password.Password;
                OleDbCommand command = new OleDbCommand("select * from Users Where [userName] = " + "'" + box_userName.Text + "'", m_connection);
                OleDbDataReader reader = null;
                m_connection.Open();
                reader = command.ExecuteReader();
                reader.Read();
             if (reader["password"].ToString() == password)
             {
                    UsersWindow uw = new UsersWindow(box_userName.Text);
                    uw.ShowDialog();
                }
                else
                 MessageBox.Show("Wrong Password");
                m_connection.Close();
            }
            else
                MessageBox.Show("User name does not signed up yet, please sign up");
        }

        private void Sign_Up_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        private bool userExist()
        {
            openConnectionToDataBase();
            OleDbCommand command = new OleDbCommand();
            OleDbDataReader reader = null;
            m_connection.Open();
            command = m_connection.CreateCommand();
            string username = box_userName.Text;
            command.CommandText = "select [userName] from Users Where [userName] = " + "'" + username + "'";

            reader = command.ExecuteReader();
            reader.Read();
            if (!reader.HasRows)
            {
                m_connection.Close();
                return false;
            }
            m_connection.Close();
            return true;
        }

        private void openConnectionToDataBase()
        {
            try
            {
                m_connection = new OleDbConnection();
                m_connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.accdb;";
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
    }
}
