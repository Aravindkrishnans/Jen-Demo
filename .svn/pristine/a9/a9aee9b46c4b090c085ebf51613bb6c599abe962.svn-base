using LabelApp.utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace LabelApp.pidsalabelGenerator
{
    public class labelGenerator
    {
        utilityConnection dbConn = new utilityConnection();
        //labelGenerator Labelgenrator = new labelGenerator();
        string[] actualCountry = { "China", "Thailand", "Indonesia", "Japan", "Malaysia","North Korea",
            "South Korea", "Singapore","Taiwan","Italy","Vietnam","Germany","Philippines","Not Applicable" };

        string[] Countrycode = { "CN", "TH", "ID", "JP", "MY", "KP", "KR", "SG", "TW", "IT", "VN", "DE", "PH", "NA" };
       static int methodCallingCount = 0;

        public string digikey(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer,string lpn,string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            string connection = ConfigurationManager.AppSettings["DBconnection"];
                            SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            DataSet dsDet1 = new DataSet();
                            da1.Fill(dsDet1);
                            DataTable dt1 = dsDet1.Tables[0];
                            DataRow row1 = dt1.Rows[0];
                            int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            string Tlot = inTlot.ToString("D6");
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string datecodetemp2 = "";
                                if (datecode == "")
                                {
                                    DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                    datecodetemp2 = dAteCodeFormarte.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                    datecodetemp2 = dAteCodeFormarte.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                                }
                                //int j = i + 1;
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""C_COMPANY"">" + row["label_name"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""PACK"">4S" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""PACK1"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">1P" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR1"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""PURCHASE"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""PURCHASE1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""CUSTOMER"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""CUSTOMER1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + Tlot + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + Tlot + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE1"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE"">6D" + datecodetemp2 + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">13q" + row["eachBox"].ToString() + " / " + row["totalBox"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT1"">" + row["eachBox"].ToString() + " M " + row["totalBox"].ToString() + @"</variable>";
                                }
                                else
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">13q" + i + " / " + templabel + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT1"">" + i + " M " + templabel + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 K" + row["PURCHASEORDERDOCUMENT"].ToString() + " 4K" + row["saporderlineno"].ToString() + " P" + row["customer"].ToString() + " 1P" + row["mfr"].ToString() + " Q" + qty + " 11k" + row["KITORDERASN"].ToString()  + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                    
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                                //dbConn.updateQuery(TlotUpdate);
                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                                //labeLabelgenrator.xmlbuilder(mxml, labelname, i);

                            }
                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }



                    else if (Type == "Inner")
                    {
                        
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        LabelsOName.Add("DIGI-KEY (IDAC)", "digikey-inner label");
                        LabelsOName.Add("ARROW ELECTRONICS INC. (IDAC)", "Arrow_inner_label");
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                            
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">1P" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR1"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""CUSTOMER"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""CUSTOMER1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE"">D" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE1"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                              
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                    
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string arrow(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer,string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {

                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[Arrow]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""PACK"">4s" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""PACK1"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">1p" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR1"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""CUSTP"">p" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""CUSTP1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                if (datecode == "")
                                {
                                    DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                    datecodetemp2 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                    datecodetemp2 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }


                    else if (Type == "Inner")
                    {
                      

                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                      
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                            
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3s" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""cuspo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""cuspo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""CUSTP"">p" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""CUSTP1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE"">" + datecodetemp + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string general(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn,string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";
                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {

                        
                            if (label != "")
                            labelname = label;
                        else
                        {
                            //do nothing;
                        }





                        if (crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }

                  
                        //string datecode2 = "";
                        //if (datecode == "")
                        //{
                        //    DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                        //    datecode2 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        //}
                        //else
                        //{
                        //    DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                        //    datecode2 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        //}

                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = "";
                            if (labelname == "avnet")
                            {
                                if ((wgt == "") && (crossdock != "1"))
                                {
                                    return "inalidweight";
                                }
                                mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""way"">" + row["TM"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""way1"">" + row["TM"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">11K" + row["KITORDERASN"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["KITORDERASN"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">" + No_of_Labels + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                            }
                            else
                            {
                                mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""add1"">" + row["C_COMPANY"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""add2"">" + row["C_ADDRESS1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""add3"">" + row["C_ADDRESS2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""add4"">" + row["C_STATE"].ToString() + row["C_CITY"].ToString() + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                            }

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                        //dbConn.updateQuery(printUpdate);
                        string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                        //dbConn.updateQuery(cooPickdetail);
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }


        public string TTi(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {

                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);

                    
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }

                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        if ((wgt == "") && (crossdock != "1"))
                        {
                            return "inalidweight";
                        }
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;

                            if(crossdock!="")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string dsql = "select lot from panLabels where c_company = 'TTI INC. (IDAC)'";
                            string connection = ConfigurationManager.AppSettings["DBconnection"];
                            SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            DataSet dsDet1 = new DataSet();
                            da1.Fill(dsDet1);
                            DataTable dt1 = dsDet1.Tables[0];
                            DataRow row1 = dt1.Rows[0];
                            int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            string Tlot = inTlot.ToString("D6");

                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                          
                            for (int i = 1; i <= templabel; i++)
                            {
                                if(crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                if (mlot == "")
                                {
                                    mlot = row["lot"].ToString();
                                }
                                string datecode2 = "";
                                if (datecode == "")
                                {
                                    DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                    int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                    string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                    datecode2 = date11 + tempdate1.ToString("D2");
                                }
                                else
                                {
                                    DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                    int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                    string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                    datecode2 = date11 + tempdate1.ToString("D2");
                                }
                             

                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""waybill"">" + row["TM"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">11KTT00" + row["KITORDERASN"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">TT00" + row["KITORDERASN"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""lineno"">4K" + row["saporderlineno"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""lineno1"">" + row["saporderlineno"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                //if(crossdock != "")
                                //{
                                //    mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + row["eachBox"].ToString() + " of " + row["totalBox"].ToString() + @"</variable>";
                                //}
                                //else
                             

                                    mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                            
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 K" + row["purchase"].ToString() + " 4K" + row["saporderlineno"].ToString() + " P" + row["customer"].ToString() + " 1P" + row["mfr"].ToString() + " 11k" + row["KITORDERASN"].ToString() + " Q" + qty + "4L" + COO1 + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                            string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'TTI INC. (IDAC)'";
                            //dbConn.updateQuery(TlotUpdate);
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                        }


                    }


                    else if (Type == "Inner")
                    {


                        foreach (DataRow row in dataTable.Rows)
                        {
                            
                            string datecode2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = date11 + tempdate1.ToString("D2");
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = date11 + tempdate1.ToString("D2"); 
                            }
                            string COO1 = "";
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["lot"].ToString();
                            }
                            string dsql = "select lot from panLabels where c_company = 'TTI INC. (IDAC)'";
                            string connection = ConfigurationManager.AppSettings["DBconnection"];
                            SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            DataSet dsDet1 = new DataSet();
                            da1.Fill(dsDet1);
                            DataTable dt1 = dsDet1.Tables[0];
                            DataRow row1 = dt1.Rows[0];
                            int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            string Tlot = inTlot.ToString("D6");
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }
                            }

                            for (int i = 1; i <= templabel; i++)
                            {
                              

                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 K" + row["purchase"].ToString() + " 4K" + row["saporderlineno"].ToString() + " P" + row["customer"].ToString() + " 1P" + row["mfr"].ToString() + " 11k" + Tlot + " Q" + qty + " 4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                                string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'TTI INC. (IDAC)'";
                                //dbConn.updateQuery(TlotUpdate);
                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }

                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string continental(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
             
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        if ((wgt == "") && (crossdock != "1"))
                        {
                            return "inalidweight";
                        }
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }

                            string datecode1 = "";
                            string datecode2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecode1 = dAteCodeFormarte.ToString("MM", CultureInfo.InvariantCulture);

                                DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte1.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = datecode1 + tempdate1.ToString("D2") + date11;
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecode1 = dAteCodeFormarte.ToString("MM", CultureInfo.InvariantCulture);

                                DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte1.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = datecode1 + tempdate1.ToString("D2") + date11;
                            }

                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                if (row["consigneekey"].ToString() == "128892")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">702038896+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">702038896+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                }
                                else
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""pack"">4S" + "330854+" + row["pack"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""pack1"">" + "330854+" + row["pack"].ToString() + @"</variable>";
                                }
                              
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">13q" + i + "of" + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT1"">" + i + "of" + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""datecode"">" + datecode2 + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                    
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                ////dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                ////dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }


                    else if (Type == "Inner")
                    {
                        if ((wgt == "") && (crossdock != "1"))
                        {
                            return "inalidweight";
                        }
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            for (int i = 1; i <= templabel; i++)
                            {
                            
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""pack"">3S" + "330854+" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""pack1"">" + "330854+" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""labelcount1"">" + No_of_Labels + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""labelcount"">Z" + No_of_Labels + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                       
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                ////dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                ////dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string robertshaw(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {

                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        

                            string datecode1 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecode1 = dAteCodeFormarte.ToString("MMddyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecode1 = dAteCodeFormarte.ToString("MMddyy", CultureInfo.InvariantCulture);
                            }
                            DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                            string date = dAteCodeFormarte1.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }

                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""vendor"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE1"">" + datecode1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE"">" + date + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">" + row["pack"].ToString() + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                               

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }



                    }


                    else if (Type == "Inner")
                    {

                        
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecode1 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecode1 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);

                                datecode1 = dAteCodeFormarte.ToString("MMddyy", CultureInfo.InvariantCulture);

                            }
                            DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                            string date = dAteCodeFormarte1.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
                            for (int i = 1; i <= templabel; i++)
                            {
                             
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""vendor"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE"">" + date + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecode1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string Mouse(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        string datecode2 = "";
                        if (datecode == "")
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                            datecode2 = date11 + tempdate1.ToString("D2");
                        }
                        else
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                            int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                            datecode2 = date11 + tempdate1.ToString("D2");
                        }
                        string dsql = "select lot from panLabels where c_company = 'MOUSER ELECTRONICS (IDAC)'";
                        string connection = ConfigurationManager.AppSettings["DBconnection"];
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                        DataSet dsDet1 = new DataSet();
                        da1.Fill(dsDet1);
                        DataTable dt1 = dsDet1.Tables[0];
                        DataRow row1 = dt1.Rows[0];
                        int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                        string Tlot = inTlot.ToString("D6");
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""way"">" + row["TM"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""PACK"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">11k" + row["KITORDERASN"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["KITORDERASN"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""lineno"">4K" + row["saporderlineno"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""lineno1"">" + row["saporderlineno"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";


                            mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 K" + row["purchase"].ToString() + " 4K" + row["saporderlineno"].ToString() + " P" + row["customer"].ToString() + " 1P" + row["mfr"].ToString() + " 11k" + Tlot + " Q" + qty + " 4L" + COO1 + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      

                        string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'MOUSER ELECTRONICS (IDAC)'";
                        //dbConn.updateQuery(TlotUpdate);
                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }

                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);

                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string fujitsu(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""vendor"">" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                

                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string benchmark(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""vendor"">" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";


                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                          
                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string whiterogers(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {

                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        for (int i = 1; i <= templabel; i++)
                        {
                           
                            string datecodeValue = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodeValue = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodeValue = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            }
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""supply"">" + "" + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package"">S" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""date"">" + datecodeValue + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      

                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string Robert(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                   
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        string datecode2 = "";
                        if (datecode == "")
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                            datecode2 = date11 + tempdate1.ToString("D2");
                        }
                        else
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                            int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                            datecode2 = date11 + tempdate1.ToString("D2");
                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }

                        if (mlot == "")
                        {
                            mlot = row["lot"].ToString();
                        }
                        else
                        {

                        }
                        string datecodeValue = "";
                        if (datecode == "")
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            datecodeValue = dAteCodeFormarte.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                            datecodeValue = dAteCodeFormarte.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                        }
                        //for (int i = 1; i <= templabel; i++)
                        //{

                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }

                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            //mxml = mxml + Environment.NewLine + @"<variable name = ""vendor"">" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""DATE"">" + row["date1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""DATE1"">" + datecodeValue + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""NOTES"">" + "" + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""FIELD1"">" + "P" + row["customer"].ToString() + "V000000815" + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""FIELD2"">" + "H0000000000000000N/A@Q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                            mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "P" + row["customer"].ToString() + "1P" + row["mfr"].ToString() + "Q" + sku + "10D" + datecode2 + "1T" + "" + "4L" + COO1 + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                          
                        //}

                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string Yazaki(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if ((mfdate == ""))
                {
                    return "invalidmfdate";
                }
                DateTime mfdate1 = DateTime.Parse(mfdate);
                mfdate = mfdate1.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
             
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        DateTime serialdate = DateTime.Parse(row["Ldate1"].ToString());
                        string serialdate1 = serialdate.ToString("yy", CultureInfo.InvariantCulture);
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        //for (int i = 1; i <= templabel; i++)
                        //{
                        for (int i = 1; i <= templabel; i++)
                        {
                            string dsql = "select lot from panLabels where c_company = 'GRUPO YAZAKI S.A. DE C.V.'";
                            string connection = ConfigurationManager.AppSettings["DBconnection"];
                            SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            DataSet dsDet1 = new DataSet();
                            da1.Fill(dsDet1);
                            DataTable dt1 = dsDet1.Tables[0];
                            DataRow row1 = dt1.Rows[0];
                            int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            string Tlot = inTlot.ToString("D6");
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""vendor"">2p" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""vendor1"">" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""supplierid"">" + "" + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">" + mfdate + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">v" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">p" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""serial"">s3001043" + serialdate1 + Tlot + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""serial1"">3001043" + serialdate1 + Tlot + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""TO"">" + row["label_name"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                            //}
                            string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'GRUPO YAZAKI S.A. DE C.V.'";
                            //dbConn.updateQuery(TlotUpdate);
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }
        public string FutureElectronics(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                if (Type == "Outer")
                    sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                else if (Type == "futureidac_outer")
                    sqlParameters[1] = new SqlParameter("@Type", "Outer");
                if (Type == "Inner")
                    sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        if ((wgt == "") && (crossdock != "1"))
                        {
                            return "inalidweight";
                        }
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            string connection = ConfigurationManager.AppSettings["DBconnection"];

                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string dsql = "select lot from panLabels where c_company = 'FUTURE ELECTRONICS INC. (IDAC)'";
                            SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            DataSet dsDet1 = new DataSet();
                            da1.Fill(dsDet1);
                            DataTable dt1 = dsDet1.Tables[0];
                            DataRow row1 = dt1.Rows[0];
                            int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            string Tlot = inTlot.ToString("D6");
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }

                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""way"">" + row["TM"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">11K" + row["KITORDERASN"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["KITORDERASN"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""country1"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""country"">4L" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""labelcount"">" + No_of_Labels + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                                string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'FUTURE ELECTRONICS INC. (IDAC)'";
                                //dbConn.updateQuery(TlotUpdate);
                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }

                            }
                        }



                    }
                    else if (Type == "futureidac_outer")
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string connection = ConfigurationManager.AppSettings["DBconnection"];
                            string dsql = "select lot from panLabels where c_company = 'FUTURE ELECTRONICS INC. (IDAC)'";

                            SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            DataSet dsDet1 = new DataSet();
                            da1.Fill(dsDet1);
                            DataTable dt1 = dsDet1.Tables[0];
                            DataRow row1 = dt1.Rows[0];
                            int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            string Tlot = inTlot.ToString("D6");
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + "futureidac_outer" + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3s" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">p" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packcount"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, "futureidac_outer", i);

                                string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'FUTURE ELECTRONICS INC. (IDAC)'";
                                //dbConn.updateQuery(TlotUpdate);
                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }
                    }


                    else if (Type == "Inner")
                    {

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            for (int i = 1; i <= templabel; i++)
                            {
                              
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3s" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">Z" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";


                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }
        public string Master(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                for (int i = 0; i < actualCountry.Length; i++)
                {
                    if (actualCountry[i].ToString() == COO)
                    {
                        COO = Countrycode[i].ToString();
                    }

                }
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        string dsql = "select lot from panLabels where c_company = 'Master Electronics (IDAC)'";
                        string connection = ConfigurationManager.AppSettings["DBconnection"];
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                        DataSet dsDet1 = new DataSet();
                        da1.Fill(dsDet1);
                        DataTable dt1 = dsDet1.Tables[0];
                        DataRow row1 = dt1.Rows[0];
                        int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                        string Tlot = inTlot.ToString("D6");
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""way"">" + row["TM"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">11K" + row["KITORDERASN"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["KITORDERASN"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""labelcount"">" + No_of_Labels + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""country"">4L" + COO + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""country1"">" + COO + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                            string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'Master Electronics (IDAC)'";
                            //dbConn.updateQuery(TlotUpdate);
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            if (crossdock == "1")
                            {
                                string cooPickdetail = "update pickdetail set PDUDF1 = '" + COO1 + "' where orderkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(cooPickdetail);
                            }
                            else
                            {
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                            }
                            if (crossdock != "")
                            {
                                break;
                            }

                        }


                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string lear(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string datecode1 = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
              
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                        datecode = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        datecode1 = dAteCodeFormarte.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">p" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""datecode"">9d" + datecode + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""datecode1"">" + datecode + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""date"">1t" + datecode1 + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""date1"">" + datecode1 + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""supply"">v" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""supply1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package"">s" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        
                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }

                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string asine(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }

                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package"">" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""count"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";
                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        
                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }

                        }

                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string elcom(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                   sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;

                        if (mlot == "")
                        {
                            mlot = row["lot"].ToString();
                        }
                        else
                        {

                        }
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }

                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">p" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";

                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">q" + qty + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""lot"">" + mlot + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""count"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";

                            mxml = mxml + Environment.NewLine + @"</label></labels>";
                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);



                        
                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }

                        }

                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }
        public  static string xmlbuilder(string mxml, string labelname, int i)
        {
            DateTime datetime = DateTime.Now;
            string printerplace = ConfigurationManager.AppSettings["POD"];
            string Copyprint = ConfigurationManager.AppSettings["copylabel"];
            StringBuilder xml = new StringBuilder();
            Random ranNum = new Random();
            
            xml.Append(mxml);

            string Tdatetime = datetime.ToString("MM/dd/yyyy hh:mm:ss.fff tt", CultureInfo.InvariantCulture);
            //string Tdatetime = string.Format("{0:F}", datetime);
            Tdatetime = Tdatetime.Replace(':', '-');
            Tdatetime = Tdatetime.Replace('/', '-');
            string mfilename = printerplace + labelname + Tdatetime + " " + i + ".xml";

            string mfilename2 = Copyprint + labelname + Tdatetime + " " + i + ".xml";
           
            //await xmlbuilder.delay()
            StreamWriter writer = new StreamWriter(mfilename);
            StreamWriter writer1 = new StreamWriter(mfilename2);


            writer.Write(xml.ToString());
            writer.Close();
            writer1.Write(xml.ToString());
            writer1.Close();
            methodCallingCount = methodCallingCount + 1;
            if(methodCallingCount == 100)
            {
                Thread.Sleep(2000);
                methodCallingCount = 0;
            }
            
            return "";
        }



        public string fcacrysler(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                 
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        if (mfdate == "")
                        {
                            return "invalidmfdate";
                        }
                        DateTime mfdate1 = DateTime.Parse(mfdate);
                        mfdate = mfdate1.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3S" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">" + mfdate + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                     
                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }
                  

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string integration(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">" + row["pack"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                  

                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string ghsp(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                       if (mfdate == "")
                        {
                            return "invalidmfdate";
                        }
                        DateTime mfdate1 = DateTime.Parse(mfdate);
                        
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        string datecode2 = "";
                        if (datecode == "")
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                            datecode2 = date11 + tempdate1.ToString("D2");
                        }
                        else
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                            int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                            datecode2 = date11 + tempdate1.ToString("D2");
                        }
                        mfdate = mfdate1.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">p" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package"">1t" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">" + mfdate + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""country"">" + COO1 + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po"">a" + row["purchase"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""serial"">s" + "1T" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""serial1"">" + "1T" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> P" + row["customer"].ToString() + " Q" + qty + " A" + row["purchase"].ToString() + " 1T" + row["pack"].ToString() + " S1T" + row["pack"].ToString() + @"</variable>";

                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                    
                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string eps(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        string datecode2 = "";
                        if (datecode == "")
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                            datecode2 = date11 + tempdate1.ToString("D2");
                        }
                        else
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                            int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                            datecode2 = date11 + tempdate1.ToString("D2");
                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if (mlot == "")
                        {
                            mlot = row["lot"].ToString();
                        }
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        string datecodeValue = "";
                        if (datecode == "")
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            datecodeValue = dAteCodeFormarte.ToString("MMdd", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                            datecodeValue = dAteCodeFormarte.ToString("MMdd", CultureInfo.InvariantCulture);
                        }
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""date"">" + datecode + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package"">" + row["pack"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">" + sku + @"</variable>";
                          
                            mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "SPA" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         

                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string alliedelec(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if (mlot == "")
                        {
                            mlot = row["lot"].ToString();
                        }
                        if(crossdock != "")
                        {
                            No_of_Labels = row["totalBox"].ToString();
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        string datecodeValue = "";
                        if (datecode == "")
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            datecodeValue = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                            datecodeValue = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        }
                        string dsql = "select lot from panLabels where c_company = 'ALLIED ELECTRONICS, INC. (IDAC)'";
                        string connection = ConfigurationManager.AppSettings["DBconnection"];
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                        DataSet dsDet1 = new DataSet();
                        da1.Fill(dsDet1);
                        DataTable dt1 = dsDet1.Tables[0];
                        DataRow row1 = dt1.Rows[0];
                        int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                        string Tlot = inTlot.ToString("D6");
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""way"">" + row["TM"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">11K" + row["KITORDERASN"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["KITORDERASN"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                       
                        string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'ALLIED ELECTRONICS, INC. (IDAC)'";
                        //dbConn.updateQuery(TlotUpdate);
                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string newark(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if (mlot == "")
                        {
                            mlot = row["lot"].ToString();
                        }
                        if(crossdock != "")
                        {
                                                            No_of_Labels = row["totalBox"].ToString();;
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        //string datecodeValue = "";
                        //if (datecode == "")
                        //{
                        //    DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                        //    datecodeValue = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        //}
                        //else
                        //{
                        //    DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                        //    datecodeValue = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        //}
                        string dsql = "select lot from panLabels where c_company = 'Newark Electronics (ECG)'";
                        string connection = ConfigurationManager.AppSettings["DBconnection"];
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                        DataSet dsDet1 = new DataSet();
                        da1.Fill(dsDet1);
                        DataTable dt1 = dsDet1.Tables[0];
                        DataRow row1 = dt1.Rows[0];
                        int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                        string Tlot = inTlot.ToString("D6");
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""way"">" + row["TM"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""way1"">" + row["TM"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">11K" + row["KITORDERASN"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["KITORDERASN"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po"">K" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""po1"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">" + No_of_Labels + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         
                        string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'Newark Electronics (ECG)'";
                            //dbConn.updateQuery(TlotUpdate);
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);

                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string samina(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if (mlot == "")
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            mlot = dAteCodeFormarte.ToString("ddyy", CultureInfo.InvariantCulture);
                            mlot = tempdate1.ToString("D2") + mlot;
                        }
                        if(crossdock != "")
                        {
                                                            No_of_Labels = row["totalBox"].ToString();;
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        if (mfdate == "")
                        {
                            return "invalidmfdate";
                        }
                        DateTime mfdate1 = DateTime.Parse(mfdate);
                        //if (datecode == "")
                        //{
                        //    DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                        //    datecode = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        //}
                        //else
                        //{
                        //    DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                        //    datecode = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        //}
                        mfdate = mfdate1.ToString("yy/MM", CultureInfo.InvariantCulture);
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">" + mfdate + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""country"">" + COO + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""lot"">" + mlot + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""count"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        

                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);

                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }
        public string valeo(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";


                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                                                            No_of_Labels = row["totalBox"].ToString();;
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        if (mfdate == "")
                        {
                            return "invalidmfdate";
                        }
                        DateTime mfdate1 = DateTime.Parse(mfdate);
                        mfdate = mfdate1.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
                        string dsql = "select lot from panLabels where c_company = 'VALEO NORTH AMERICA INC'";
                        string connection = ConfigurationManager.AppSettings["DBconnection"];
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                        DataSet dsDet1 = new DataSet();
                        da1.Fill(dsDet1);
                        DataTable dt1 = dsDet1.Tables[0];
                        DataRow row1 = dt1.Rows[0];
                        int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                        string Tlot = inTlot.ToString("D6");
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package"">" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR1"">" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""lot"">" + Tlot + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""labelcode"">s" + Tlot + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""labelcode1"">" + Tlot + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">" + mfdate + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      
                        string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'VALEO NORTH AMERICA INC'";
                            //dbConn.updateQuery(TlotUpdate);
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }
        public string jabill(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string returnvalue = "Success";
                string labelname = "";
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", "Outer");
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string COO1 = "";
                        for (int j = 0; j < actualCountry.Length; j++)
                        {
                            if (actualCountry[j].ToString() == COO)
                            {
                                COO1 = Countrycode[j].ToString();
                            }

                        }
                        if (label != "")
                            labelname = label;
                        else
                            break;
                        if(crossdock != "")
                        {
                                                            No_of_Labels = row["totalBox"].ToString();;
                            qty = row["qty"].ToString();
                            sku = row["vendor"].ToString();
                        }
                        int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                        string datecodeValue = "";
                        if (datecode == "")
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            datecodeValue = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                            datecodeValue = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                        }
                        for (int i = 1; i <= templabel; i++)
                        {
                            if (crossdock != "")
                            {
                                i = int.Parse(row["eachBox"].ToString());
                            }
                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3S" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">1P" + sku + @"</variable>";
                            if (crossdock != "")
                            {
                                mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                            }
                            mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR1"">" + sku + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""date"">" + datecodeValue + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""count"">" + i + " of " + templabel + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"</label></labels>";

                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                       

                        string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                            if (crossdock != "")
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    returnvalue = "FAil";
                }

                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string flex(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        if ((wgt == "") && (crossdock != "1"))
                        {
                            return "inalidweight";
                        }
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (mfdate == "")
                            {
                                return "invalidmfdate";
                            }
                            DateTime mfdate1 = DateTime.Parse(mfdate);
                            string mfdate3 = "";
                            mfdate3 = mfdate1.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecode1 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecode1 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecode1 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["pack"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""date"">9d" + datecode1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""date1"">" + datecode1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1p" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">p" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">1t" + mfdate3 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mdate1"">" + mfdate3 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""country"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3s" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""PACK"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                              
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                ////dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                ////dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }


                    else if (Type == "Inner")
                    {
                       

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (mfdate == "")
                            {
                                return "invalidmfdate";
                            }
                            DateTime mfdate1 = DateTime.Parse(mfdate);
                            string mfdate3 = "";
                            mfdate3 = mfdate1.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["lot"].ToString();
                            }
                            string datecode1 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecode1 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecode1 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                              
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">1t" + mfdate3 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mdate1"">" + mfdate3 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1p" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">p" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""date"">9d" + datecode1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""date1"">" + datecode1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""country"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>"
;
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                ////dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                ////dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string sanmina(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {

                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                         
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if (crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte1.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                       
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">" + row["PURCHASEORDERDOCUMENT"].ToString() + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["PURCHASEORDERDOCUMENT"].ToString() + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""datetime"">" + datetime + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""grosswgt"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }


                    else if (Type == "Inner")
                    {
                        

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
            
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["lot"].ToString();
                            }
                            string datecode1 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecode1 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecode1 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                              
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">" + row["PURCHASEORDERDOCUMENT"].ToString() + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["PURCHASEORDERDOCUMENT"].ToString() + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                             
                            }
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);

                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string vitesco(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                string COO1 = "";
                for (int j = 0; j < actualCountry.Length; j++)
                {
                    if (actualCountry[j].ToString() == COO)
                    {
                        COO1 = Countrycode[j].ToString();
                    }

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if (crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte1.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""pack"">3S331872+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""pack1"">331872+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">13Q" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT1"">" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""datetime"">" + datetime + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                          
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }


                    else if (Type == "Inner")
                    {
                      

                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                return "MandatoryLot";
                                break;
                            }
                            string datecode1 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecode1 = dAteCodeFormarte.ToString("MMddyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecode1 = dAteCodeFormarte.ToString("MMddyy", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                              
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + mlot + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + mlot + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecode1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecode1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string sumito(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datecode1 = dAteCodeFormarte.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">p" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty1"">" + qty + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""lot"">a" + mlot + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""lot1"">" + mlot + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">l" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">m" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }

                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "M" + row["pack"].ToString() + " P" + row["customer"].ToString() + " Q" + qty + " A" + row["purchase"].ToString() + " V12450" + " L" + mlot + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                          
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string honsyssensor(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            datecode = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE1"">" + datecode + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }
        public string ge(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Today;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            //if (datecode == "")
                            //{
                            //    DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            //    datecodeValue = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            //}
                            //else
                            //{
                            //    DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                            //    datecodeValue = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            //}
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""VENDOR"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                           
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string getItemLabel(String SKU, String Qty, String No_of_Labels, string printer)
        {
            try
            {
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];

                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + "itemlabel" + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + No_of_Labels + @""">";
                mxml = mxml + Environment.NewLine + @"<label>";
                mxml = mxml + Environment.NewLine + @"<variable name = ""sku"">" + SKU.ToString() + @"</variable>";
                mxml = mxml + Environment.NewLine + @"<variable name = ""qty"">" + Qty.ToString() + @"</variable>";
                mxml = mxml + Environment.NewLine + @"</label></labels>";
                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, "itemlabel", 1);

                return "Success";
            }
            catch (Exception ex)
            {
                dbConn.logCapture(ex.Message.ToString(), "itemlabel", Qty, SKU,"","");
                return "Label not generated, Please contact support team";
            }

        }
        //Norvanco
        public string KOSTAL(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                      
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3s727745+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">727745+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">13q" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT1"">" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATETIME"">" + datetime + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string LEARESD(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                string COO1 = "";
                for (int j = 0; j < actualCountry.Length; j++)
                {
                    if (actualCountry[j].ToString() == COO)
                    {
                        COO1 = Countrycode[j].ToString();
                    }

                }
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3s00006453+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">00006453+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">13q" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT1"">" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATETIME"">" + datetime + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""WEIGHT"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string fedco(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                string COO1 = "";
                for (int j = 0; j < actualCountry.Length; j++)
                {
                    if (actualCountry[j].ToString() == COO)
                    {
                        COO1 = Countrycode[j].ToString();
                    }

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if (crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3s" + row["pack"].ToString() + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString() + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATETIME"">" + datetime + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>"; 
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                    
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string norvanco_general(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";
                string crossdockData = "";
                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    crossdockData = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                string COO1 = "";
                for (int j = 0; j < actualCountry.Length; j++)
                {
                    if (actualCountry[j].ToString() == COO)
                    {
                        COO1 = Countrycode[j].ToString();
                    }

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if (crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                if(crossdockData != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3s" + row["SALESORDERDOCUMENT"].ToString() + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["SALESORDERDOCUMENT"].ToString() + row["CANO"].ToString() + @"</variable>";
                                }
                                else
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""package"">3s" + row["pack"].ToString()   + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""package1"">" + row["pack"].ToString()  + @"</variable>";
                                }
                           
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATETIME"">" + datetime + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string robertbosch(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if (crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""SERIAL"">S" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""SERIAL1"">" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }
        public string aptiv(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                if (mfdate == "")
                {
                    return "invalidmfdate";
                }
                DateTime mfdate1 = DateTime.Parse(mfdate);
                mfdate = mfdate1.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""lineno"">4K" + row["saporderlineno"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""lineno1"">" + row["saporderlineno"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo11"">1T" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo12"">" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">" + mfdate + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string bcs(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }

                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                Random generator = new Random();
                                String randomnumber = generator.Next(0, 10000).ToString("D5");
                                mxml = mxml + Environment.NewLine + @"<variable name = ""serial"">S237549" + randomnumber + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""serial1"">237549" + randomnumber + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string continentalauto(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte1.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            }

                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                if(row["consigneekey"].ToString() == "128892")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">702038896+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">702038896+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                }
                                else
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s331872+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">331872+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s331872+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">331872+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">13q" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT1"">" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATETIME"">" + datetime + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string DIGI_KEY_CORP(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";
                string crossDockData = "";
                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    crossDockData = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
            
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte1.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("yyMMdd", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("yyMMdd", CultureInfo.InvariantCulture);
                            }
                            string datecodetemp3 = "";
                            string datetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                datetemp2 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecodetemp3 = datetemp2 + tempdate1.ToString("D2");
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                datetemp2 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecodetemp3 = datetemp2 + tempdate1.ToString("D2");
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                if(crossDockData != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["PURCHASEORDERDOCUMENT"].ToString() + "+" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["PURCHASEORDERDOCUMENT"].ToString() + "+" + row["CANO"].ToString() + @"</variable>";
                                }
                                else
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                }

                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""datecode"">D" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""datecode1"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 P" + row["customer"].ToString() + " 1P" + row["mfr"].ToString() + " Q" + qty + " 10D" + datecodetemp2 + " 1T" + mlot + " 4L" + COO + " " + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string HELLA_Electronics(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                  
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                string COO1 = "";
                for (int j = 0; j < actualCountry.Length; j++)
                {
                    if (actualCountry[j].ToString() == COO)
                    {
                        COO1 = Countrycode[j].ToString();
                    }

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }

                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""cusotpo"">V" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""cusotpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">S" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field4"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field5"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                          
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string DURA_AUTOMOTIVE(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte1.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("yyMMdd", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("yyMMdd", CultureInfo.InvariantCulture);
                            }

                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                Random generator = new Random();
                                String randomnumber = generator.Next(0, 10000).ToString("D5");
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3sS022803+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + randomnumber + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">S022803+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + randomnumber + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">13a" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT1"">" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""datetime"">" + datetime + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field4"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field5"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field6"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field7"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string ZF(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string mfdate, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                      
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            DateTime dAteCodeFormarte1 = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte1.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            if (mlot == "")
                            {
                                mlot = row["date2"].ToString();
                            }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }

                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                Random generator = new Random();
                                String randomnumber = generator.Next(0, 10000).ToString("D5");
                                mxml = mxml + Environment.NewLine + @"<variable name = ""serial"">S237549" + randomnumber + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""serial1"">237549" + randomnumber + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }

        }

        public string abb(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            //string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string datecodetemp2 = "";
                                if (datecode == "")
                                {
                                    DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                    datecodetemp2 = dAteCodeFormarte.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                    datecodetemp2 = dAteCodeFormarte.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                                }
                                int j = i + 1;

                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["CONSIGNEEKEY"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["CONSIGNEEKEY"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecodetemp2 + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""WEIGHT"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i.ToString("D3") + " / " + templabel.ToString("D3") + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                    
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                                //using (SqlConnection conn = new SqlConnection(connection))
                                //{
                                //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                                //    try
                                //    {
                                //        conn.Open();
                                //        cmd.ExecuteNonQuery();
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        Console.WriteLine(ex.Message);
                                //    }
                                //    finally
                                //    {
                                //        conn.Close();
                                //    }
                                //}
                                if (crossdock != "")
                                {
                                    break;
                                }

                            }
                        }


                    }



                    else if (Type == "Inner")
                    {
                        
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        LabelsOName.Add("DIGI-KEY (IDAC)", "digikey-inner label");
                        LabelsOName.Add("ARROW ELECTRONICS INC. (IDAC)", "Arrow_inner_label");
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                             
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                             
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        
                          
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string alps(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            //string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");
                          
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                Random generator = new Random();
                                String randomnumber = generator.Next(0, 100000).ToString("D6");
                                mxml = mxml + Environment.NewLine + @"<variable name = ""serial"">" + row["SALESORDERDOCUMENT"].ToString() + randomnumber + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""serial1"">" + row["SALESORDERDOCUMENT"].ToString() + randomnumber + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LINE"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                       
                          
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }



                    else if (Type == "Inner")
                    {
                        

                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                            
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         
                            
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string arrowv1(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
           
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            //string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            if (mlot == "")
                            {
                                mlot = row["lot"].ToString();
                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            string datecodetemp3 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                datecodetemp2 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecodetemp3 = datecodetemp2 + tempdate1.ToString("D2");
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                datecodetemp2 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecodetemp3 = datecodetemp2 + tempdate1.ToString("D2");
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                Random generator = new Random();
                                String randomnumber = generator.Next(0, 100000).ToString("D6");
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["SALESORDERDOCUMENT"].ToString() + randomnumber + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["SALESORDERDOCUMENT"].ToString() + randomnumber + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">9D" + datecodetemp3 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecodetemp3 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field4"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 P"+ row["customer"].ToString() + " 1P"+ row["mfr"].ToString() + " Q"+qty+ " 10D"+ datecodetemp3 + " 1T" + mlot + " 4L"+COO+ " " + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         
                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }


                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }



                    else if (Type == "Inner")
                    {
                      

                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            string datecodetemp3 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                datecodetemp2 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecodetemp3 = datecodetemp2 + tempdate1.ToString("D2");
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                datecodetemp2 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecodetemp3 = datecodetemp2 + tempdate1.ToString("D2");
                            }
                            if (mlot == "")
                            {
                                mlot = row["lot"].ToString();
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                              
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">9D" + datecodetemp3 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecodetemp3 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 P" + row["customer"].ToString() + " 1P" + row["mfr"].ToString() + " Q" + qty + " 10D" + datecodetemp3 + " 1T" + mlot + " 4L" + COO + " " + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                       
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string cornel(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            //string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                //SecureRandom random = new SecureRandom();
                                //int num = random.NextInt(100000);
                                //String formatted = String.Format("%05d", num);
                                Random generator = new Random();
                                String randomnumber = generator.Next(0, 10000).ToString("D5");
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["pack"].ToString() + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["pack"].ToString() + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field4"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                            
                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }


                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }



                    else if (Type == "Inner")
                    {
                       

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            //string dsql = "select lot from panLabels where c_company='CORNELL DUBILIER ELECT. (MA)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");
                            string datecode2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = date11 + tempdate1.ToString("D2");

                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = date11 + tempdate1.ToString("D2");
                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            for (int i = 1; i <= templabel; i++)
                            {
                             
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T3548" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">3548" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""SPECIAL"">Z" + COO1 + "+" + datecode2 + "+R" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""SPECIAL1"">" + COO1 + "+" + datecode2 + "+R" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                //string TlotUpdate = "update panLabels set lot = '" + Tlot + "' where c_company = 'CORNELL DUBILIER ELECT. (MA)'";

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string FLEXTRONICS_AMERICA(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";
                string crossdockData = "";
                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    crossdockData = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    
                    if ((wgt == "") && (crossdockData != "1"))
                    {
                        return "inalidweight";
                    }
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            //string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                if(crossdockData != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["PURCHASEORDERDOCUMENT"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["PURCHASEORDERDOCUMENT"].ToString()  + @"</variable>";
                                }
                                else
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["pack"].ToString().TrimStart(new Char[] { '0' })  + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["pack"].ToString().TrimStart(new Char[] { '0' })  + @"</variable>";
                                }
                             
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""WEIGHT"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field4"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                          
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }



                    else if (Type == "Inner")
                    {
                        

                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                              
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">K" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">9D" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                    
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string KIMBALL(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";
                string crossdockData = "";
                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    crossdockData = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {

                    if ((wgt == "") && (crossdockData != "1"))
                    {
                        return "inalidweight";
                    }
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            //string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if (crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("yyMM", CultureInfo.InvariantCulture);
                            }
                            string datecodetemp3 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp3 = dAteCodeFormarte.ToString("yyMMdd", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp3 = dAteCodeFormarte.ToString("yyMMdd", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                if (crossdockData != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["PURCHASEORDERDOCUMENT"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["PURCHASEORDERDOCUMENT"].ToString() + @"</variable>";
                                }
                                else
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                }

                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""WEIGHT"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""lot"">" + datecodetemp3 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""lot1"">" + datecodetemp3 + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno, lpn);
                return "Label not generated, Please contact support team";
            }
        }
        public string HARMAN_DE(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            //string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s417815+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">417815+" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">13q" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT1"">" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""datetime"">" + datetime + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""GROSSWGT"">" + wgt + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                    
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }



                    else if (Type == "Inner")
                    {
                      

                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                            
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">9D" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string HITACHI(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">4L" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY1"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT"">13q" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LABELCOUNT1"">" + i + " / " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">13q" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT1"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                        
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }



                    else if (Type == "Inner")
                    {

                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("yyyyddMM", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("yyyyddMM", CultureInfo.InvariantCulture);
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }
                            }
                            for (int i = 1; i <= templabel; i++)
                            {

                          
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }
        public string HITACHIASTEMO(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            if (mlot == "")
                            {
                                mlot = row["lot"].ToString();
                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }


                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + No_of_Labels + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + Orderkey + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + Orderkey + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field4"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field5"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);

                                string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }
                    else if (Type == "Inner")
                    {
                        int i = 0;

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (mlot == "")
                            {
                                mlot = row["lot"].ToString();
                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }

                            string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + No_of_Labels + @""">";
                            mxml = mxml + Environment.NewLine + @"<label>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["PURCHASEORDERDOCUMENT"].ToString() + row["CANO"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["PURCHASEORDERDOCUMENT"].ToString() + row["CANO"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">" + row["mfr"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""serial"">" + Orderkey+"0" + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""serial1"">" + Orderkey + "0" + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                            mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";


                            mxml = mxml + Environment.NewLine + @"</label></labels>";
                            
                            LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                            //dbConn.updateQuery(printUpdate);
                            string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                            //dbConn.updateQuery(cooPickdetail);
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }
        public string JABILCIRCUITNorvanko(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            //string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                            string datetime = dAteCodeFormarte.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">3s09876+" + row["pack"].ToString()+i + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">09876" + row["pack"].ToString() +i+ @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""WEIGHT"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field4"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field5"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field6"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";



                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }

                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }



                    else if (Type == "Inner")
                    {
                        

                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MMddyyyy", CultureInfo.InvariantCulture);
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                              
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">9D" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";


                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                      
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string lutron(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            //string dsql = "select lot from panLabels where c_company = 'DIGI-KEY (IDAC)'";
                            //string connection = ConfigurationManager.AppSettings["DBconnection"];
                            //SqlDataAdapter da1 = new SqlDataAdapter(dsql, connection);
                            //DataSet dsDet1 = new DataSet();
                            //da1.Fill(dsDet1);
                            //DataTable dt1 = dsDet1.Tables[0];
                            //DataRow row1 = dt1.Rows[0];
                            //int inTlot = int.Parse(row1["lot"].ToString()) + 1;
                            //string Tlot = inTlot.ToString("D6");

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }


                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field3"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field4"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field5"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field6"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                         
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }



                    else if (Type == "Inner")
                    {

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecodetemp2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                datecodetemp2 = dAteCodeFormarte.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                            
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";


                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">" + datecodetemp2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["CANO"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNTRY"">" + COO1 + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";


                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                   
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string Magna(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {
                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
             
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                if (dataTable.Rows.Count > 0)
                {
                    if ((wgt == "") && (crossdock != "1"))
                    {
                        return "inalidweight";
                    }
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                           
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                            No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            } 
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing"">4s" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + "+"+i+ @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""packing1"">" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + "+"+i + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""COUNT"">" + i + " of " + templabel + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""WEIGHT"">" + wgt + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""desc"">" + row["descr"].ToString() + @"</variable>";

                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field1"">" + "" + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""Field2"">" + "" + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 4s" + row["pack"].ToString().TrimStart(new Char[] { '0' }) + "+" + i + " 1P" + row["mfr"].ToString() + " Q" + qty + " P" + sku + " k" + row["purchase"].ToString() + @" </variable>";

                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                  
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                            //string Lupsql = "update panLabels set lot = '" + Tlot + "' where c_company = 'DIGI-KEY (IDAC)'";
                            //using (SqlConnection conn = new SqlConnection(connection))
                            //{
                            //    SqlCommand cmd = new SqlCommand(Lupsql, conn);
                            //    try
                            //    {
                            //        conn.Open();
                            //        cmd.ExecuteNonQuery();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        conn.Close();
                            //    }
                            //}
                        }


                    }



                    else if (Type == "Inner")
                    {
                      
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (crossdock != "")
                            {
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }

                            if (mlot == "")
                            {
                                return "MandatoryLot";
                                break;
                            }
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecode2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = date11 + tempdate1.ToString("D2");
                                
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = date11 + tempdate1.ToString("D2");
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                             
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";


                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + mlot.ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + mlot.ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo"">k" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpo1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE"">10D" + datecode2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""DATECODE1"">" + datecode2 + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                     
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }
                        }
                      
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

        public string visteon(string Type, string sku, string Orderkey, string No_of_Labels, string COO, string qty, string label, string datecode, string mlot, string wgt, string printer, string mfdate, string lpn, string orderlineno, string crossDockChecker)
        {
            try
            {

                DateTime currentdate = DateTime.Now;
                string labelname = "";
                string returnvalue = "Success";
                string crossdock = "";

                DataTable dataTable = new DataTable();
                string printerplace = ConfigurationManager.AppSettings["POD"];
                string printername = ConfigurationManager.AppSettings["printer"];
                // sp parameter
                SqlParameter[] sqlParameters = new SqlParameter[4];
                sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
                sqlParameters[1] = new SqlParameter("@Type", Type.ToString());
                sqlParameters[2] = new SqlParameter("@sku", sku);
                if ((qty == "") || (crossDockChecker == "True"))
                {
                    crossdock = "1";
                    sqlParameters[3] = new SqlParameter("@lpn", lpn);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                    //dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PIDSA_CROSSDOCK]", sqlParameters);
                }
                else
                {
                    sqlParameters[3] = new SqlParameter("@orderlineno", orderlineno);
                    dataTable = dbConn.executeSP("[wmwhse3].[VY_PIDSA_PROD_STANDARD]", sqlParameters);

                }
                //if(mfdate == "")
                //{
                //    return "invalidmfdate";
                //}
                if (mfdate == "")
                {
                    return "invalidmfdate";
                }
                DateTime mfdate1 = DateTime.Parse(mfdate);
                mfdate = mfdate1.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime sixMonthsAfter = mfdate1.AddMonths(+6);
                string expdate = sixMonthsAfter.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                if (dataTable.Rows.Count > 0)
                {
                    if (Type == "Outer")
                    {
                        IDictionary<string, string> LabelsOName = new Dictionary<string, string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }

                            if (label != "")
                                labelname = label;
                            else
                                break;
                            if(crossdock != "")
                            {
                                No_of_Labels = row["totalBox"].ToString();
                                qty = row["qty"].ToString();
                                sku = row["vendor"].ToString();
                                wgt = row["packwegt"].ToString();

                            }
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }if(sku == "") { sku = row["vendor"].ToString(); }

                            for (int i = 1; i <= templabel; i++)
                            {
                                if (crossdock != "")
                                {
                                    i = int.Parse(row["eachBox"].ToString());
                                }
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ASN"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""ASN1"">" + row["purchase"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">16D" + mfdate + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mdate1"">16D" + mfdate + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""expdate"">14D" + expdate + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""expdate1"">" + expdate + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""SKU"">S" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""SKU1"">" + sku + @"</variable>";
                                if (crossdock != "")
                                {
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""TOID"">" + row["lpn"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""containerkey"">" + row["CANO"].ToString() + @"</variable>";
                                    mxml = mxml + Environment.NewLine + @"<variable name = ""ctnnumber"">" + row["eachBox"].ToString() + @"</variable>";
                                }
                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 S" + sku + " P" + row["customer"].ToString() + " V" + "80016008" + " Q" + qty + " 1T" + row["pack"].ToString() + " 16D" + mfdate + " 14D" + expdate + " 1P" + row["mfr"].ToString() + @" </variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";
                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                    

                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);
                                if (crossdock != "")
                                {
                                    break;
                                }
                            }
                        }


                    }



                    else if (Type == "Inner")
                    {

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string COO1 = "";
                            for (int j = 0; j < actualCountry.Length; j++)
                            {
                                if (actualCountry[j].ToString() == COO)
                                {
                                    COO1 = Countrycode[j].ToString();
                                }

                            }
                            if (label != "")
                                labelname = label;
                            else
                                break;
                            int templabel = int.Parse(No_of_Labels);if(sku == "") { sku = row["vendor"].ToString(); }
                            string datecode2 = "";
                            if (datecode == "")
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(row["Ldate1"].ToString());
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = date11 + tempdate1.ToString("D2");
                            }
                            else
                            {
                                DateTime dAteCodeFormarte = DateTime.Parse(datecode);
                                int tempdate1 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dAteCodeFormarte, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                string date11 = dAteCodeFormarte.ToString("yy", CultureInfo.InvariantCulture);
                                datecode2 = date11 + tempdate1.ToString("D2");
                            }
                            for (int i = 1; i <= templabel; i++)
                            {
                              
                                string mxml = @"<?xml version=""1.0"" encoding=""UTF-8""?><labels _FORMAT=""" + labelname + @""" _PRINTERNAME=""" + printer + @""" _QUANTITY= """ + "1" + @""">";
                                mxml = mxml + Environment.NewLine + @"<label>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""company"">" + row["c_company"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr1"">" + row["c_address1"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr2"">" + row["c_address2"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""addr3"">" + row["C_CITY"].ToString() + " " + row["C_STATE"].ToString() + " " + row["C_ZIP"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mdate"">16D" + mfdate + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mdate1"">16D" + mfdate + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""expdate"">14D" + expdate + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""expdate1"">" + expdate + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY"">Q" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""QTY1"">" + qty + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin"">P" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""custpin1"">" + row["customer"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr"">1P" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""mfr1"">" + row["mfr"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT"">1T" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""LOT1"">" + row["pack"].ToString() + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""SKU"">S" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""SKU1"">" + sku + @"</variable>";
                                mxml = mxml + Environment.NewLine + @"<variable name = ""productionno"">" + "[)> 06 S" + sku + " P" + row["customer"].ToString() + " V" + "80016008" + " Q" + qty + " 1T" + row["pack"].ToString() + " 16D" + mfdate + " 14D" + expdate + " 1P" + row["mfr"].ToString() + @" </variable>";
                                mxml = mxml + Environment.NewLine + @"</label></labels>";

                                LabelApp.pidsalabelGenerator.labelGenerator.xmlbuilder(mxml, labelname, i);
                     
                            string printUpdate = "update receipt set SOURCELOCATION = 'PRINTED' where receiptkey = '" + Orderkey + "'";
                                //dbConn.updateQuery(printUpdate);
                                string cooPickdetail = "update receiptdetail set EXT_UDF_STR2 = '" + COO1 + "' where receiptkey = '" + Orderkey + "' AND sku = '" + sku + "'";
                                //dbConn.updateQuery(cooPickdetail);

                            }

                        }
                    }
                }
                else
                {
                    returnvalue = "fail";
                }
                if ((labelname != "") && (returnvalue == "Success"))
                {
                    dbConn.logCapture("Success", label, Orderkey, sku, orderlineno, lpn);
                    return "Success";
                }
                else
                {
                    dbConn.logCapture("Not Valid", label, Orderkey, sku, orderlineno, lpn);
                    return "Fails";
                }
            }
            catch (Exception ex)
            {
                string captureException = ex.Message.Replace("'", " ");
                dbConn.logCapture(captureException, label, Orderkey, sku, orderlineno,lpn);
                return "Label not generated, Please contact support team";
            }
        }

    }
}