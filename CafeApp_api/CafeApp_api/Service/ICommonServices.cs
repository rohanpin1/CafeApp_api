using Microsoft.Data.SqlClient;

namespace CafeApp_api.Service
{
    public interface ICommonServices
    {
        SqlCommand RunCommand(string query);
        //void OpenConnection();
        void CloseConnection();
    }
}
