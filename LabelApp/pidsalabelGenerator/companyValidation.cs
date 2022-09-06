using LabelApp.utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LabelApp.pidsalabelGenerator
{
    public class companyValidation
    {
        public string xmlchecker(string InnerQty, string sku, string OuterQty, string Orderkey, string No_of_Labels, string COO, string mfdate, string datecode, string lot, string wgt, string printer, string innercount, string crossdockorder, string lpn,string orderlineno)
        {
            SqlParameter[] sqlParameters = new SqlParameter[2];
            DataTable dataTable = new DataTable();
            utilityConnection dbConn = new utilityConnection();
            string crossDockChecker = "select type from receipt where receiptkey = '" + Orderkey + "' AND type ='500'";
            string connection = ConfigurationManager.AppSettings["DBconnection"];
            SqlDataAdapter da1 = new SqlDataAdapter(crossDockChecker, connection);
            DataSet dsDet1 = new DataSet();
            da1.Fill(dsDet1);
            DataTable dt1 = dsDet1.Tables[0];
           
            sqlParameters[0] = new SqlParameter("@orderkey", Orderkey.ToString());
            sqlParameters[1] = new SqlParameter("@cross", crossdockorder.ToString());

            dataTable = dbConn.executeSP("[wmwhse3].[Panasonic_Label_Comapany]", sqlParameters);
   
            string result = "";
            if (crossdockorder == "True")
            {
                if(((crossdockorder == "True") && (OuterQty == null)) || ((crossdockorder == "True") && (InnerQty != null)))
                {
                    if (dt1.Rows.Count == 0)
                    {
                        dataTable = new DataTable();
                        result = "Please Print Cross Dock Order";
                    }
                    else
                    {
                        if(InnerQty == null)
                        OuterQty = "";
                    }
                }
              
            }
               
            //}

            
           
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                string innerlabel = row["innerLabel"].ToString();
                string outerlabel = row["outLabel"].ToString();
                string company_name = row["c_company"].ToString();
                string general = row["genLabel"].ToString();

                labelGenerator LabelGenerator = new labelGenerator();
                if ((innerlabel == "digikey_inner_label") && (outerlabel == "digikey_outer_label"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.digikey(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.digikey(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "Arrow_inner_label") && (outerlabel == "Arrow_outer_label"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.arrow(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.arrow(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "avnet") || (general == "GENERAL"))
                {
                    string Tlabel = "";
                    if (general != "")
                        Tlabel = "GENERAL";
                    else
                        Tlabel = outerlabel;

                    result = LabelGenerator.general(null, sku, Orderkey, No_of_Labels, COO, OuterQty, Tlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                }
                if ((innerlabel == "TTI_INNER_LABEL") && (outerlabel == "TTI_OUTER_LABEL"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.TTi(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.TTi(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "continental_inner") && (outerlabel == "continental_outer"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.continental(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.continental(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "Robertshaw_innerLabel") && (outerlabel == "Robertshaw_OuterLabel"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.robertshaw(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.robertshaw(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "mouser"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.Mouse(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "FUJITSU"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.fujitsu(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "BENCHMARK ELEC"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.benchmark(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "WhiteRodgers"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.whiterogers(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "master"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.Master(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "Learcorp"))
                {
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.lear(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "Aisin"))
                {
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.asine(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "Elcom"))
                {
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.elcom(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "Robert Bosch"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.Robert(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "Yazaki"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.Yazaki(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "FCA_CHRYSLER"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.fcacrysler(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "Integration Micro Circuit"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.integration(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "ghsp"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.ghsp(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "EPS"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.eps(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "alliedelec"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.alliedelec(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "newark"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.newark(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "SANMINA-ROCHE"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.samina(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "Valeo_north"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.valeo(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "Jabil_Circuit"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.jabill(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "Future_Inner") && ((outerlabel == "Future_Outer") || (outerlabel == "futureidac_outer")))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.FutureElectronics(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        if (outerlabel == "futureidac_outer")
                        {
                            SqlCommand myCommand = new SqlCommand();
                            result = LabelGenerator.FutureElectronics("futureidac_outer", sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                        }
                        if (outerlabel == "Future_Outer")
                        {
                            string outerqtychecker = "Outer";
                            SqlCommand myCommand = new SqlCommand();
                            result = LabelGenerator.FutureElectronics(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                        }
                    }
                }
                if (outerlabel == "Sumitomo outer label")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.sumito(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "HonsysSensor")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.honsyssensor(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "GE")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.ge(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "KOSTAL")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.KOSTAL(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "LEARESD")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.LEARESD(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "FEDCO_outer")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.fedco(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (general == "norvanco_general")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.norvanco_general(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, general, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "ROBERTBOSCH_OUTER")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.robertbosch(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((outerlabel == "APTIV_REYNOSA") || (outerlabel == "borg_warner"))
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.aptiv(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "BCS_automotive_interface_solut")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.bcs(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "continental_automotive_systems")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.continentalauto(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "Flextronics-Inner Label") && (outerlabel == "Flextronics-Outer label"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.flex(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.flex(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "SANMINA_INNER") && (outerlabel == "SANMINA_OUTER"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.sanmina(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.sanmina(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "Vitesco_Inner") && (outerlabel == "Vitesco_Outer"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.vitesco(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                        if (result == "MandatoryLot")
                            return "MandatoryLot";
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.vitesco(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "abb_power_inner") && (outerlabel == "abb_power_outer"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.abb(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.abb(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "ALPS_Inner") && (outerlabel == "ALPS_OUTER"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.alps(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.alps(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }

                if ((innerlabel == "ARROW_Inner") && (outerlabel == "ARROW_Outer"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.arrowv1(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.arrowv1(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }

                if ((innerlabel == "cornell_dubilier_elect_inner") && (outerlabel == "CORNELL_DUBILIER_ELECT_OUTER"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.cornel(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.cornel(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "FLEXTRONICS_AMERICA_2") && (outerlabel == "FLEXTRONICS_AMERICA_1"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.FLEXTRONICS_AMERICA(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.FLEXTRONICS_AMERICA(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "KIMBALL")
                {
                    //if (InnerQty != null)
                    //{
                    //    string innerqtychecker = "Inner";
                    //    SqlCommand myCommand = new SqlCommand();
                    //    result = LabelGenerator.KIMBALL(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    //}
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.KIMBALL(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "HARMAN_DE_2") && (outerlabel == "HARMAN_DE_1"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.HARMAN_DE(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.HARMAN_DE(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "HITACHI_AMERICAS_Inner") && (outerlabel == "HITACHI_AMERICAS_Outer"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.HITACHI(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.HITACHI(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "HITACHI_INNER") && (outerlabel == "HITACHI_Outer"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.HITACHIASTEMO(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.HITACHIASTEMO(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "JABIL_CIRCUIT_2") && (outerlabel == "JABIL_CIRCUIT_1"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.JABILCIRCUITNorvanko(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.JABILCIRCUITNorvanko(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "LUTRON_SM_2") && (outerlabel == "LUTRON_SM_1"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.lutron(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.lutron(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "MAGNA_ELECTRONICS_2") && (outerlabel == "MAGNA_ELECTRONICS"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.Magna(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                        if (result == "MandatoryLot")
                            return "MandatoryLot";
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.Magna(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if ((innerlabel == "VISTEON_INNER") && (outerlabel == "VISTEON_OUTER"))
                {
                    if (InnerQty != null)
                    {
                        string innerqtychecker = "Inner";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.visteon(innerqtychecker, sku, Orderkey, innercount, COO, InnerQty, innerlabel, datecode, lot, wgt, printer, mfdate, lpn, orderlineno, crossdockorder);
                    }
                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.visteon(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, datecode, lot, wgt, printer, mfdate, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "DIGI_KEY_CORP")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.DIGI_KEY_CORP(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "HELLA_Electronics")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.HELLA_Electronics(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "DURA_AUTOMOTIVE")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.DURA_AUTOMOTIVE(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }
                if (outerlabel == "ZF_Active")
                {

                    if (OuterQty != null)
                    {
                        string outerqtychecker = "Outer";
                        SqlCommand myCommand = new SqlCommand();
                        result = LabelGenerator.ZF(outerqtychecker, sku, Orderkey, No_of_Labels, COO, OuterQty, outerlabel, mfdate, datecode, lot, wgt, printer, lpn, orderlineno, crossdockorder);
                    }
                }

                return result;
            }
            else
            {
                if((dt1.Rows.Count == 0) && (crossdockorder == "True"))
                {
                    return "Invalid Cross Dock Order";
                }
                else
                {
                    return "Invalid Company";
                }
                
            }

            return result;
        }
    }
}