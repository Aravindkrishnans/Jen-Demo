using LabelApp.pidsalabelGenerator;
using LabelApp.utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabelApp.Controllers
{
    public class PidsaController : Controller
    {
        // GET: Pidsa
        public ActionResult Index()
        {
            return View();
        }
        //Login Scren 
        public ActionResult Login(string UserName, string pwd)
        {

            Session["user"] = null;
            ViewBag.Message = "";
            ViewBag.Assembly = "To be Selected";

            return View();
        }
        // Login Validation from login screen
        [HttpPost]

        public JsonResult ValidateLogin(string UserName, string pwd, string FooBarDropDown)
        
        {
            var data = "";
            if (UserName.Trim() != "" && pwd.Trim() != "")
            {

                string message = "";
                validation user = new validation();
                user.ValidateUser(UserName, pwd);
                user.Assembly = FooBarDropDown;
                if (user.ValidUser)
                {
                    Session["user"] = user;
                    //FormsAuthentication.SetAuthCookie(UserName,true);
                    ViewBag.ScanAllowed = user.ScanAllowed;
                    ViewBag.ReScanAllowed = user.ReScanAllowed;
                    ViewBag.CSRAllowed = user.CSRAllowed;
                    ViewBag.ReprintAllowed = user.ReprintAllowed;
                    ViewBag.UserName = user.UserName;
                    ViewBag.mcategory = user.category;
                    data = "ValiUser";
                }
                else
                {
                    message = user.Message;
                    data = message;
                }
                
              

            }
            else
            {
                ViewBag.Message = "Username and Password are mandatory.";
                
            }
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        //Label App Common Screen

        [HttpGet]
        public ActionResult Cross()
        {
            ViewBag.Printercount = null;
            if (Session["user"] != null)
            {
                string detSql1 = "select Row_ID,Printer_Name FROM PIDSA_Printer";//Processedflag = 0";
                string con = ConfigurationManager.AppSettings["DBconnection"];                                                     //"POKEY = 0000000026";
                                                                                                                                   //"POKEY = 0000000026";
                SqlDataAdapter da1 = new SqlDataAdapter(detSql1, con);
                DataSet dsDet1 = new DataSet();
                da1.Fill(dsDet1);
                DataTable dt1 = dsDet1.Tables[0];
                ViewBag.Printercount = dt1.Rows.Count;
                foreach (DataRow row1 in dt1.Rows)
                {
                  TempData["printer" + int.Parse(row1["Row_ID"].ToString())] = row1["Printer_Name"].ToString();
                }
            }
                return View();
        }
        // Label Function Calling
        [HttpPost]

        public JsonResult getOrderrValues(string InnerQty, string sku, string OuterQty, string Orderkey, string No_of_Labels, string COO, string mfdate, string datecode, string lot, string wgt, string printer, string innercount,string crossdock,string lpn,string orderLineno)
        {
            utilityConnection dbConn = new utilityConnection();
            var data = "";
            try
            {
                if (InnerQty == "")
                    InnerQty = null;
                if (OuterQty == "")
                    OuterQty = null;
                if (((crossdock == "True") && (InnerQty != "")) || ((crossdock != "True") && (InnerQty != "")) || ((crossdock == "False") && (OuterQty != "")))
                {
               
                }
                if(sku != "")
                {
                string mQualifierchecker = sku.Substring(0, 2);
                if ((mQualifierchecker == "1p") || (mQualifierchecker == "1P"))
                {
                    sku = sku.Remove(0, 2);
                }
                }
                if(InnerQty != null)
                {
                    decimal tinnerqty = decimal.Parse(InnerQty.ToString());

                    int tinnerqty1 = Convert.ToInt32(Convert.ToDouble(tinnerqty));
                    InnerQty = tinnerqty1.ToString();
                }
              
                if(OuterQty != null)
                {
                    decimal toutterqty = decimal.Parse(OuterQty.ToString());

                    int toutterqty1 = Convert.ToInt32(Convert.ToDouble(toutterqty));
                    OuterQty = toutterqty1.ToString();
                }
              
              
               
              
                companyValidation cvalidation = new companyValidation();
                string result = cvalidation.xmlchecker(InnerQty, sku, OuterQty, Orderkey, No_of_Labels, COO, mfdate, datecode, lot, wgt, printer, innercount, crossdock, lpn, orderLineno);

                if (result == "Success")
                {
                    data = "Label Successfully generated";
                }
                else if(result == "")
                {
                    data = "1) Please check the if the item is there in the order for the orderkey that you have entered.\n";
                    data = data + "2) please check if the LPN/lineno that you have entered is there in the order for the orderkey that you have entered.\n";
                    data = data + "3) Please check if the order is picked complete";
                }
                else if(result == "Fails")
                {
                    data = "1) Please check the if the item is there in the order for the orderkey that you have entered.\n";
                    data = data + "2) please check if the LPN/lineno that you have entered is there in the order for the orderkey that you have entered.\n";
                    data = data + "3) Please check if the order is picked complete";
                }
                if (result == "Invalid Company")
                {
                    data = "Invalid Company";
                }
                if (result == "Invalid Cross Dock Order")
                {
                    data = "Please Provide Cross Dock Orders";
                }
                if(result == "Label not generated, Please contact support team")
                {
                    data = "Label not printed, Please contact IT support!";
                }
                if(result == "invalidmfdate")
                {
                    data = "This order requires Manufacture date";
                }
                if(result == "inalidweight")
                {
                    data = "This order requires Weight";
                }
                if(result == "MandatoryLot")
                {
                    data = "This order requires Lot";
                }



                //}

            }
            catch (Exception ex)
            {
                dbConn.logCapture(ex.Message.ToString(), "System", Orderkey, sku, orderLineno,lpn);
                data = "Something went wrong!";
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ItemLabel()
        {
            ViewBag.Printercount = null;
            if (Session["user"] != null)
            {
                string detSql1 = "select Row_ID,Printer_Name FROM PIDSA_Printer";//Processedflag = 0";
                string con = ConfigurationManager.AppSettings["DBconnection"];  
                                                                                        
                SqlDataAdapter da1 = new SqlDataAdapter(detSql1, con);
                DataSet dsDet1 = new DataSet();
                da1.Fill(dsDet1);
                DataTable dt1 = dsDet1.Tables[0];
                ViewBag.Printercount = dt1.Rows.Count;
                foreach (DataRow row1 in dt1.Rows)
                {
                    TempData["printer" + int.Parse(row1["Row_ID"].ToString())] = row1["Printer_Name"].ToString();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Label()
        {
            ViewBag.Printercount = null;
            if (Session["user"] != null)
            {
                string detSql1 = "select Row_ID,Printer_Name FROM PIDSA_Printer";//Processedflag = 0";
                string con = ConfigurationManager.AppSettings["DBconnection"];                                                     //"POKEY = 0000000026";
                                                                                                                                   //"POKEY = 0000000026";
                SqlDataAdapter da1 = new SqlDataAdapter(detSql1, con);
                DataSet dsDet1 = new DataSet();
                da1.Fill(dsDet1);
                DataTable dt1 = dsDet1.Tables[0];
                ViewBag.Printercount = dt1.Rows.Count;
                foreach (DataRow row1 in dt1.Rows)
                {
                    TempData["printer" + int.Parse(row1["Row_ID"].ToString())] = row1["Printer_Name"].ToString();
                }
            }
            return View();
        }

        public JsonResult getItemLabel(string sku, string qty, string nol, string printer)
        {
            var data = "";
            labelGenerator labelGenerate = new labelGenerator();

            String result = labelGenerate.getItemLabel(sku, qty, nol, printer);

            if (result == "Success")
            {
                data = "Label Successfully generated";
            }
            else
            {
                data = "Label Not printed, Please provide valid Input";
            }
            if (result == "Invalid_Company")
            {
                data = "Invalid_Company";
            }


            //}
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Home()
        {
          
            return View();
        }

        public ActionResult Report()
        {

            return View();
        }

        public JsonResult getReport(string sOrderKey)
        {

            return Json("");
        }
    }
}