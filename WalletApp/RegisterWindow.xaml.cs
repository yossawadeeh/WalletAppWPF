using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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

namespace WalletApp
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Button_Upload_Image(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            { 
                Title = "Choose image.",
                Filter = "Image|*.png;*.jpg|All Files|*.*"
            };

            if(ofd.ShowDialog() == true)
            {
                userImage.Source = new BitmapImage(new Uri(ofd.FileName));
            }

        }

        private void Button_New_Register(object sender, RoutedEventArgs e)
        {
            try
            {
                SQLCore.Conn.Open();

                SQLCore.cmdText = "SELECT * FROM UserData WHERE email = '" + emailBox.Text.Trim() + "'";
                SQLCore.Cmd = new SqlCommand(SQLCore.cmdText, SQLCore.Conn);
                SQLCore.dr = SQLCore.Cmd.ExecuteReader();

                if (SQLCore.dr.HasRows)
                {
                    MessageBox.Show("Email has registed.");
                }
                else
                {
                    SQLCore.Conn.Close();
                    SQLCore.cmdText = "INSERT INTO UserData(username, email, hasspassword, firstname, lastname, image) VALUES" +
                        " ('" + usernameBox.Text.Trim() + "','" + emailBox.Text.Trim() + "','" + passwordBox.Password.Trim() + "','" + firstnameBox.Text.Trim() + "','" + lastnameBox.Text.Trim() + "',@image" + ")";
                    SQLCore.Conn.Open();
                    SQLCore.Cmd = new SqlCommand(SQLCore.cmdText, SQLCore.Conn);

                    // convert BitMap to Byte
                    byte[] byteImage;
                    BitmapSource bitmap = (BitmapSource)userImage.Source;
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));

                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        byteImage = ms.ToArray();
                    }

                    SQLCore.Cmd.Parameters.AddWithValue("@image", byteImage);

                    SQLCore.Cmd.ExecuteNonQuery();

                    MessageBox.Show("SUCCESS", "Message from system.", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                SQLCore.Conn.Close();
            }
            catch(Exception er)
            {
                MessageBox.Show("ERROR : " + er.Message, "Message from system.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
