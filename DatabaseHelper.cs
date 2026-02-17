using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public static class ConfigurationManager
{

    public static int GetYear(int cls, int sess)
    {
        string query = "SELECT iyear FROM Onlineopr..Configuration WHERE class = " + cls + " and sess = " + sess + " and isActive = 1";

        using (SqlConnection connection = new SqlConnection(DatabaseHelper.ConnectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            try
            {
                connection.Open();
                var result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return 0; // not active yet

                return int.Parse(result.ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

public static class DatabaseHelper
{
    public static readonly string ConnectionString = "Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASSWORD";

    public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
    {
        SqlConnection connection = new SqlConnection(ConnectionString);
        SqlCommand command = new SqlCommand(query, connection);
        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }

        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataTable dataTable = new DataTable();
        adapter.Fill(dataTable);
        return dataTable;
    }


    public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                connection.Open();
                return command.ExecuteScalar();
            }
        }
    }

    public static DateTime GetServerDate()
    {
        string query = "SELECT CAST(GETDATE() as date)";
        object result = ExecuteScalar(query);
        return Convert.ToDateTime(result);
    }

    public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery(); // Get affected rows
            connection.Close();

            return rowsAffected; // Return the count of inserted/updated/deleted rows
        }
    }

    internal static DataTable ExecuteQuery(string query, Dictionary<string, object> parameters)
    {
        throw new NotImplementedException();
    }
}
