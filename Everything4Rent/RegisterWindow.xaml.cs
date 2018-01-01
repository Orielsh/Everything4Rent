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
using System.Data;
using System.Data.OleDb;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Everything4Rent
{
    public partial class RegisterWindow : Window
    {
        string m_tmp;
        OleDbConnection m_connection;
        public RegisterWindow()
        {
            InitializeComponent();
            initialGenderBox();
            initialBirthBox();
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            runDB();
            try
            {
                if (box_userName.Text == "")
                    throw new Exception("Enter UserName");

                if (isRecordExist("userName", box_userName.Text))
                    throw new Exception("UserName already exist, choose another");

                if (box_firstName.Text == "")
                    throw new Exception("Enter First Name");
                m_tmp = box_firstName.Text.Replace(" ", string.Empty);

                if (box_lastName.Text == "")
                    throw new Exception("Enter Last Name");
                m_tmp = box_lastName.Text.Replace(" ", string.Empty);

                if (string.IsNullOrEmpty(box_password.Password) || string.IsNullOrEmpty(box_confirmPassword.Password))
                    throw new Exception("Enter Password and Confirm it");

                if (box_confirmPassword.Password != box_confirmPassword.Password)
                    throw new Exception("Password and Confirm Password does not match");

                if (box_email.Text == "")
                    throw new Exception("Enter Email Adress");
                if (isRecordExist("mailAddress", box_email.Text))
                    throw new Exception("Email already exist, choose another");

                if (box_gender.SelectedItem == null)
                    throw new Exception("Choose Gender");

                if (choose_day.SelectedItem == null || choose_month.SelectedItem == null || choose_year.SelectedItem == null)
                    throw new Exception("Enter Full Birth Date");

                string to = box_email.Text;
                string from1 = "Everything4Rent@gmail.com";
                MailMessage message = new MailMessage();

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("everythingforent@gmail.com", "ofiryakov");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                message.From = new MailAddress(from1, "Everything4Rent");
                message.To.Add(new MailAddress(to));
                message.Subject = "Welcome To Everything4Rent";
                message.Body = string.Format("Thank you for your regisetration, now feel free to give us your money $");
                try
                {
                    client.Send(message);
                }
                catch
                {
                    throw new Exception("Invalid Email");

                }

                m_connection.Open();
                string date = choose_day.SelectedItem.ToString() + "/" + choose_month.SelectedItem.ToString() + "/" + choose_year.SelectedItem.ToString();

                //string strSql = "INSERT INTO Users([userName],[firstName],[lastName],[Password],[gender],[mailAddress],[birthDate],[image])values(?,?,?,?,?,?,?,?)";
                string insertSQL = "INSERT INTO Users([userName],[firstName],[lastName],[Password],[gender],[mailAddress],[birthDate])values(?,?,?,?,?,?,?)";

                using (OleDbCommand command = new OleDbCommand(insertSQL, m_connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("[userName]", box_userName.Text);
                    command.Parameters.AddWithValue("[firstName]", box_firstName.Text);
                    command.Parameters.AddWithValue("[lastName]", box_lastName.Text);
                    command.Parameters.AddWithValue("[Password]", box_password.Password);
                    command.Parameters.AddWithValue("[gender]", box_gender.SelectedItem.ToString());
                    command.Parameters.AddWithValue("[mailAddress]", box_email.Text);
                    command.Parameters.AddWithValue("[birthDate]", date);
                    //command.Parameters.AddWithValue("[image]", image);

                    command.ExecuteNonQuery();
                }
                m_connection.Close();

                MessageBox.Show("Successfully Signed!");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            m_connection.Close();
        }

        private void Browse_Image_Click(object sender, EventArgs e)
        {
            TextBox textbox1 = new TextBox();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == true)
            {
                textbox1.Text = open.FileName;

            }
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
        private bool isRecordExist(string fieldName, string inputString)
        {
            runDB();
            OleDbCommand command = new OleDbCommand();
            OleDbDataReader reader = null;
            m_connection.Open();
            command = m_connection.CreateCommand();
            command.CommandText = "select " + fieldName + " from Users Where " + fieldName + " = " + "'" + inputString + "'";

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

        private void initialGenderBox()
        {
            box_gender.Items.Add("Male");
            box_gender.Items.Add("Female");
        }

        private void initialBirthBox()
        {
            for (int i = 1; i <= 31; i++)
                choose_day.Items.Add(i.ToString());

            for (int i = 1; i <= 12; i++)
                choose_month.Items.Add(i.ToString());

            for (int i = 2000; i > 1948; i--)
                choose_year.Items.Add(i.ToString());

        }

        private void choose_day_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
