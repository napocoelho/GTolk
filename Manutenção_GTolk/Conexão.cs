

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConnectionManagerDll;



public static class Conexão
{
    private static ThreadSafeConnection get = null;
    public static ThreadSafeConnection Get { get { return Conexão.get; } private set { Conexão.get = value; } }

    public static bool IsConnected
    {
        get
        {
            bool retorno = false;

            try
            {
                retorno = (ConnectionManager.GetConnection != null) && (ConnectionManager.GetConnection.State == System.Data.ConnectionState.Open);
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }
    }

    public static void Connect(string server, string databaseName, int timeoutInSeconds, string user, string password, string appName)
    {
        string connectionString = "Initial Catalog={1};" +
                                          "Data Source={0};" +
                                          "User ID={3};" +
                                          "Password={4};" +
                                          "Connect Timeout={2};" +
                                          "Application Name='{5}'";

        connectionString = string.Format(connectionString, server, databaseName, timeoutInSeconds, user, password, appName);

        ConnectionManagerDll.ConnectionManager.CreateInstance(connectionString);
        Conexão.Get = ConnectionManagerDll.ConnectionManager.GetConnection;
    }

    public static void Reconnect()
    {
        ConnectionManager.Reconnect();
    }

    public static DateTime GetServerDateTime()
    {
        object value = Conexão.Get.ExecuteScalar("SELECT GetDate()");
        return DateTime.Parse(value.ToString());
    }
}