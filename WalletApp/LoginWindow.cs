using System;
using System.Data.SqlClient;
using System.Windows;

namespace WalletApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            SQLCore.Conn = new SqlConnection("Server=.\\sqlexpress;Database=WalletWPF;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            try
            {
                SQLCore.Conn.Open();
                SQLCore.cmdText = "SELECT Count(*) From UserData Where email = @email AND hasspassword = @pass";
                SQLCore.Cmd = new SqlCommand(SQLCore.cmdText, SQLCore.Conn);
                SQLCore.Cmd.Parameters.AddWithValue("@email", emailBox.Text.Trim());
                SQLCore.Cmd.Parameters.AddWithValue("@pass", passwordBox.Password.Trim());

                int numRow = (int)SQLCore.Cmd.ExecuteScalar();
                if(numRow > 0)
                {
                    SQLCore.cmdText = "SELECT * From UserData Where email = '" + emailBox.Text.Trim() + "'";
                    SQLCore.Cmd = new SqlCommand(SQLCore.cmdText, SQLCore.Conn);
                    SQLCore.dr = SQLCore.Cmd.ExecuteReader();

                    if(SQLCore.dr != null)
                    {
                        SQLCore.dr.Read();
                        //MessageBox.Show($"Name: {SQLCore.dr["firstname"]} Lastname: {SQLCore.dr["lastname"]}");
                        MainWindow mainWin = new MainWindow(this, $"{SQLCore.dr["firstname"]} {SQLCore.dr["lastname"]}", (byte[])SQLCore.dr["image"]);
                        mainWin.Show();
                        this.Hide();
                        SQLCore.dr.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email or password.", "Message from system.", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                SQLCore.Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR : " + ex.Message, "Message from system.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            RegisterWindow regWin = new RegisterWindow();
            regWin.Show();
        }
    }
}
