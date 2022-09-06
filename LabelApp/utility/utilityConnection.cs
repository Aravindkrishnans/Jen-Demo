using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LabelApp.Models;

namespace LabelApp.utility
{
    public class utilityConnection
    {
        private SqlDataAdapter myAdapter;
        private SqlConnection conn;

        public utilityConnection()
        {
            myAdapter = new SqlDataAdapter();
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myconectionstring"].ConnectionString);
        }
        private SqlConnection openConnection()
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        public DataTable executeSP(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();
            try
            {
                myCommand.Connection = openConnection();
                myCommand.CommandText = _query;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddRange(sqlParameter);
                myAdapter.SelectCommand = myCommand;
                myAdapter.Fill(ds);
                dataTable = ds.Tables[0];
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return dataTable;
        }

        public void logCapture(string LogMessage, string LogLabelName, string order, string item, string orderlineno, string lpn)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["myconectionstring"].ConnectionString))
            {
                usermanagement usermanagement = new usermanagement();
                string user = usermanagement.user.ToString();
                con.Open();
                using (var myCommand = new SqlCommand("INSERT INTO [wmwhse3].[PIDSA_LABEL_APP_AUDIT_LOG]([LOG_NAME],[LABEL_NAME],[LOG_BY],[LOG_TIME],[ORDERS],[ITEM],LPN,orderlineno,Addby) VALUES('" + LogMessage + "', '" + LogLabelName + "', 'LabelApp', GETDATE(),'" + order + "','" + item + "','" + lpn + "','" + orderlineno + "','" + user + "')", con))
                {
                    SqlDataReader dr = myCommand.ExecuteReader();
                    if (dr.Read())
                    {

                    }
                }

            }
        }

        public string updateQuery(string _query)
        {
            string connection = ConfigurationManager.AppSettings["DBconnection"];

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand(_query, conn);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return "Success";
        }
    }
}