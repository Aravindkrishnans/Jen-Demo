
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LabelApp.utility;
using LabelApp.Models;


namespace LabelApp.utility
{
    public class validation
    {
        public string UserName { get; set; }
        public bool ScanAllowed { get; set; }
        public bool ReScanAllowed { get; set; }
        public bool CSRAllowed { get; set; }
        public bool ReprintAllowed { get; set; }
        public bool IsAdmin { get; set; }
        public string Message { get; set; }
        public bool ValidUser { get; set; }
        public int category { get; set; }
        public string Assembly { get; set; }
        public void ValidateUser(string userName, string password)
        {

            utilityConnection dbConn = new utilityConnection();
            try
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["myconectionstring"].ConnectionString))
                {
                    con.Open();
                    string query = "select ScanAllowed,ReScanAllowed,CSRAllowed,ReprintAllowed,IsAdmin,category from userdetails where username='" + userName + "' and password='" + password + "' and IsActive=1";
                    using (var myCommand = new SqlCommand(query, con))
                    {
                        usermanagement usermanagement = new usermanagement();

                        SqlDataReader dr = myCommand.ExecuteReader();
                        if (dr.Read())
                        {
                            usermanagement.user = userName;
                            UserName = userName;
                            //ScanAllowed = Convert.ToBoolean(dr[0]);
                            //ReScanAllowed = Convert.ToBoolean(dr[1]);
                            //CSRAllowed = Convert.ToBoolean(dr[2]);
                            //ReprintAllowed = Convert.ToBoolean(dr[3]);
                            IsAdmin = Convert.ToBoolean(dr[4]);
                            //category = int.Parse(dr[5].ToString());
                            Message = "Authentication Successfull.";
                            ValidUser = true;
                        }
                        else
                        {
                            Message = "Invalid Username or Password.";
                            ValidUser = false;
                            UserName = userName;
                            string ErrorMsg = Message + " UserName:" + UserName + " Password:" + password;
                            usermanagement.user = "invaliduser";
                            UserName = userName;
                            dbConn.logCapture(ErrorMsg, "ValidateUser", "", "","","");
                        }
                    }
                    //ScanDetails msc = new ScanDetails();
                    //msc.Writelog(Message, UserName, "INFO", "ValidateUser");
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //UserName = userName;
                //ValidUser = false;
                //Message = "Authentication Failed." + ex.Message + ".Please contact administrator.";
                //ScanDetails msc = new ScanDetails();
                //msc.Writelog(Message, UserName, "ERROR", "ValidateUser");
                //dbConn.logCapture(Message, "ValidateUser", "", "");
            }

        }
    }
}