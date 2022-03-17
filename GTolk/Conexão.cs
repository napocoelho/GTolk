using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConnectionManagerDll;



public static class Conexão
{
    private static string lastConnectionString;
    private static ThreadSafeConnection Get { get { return ConnectionManager.GetConnection; } }

    private static void ExecuteActionWithReconnect(Action executable)
    {
        Conexão.ExecuteFuncWithReconnect<object>(() => { executable(); return null; });
    }

    private static T ExecuteFuncWithReconnect<T>(Func<T> executable)
    {
        T value = default(T);
        bool tryToReconnect = false;
        int timeToWait = 1;

        do
        {
            try
            {
                if (tryToReconnect)
                {
                    Conexão.Reconnect();
                }

                value = executable();

                tryToReconnect = false;
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                string msg = sqlEx.Message;

                if (sqlEx.Number == 121 || !Conexão.IsConnected)
                {
                    try
                    {
                        tryToReconnect = true;
                        System.Threading.Thread.Sleep(timeToWait);
                        Conexão.Reconnect();
                        timeToWait = 500;
                    }
                    catch (Exception fail) { msg = fail.Message; }
                }
                else
                {
                    throw sqlEx;
                }

            }
            catch (Exception ex)
            {
                if (!Conexão.IsConnected)
                {
                    try
                    {
                        tryToReconnect = true;
                        System.Threading.Thread.Sleep(timeToWait); // espera um pouco e tenta se conectar novamente;
                        Conexão.Reconnect();
                        timeToWait = 500;
                    }
                    catch (Exception fail) { string msg = fail.Message; }
                }
                else
                {
                    throw ex;
                }
            }
        } while (tryToReconnect);

        return value;
    }

    public static object ExecuteScalar(string sql)
    {
        return Conexão.ExecuteFuncWithReconnect<object>(() => { return Conexão.Get.ExecuteScalar(sql); });
    }

    public static int? ExecuteScalarAndGetLastInsertedId(string sql)
    {
        return Conexão.ExecuteFuncWithReconnect<int?>(() => { return Conexão.Get.ExecuteScalarAndGetLastInsertedId(sql); });
    }

    public static int ExecuteNonQuery(string sql)
    {
        return Conexão.ExecuteFuncWithReconnect<int>(() => { return Conexão.Get.ExecuteNonQuery(sql); });
    }

    public static System.Data.DataTable ExecuteDataTable(string sql)
    {
        return Conexão.ExecuteFuncWithReconnect<System.Data.DataTable>(() => { return Conexão.Get.ExecuteDataTable(sql); });
    }

    public static void BeginTransaction()
    {
        Conexão.ExecuteActionWithReconnect(() => { Conexão.Get.BeginTransaction(); });
    }

    public static void CommitTransaction()
    {
        Conexão.ExecuteActionWithReconnect(() => { Conexão.Get.CommitTransaction(); });
    }

    public static void TryCommitTransaction()
    {
        Conexão.ExecuteActionWithReconnect(() => { Conexão.Get.TryCommitTransaction(); });
    }

    

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
        lastConnectionString = "Initial Catalog={1};" +
                                          "Data Source={0};" +
                                          "User ID={3};" +
                                          "Password={4};" +
                                          "Connect Timeout={2};" +
                                          "Application Name='{5}'";

        lastConnectionString = string.Format(lastConnectionString, server, databaseName, timeoutInSeconds, user, password, appName);

        ConnectionManagerDll.ConnectionManager.CreateInstance(lastConnectionString);
        //Conexão.Get = ConnectionManagerDll.ConnectionManager.GetConnection;
    }

    public static void Reconnect()
    {
        ConnectionManagerDll.ConnectionManager.CreateInstance(lastConnectionString);
        //ConnectionManager.Reconnect();
    }

    public static DateTime GetServerDateTime()
    {
        object value = Conexão.ExecuteFuncWithReconnect<object>(() => { return Conexão.ExecuteScalar("SELECT GetDate()"); });
        return DateTime.Parse(value.ToString());
    }
}