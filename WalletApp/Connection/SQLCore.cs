using System.Data.SqlClient;

namespace WalletApp
{
    public class SQLCore
    {
        public static SqlConnection Conn { get; set; }
        public static SqlCommand Cmd { get; set; }
        public static SqlDataReader dr { get; set; }
        public static string cmdText { get; set; }
    }
}
