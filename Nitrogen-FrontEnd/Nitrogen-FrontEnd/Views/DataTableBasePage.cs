using System.Windows.Controls;
using System.Data.SqlClient;
using Nitrogen_FrontEnd.Services;

namespace Nitrogen_FrontEnd.Views
{
    public class DataTableBasePage : Page
    {
        protected DatabaseService databaseService;
        protected SqlConnection sqlConnection;

        public DataTableBasePage()
        {
            databaseService = new DatabaseService("connection_string_here");
            sqlConnection = new SqlConnection("connection_string_here");
        }
    }
}
