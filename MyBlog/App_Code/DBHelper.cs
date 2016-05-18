using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// DBHelper 的摘要说明
/// </summary>
public class DBHelper
{
    private static String ConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    public static int NonQuery(String ComStr)
    {
        SqlConnection con = new SqlConnection(ConnectString);
        try {
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandText = ComStr;
            return command.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            return -1;
        }
        finally
        {
            con.Close();
        }
    }
    public static DataSet DataSet(String ComStr)
    {
        SqlConnection con = new SqlConnection(ConnectString);
        try
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(ComStr, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
        }
    }
}