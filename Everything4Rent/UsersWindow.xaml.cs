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
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace Everything4Rent
{
    /// <summary>
    /// Interaction logic for UsersWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        OleDbConnection m_connection;
        string _id;
        string _name;
        string _price;
        string _location;
        List<String> m_dID = new List<string>();

        public UsersWindow(string userName)
        {
            InitializeComponent();
            runDB();
            m_connection.Open();
            OleDbDataReader reader = null;
            OleDbCommand cmd = new OleDbCommand("select * from Products", m_connection);
            reader = cmd.ExecuteReader();
            string prod_id;
            while (reader.Read())
            {
                prod_id = (reader["productId"].ToString());
                m_dID.Add(prod_id);
                choose_id_read.Items.Add(prod_id);
                choose_id_update.Items.Add(prod_id);
                choose_id_delete.Items.Add(prod_id);
            }
            m_connection.Close();

        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (id_from_create.Text == "")
                MessageBox.Show("Enter product ID");
            _id = id_from_create.Text;
            choose_id_read.Items.Add(_id);
            choose_id_update.Items.Add(_id);
            choose_id_delete.Items.Add(_id);


            if (name_from_create.Text == "")
                MessageBox.Show("Enter product name");
            _name = name_from_create.Text;

            if (price_from_create.Text == "")
                MessageBox.Show("Enter price");
            _price = price_from_create.Text;

            if (location_from_create.Text == "")
                MessageBox.Show("Enter location");
            _location = location_from_create.Text;


            m_connection.Open();
            string insertSQL = "INSERT INTO Products([productID],[productName],[productPrice],[productLocation])values(?,?,?,?)";

            using (OleDbCommand command = new OleDbCommand(insertSQL, m_connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("[productID]", _id);
                command.Parameters.AddWithValue("[productName]", _name);
                command.Parameters.AddWithValue("[productPrice]", _price);
                command.Parameters.AddWithValue("[productLocation]", _location);

                command.ExecuteNonQuery();
            }
            MessageBox.Show("Product Successfully Created!");

            m_connection.Close();

        }
        private void Read_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Product Successfully Read!");

        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Product Successfully Updated!");
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //choose_id_read.Items.Remove(_id);
            //choose_id_update.Items.Remove(_id);
            //choose_id_delete.Items.Remove(_id);
            //m_dID.Remove(_id);
            MessageBox.Show("Product Successfully Deleted!");
        }

        private void choose_ID_Update(object sender, SelectionChangedEventArgs e)
        {

        }
        private void choose_ID_Read(object sender, SelectionChangedEventArgs e)
        {

        }
        private void choose_ID_Delete(object sender, SelectionChangedEventArgs e)
        {
        }


        private void runDB()
        {
            try
            {
                m_connection = new OleDbConnection();
                m_connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.accdb;";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }
}
