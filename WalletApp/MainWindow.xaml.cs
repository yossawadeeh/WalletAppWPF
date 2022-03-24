using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WalletApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window loginWin;
        public MainWindow(Window parent, string fullname, byte[] byteImage)
        {
            InitializeComponent();
            loginWin = parent;
            fullNameBox.Text = fullname;

            BitmapImage imgSource = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteImage, 0, byteImage.Length);
                ms.Seek(0, SeekOrigin.Begin);
                imgSource.BeginInit();
                imgSource.StreamSource = ms;
                imgSource.CacheOption = BitmapCacheOption.OnLoad;
                imgSource.EndInit();
                imgSource.Freeze();

                userImage.Source = imgSource;
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loginWin.Show();
        }
    }
}
