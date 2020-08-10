using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Globalization;
using OpenPop.Pop3;
using OpenPop.Mime;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Net;

namespace Library
{
    public class FunctionCommon
    {
        DataLinQDataContext db = new DataLinQDataContext();

        public Boolean CheckStaffFollow(int cusID, string StaffID)
        {
            foreach (var staff in db.CHECK_STAFF_FOLLOW(StaffID))
            {
               if(cusID == staff.CustomerId)
               {
                    return true;
               }
            }
            return false;
        }
        public Boolean CheckStaffFollow(int cusID, string StaffID, string nation)
        {
            foreach (var fpA in db.LOAD_STAFFPF(cusID, "FPA", nation))
            {
                if (StaffID == fpA.STAFF_ID.ToString())
                {
                    if (fpA.FLAGCHK == "checked")
                    {
                        return true;
                    }
                   
                }
            }
            foreach (var counsultant in db.LOAD_STAFFPF(cusID, "CS", nation))
            {
                if (StaffID == counsultant.STAFF_ID.ToString())
                {
                    if (counsultant.FLAGCHK == "checked")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckBlocked_cus(string _cusId)
        {
            bool flag = false;
            foreach (var r in db._2019_CHECK_BLOCKED_CUS(Convert.ToInt32(_cusId)))
            {
                if (r.HANDLE == "HAVE")
                {
                    flag = true;
                }
            }
            return flag;
        }
        public string GetSaleFollow(int cusID)
        {
            string sales = "";
            foreach (var sale in db._2019_GET_SALES_FOLLOW(cusID))
            {
                sales += sale.STAFF_NAME + " (" + sale.TEAM_NAME + "); ";
            }
            return sales;           
        }
       
        public string GetProduct(int cusID)
        {
            string prod = "";
            foreach (var r in db._2019_LOAD_PRODUCT_STT_CUSTOMER(cusID,"ALL"))
            {
                prod += r.PROD_NAME + "; ";
            }
            return prod;
        }

        public string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public string Encrypt(string text)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(text.Trim(), "SHA1");
        }

        public string NVL(object obj)
        {
            string revalue = "";
            if (obj != null)
            {
                revalue = obj.ToString();
            }
            else
            {
                revalue = "";
            }
            return revalue;
        }

        /*frank code*/
        //Function add ' to string
        public string Addsymbol(string inObject)
        {
            if (inObject.ToString().Trim() != "")
            {
                var symbol = "'";
                inObject = symbol + inObject + symbol;
            }
            return inObject;
        }
        public string ConvertToUnSign(string text)
        {
            for (int i = 32; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), " ");
            }
            text = text.Replace(".", "-");
            text = text.Replace(" ", "-");
            text = text.Replace(",", "-");
            text = text.Replace(";", "-");
            text = text.Replace(":", "-");
            text = text.Replace("?", "-");
            text = text.Replace("!", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }

        public string ConvertNameTolower(string text)
        {
            if (text != "" || text != null)
            {
                for (int i = 32; i < 48; i++)
                {
                    text = text.Replace(((char)i).ToString(), " ");
                }
                Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
                string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
                return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
            }
            else
                return "";
        }

        public int checkEmailStaff(string staffemail)
        {
            var qry = (from x in db.M_STAFFs
                       where x.STAFF_EMAIL == staffemail
                       select x).Count();
            return qry;
        }
        public int checkClassification(string code1, string code2)
        {
            var qry = (from x in db.M_CLASSIFICATIONs
                       where x.CLASS_CODE1 == code1 && x.CLASS_CODE2 == code2
                       select x).Count();
            return qry;
        }

        public int checkPartner(string emailpass)
        {
            var qry = (from x in db.M_PARTNERs
                       where x.PARTNER_EMAIL == emailpass
                       select x).Count();
            return qry;
        }

        public int checkEvent(string eventcode, string eventname)
        {
            var qry = (from x in db.M_EVENTs
                       where x.EVENT_CODE2 == eventcode && x.EVENT_NAME == eventname
                       select x).Count();
            return qry;
        }
        public string EncryptPassword64(string txtPassword)
        {
            byte[] passBytes = System.Text.Encoding.Unicode.GetBytes(txtPassword);
            string encryptPassword = Convert.ToBase64String(passBytes);
            return encryptPassword;
        }

        public string DecryptPassword64(string encryptedPassword)
        {
            byte[] passByteData = Convert.FromBase64String(encryptedPassword);
            string originalPassword = System.Text.Encoding.Unicode.GetString(passByteData);
            return originalPassword;
        }

        public string ConvertArray(string text)
        {
            text = text.Replace(",", ";");
            text = text.Replace(" ", "");
            text = text.Replace("/", ";");
            text = text.Replace(";;", ";");
            return text.ToLower();
        }

        public void SaveUrl(string url)
        {
            HttpContext.Current.Response.Cookies["TAG_URL"].Value = HttpContext.Current.Server.UrlEncode(url);
            HttpContext.Current.Response.Cookies["TAG_URL"].Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Redirect("/default.aspx");
        }
        public string GetUrl()
        {
            string url = "/main/intro.aspx";
            try
            {
                url = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["TAG_URL"].Value);
                //HttpContext.Current.Response.Cookies["TAG_URL"].Expires = DateTime.Now.AddDays(-1);
                return url;
            }
            catch (Exception)
            {
                HttpContext.Current.Response.Cookies["TAG_URL"].Value = HttpContext.Current.Server.UrlEncode(url);
                HttpContext.Current.Response.Cookies["TAG_URL"].Expires = DateTime.Now.AddDays(1);
                return url;
            }
        }

        public string ReplyWhenCusMakeSurveyFormDG(
           string Cus_Name,
           string Cus_Sex,
           string Content,
           string Head_content
           )
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            string path = "~/email/template/FormDanhGiaTrucTuyen.htm";

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{SEX_L}", Cus_Sex);
            body = body.Replace("{NAME}", Cus_Name);
            body = body.Replace("{CONTENT}", Content);
            body = body.Replace("{HEAD_CONTENT}", Head_content);
            return body;
        }

        //Se delete sau khi xong het cac form khao sat chat luong
        public string AlertWhenCusMakeSurveyCP(
          string Cus_Id,
          string nv_TLHS,
          string Cus_Name,
          string Content
          )
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            string path = "~/email/template/FormDanhGiaCaseProcessing.htm";

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CUS_ID}", Cus_Id);
            body = body.Replace("{CUS_NAME}", Cus_Name);
            body = body.Replace("{CONTENT}", Content);
            body = body.Replace("{NVTLHS}", nv_TLHS);
            return body;
        }

        public string AlertWhenCusMakeSurvey(
         string _Cus_Id,
         string _Tit_nv,
         string _Nv,
         string _Cus_Name,
         string _Content
         )
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            string path = "~/email/template/FormKSCL_DichVu.htm";

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CUS_ID}", _Cus_Id);
            body = body.Replace("{CUS_NAME}", _Cus_Name);
            body = body.Replace("{CONTENT}", _Content);
            body = body.Replace("{TITLE_NV}", _Tit_nv);
            body = body.Replace("{NV}", _Nv);
            return body;
        }

        public string SendTemplate(string this_tempid,
                                        string this_cus,
                                        string this_staff,
                                        string cussex,
                                        string cussexup,
                                        string product,
                                        string prod_other1,
                                        string prod_other2,
                                        string prod_other3,
                                        string sales,
                                        string signature)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/sendwmtemplate"+ this_tempid +".htm")))
            {
                body = reader.ReadToEnd();
            }           
            body = body.Replace("{CUS_SEX}", cussex);
            body = body.Replace("{CUS_NAME}", this_cus);
            body = body.Replace("{STAFF_NAME}", this_staff);
            body = body.Replace("{SALES}", sales);
            body = body.Replace("{DICHVU}", product);
            body = body.Replace("{PROD_OTHER1}", prod_other1);
            body = body.Replace("{PROD_OTHER2}", prod_other2);
            body = body.Replace("{PROD_OTHER3}", prod_other3);
            body = body.Replace("{SIGNATURE}", signature);
            return body;
        }

        public string SendTemplateIBT(string this_tempid,
                                        string this_cus,
                                        string this_staff,
                                        string this_staff_name,
                                        string this_staff_phone,
                                        string this_staff_email,
                                        string cussex,
                                        string cussexup,                                      
                                        string signature)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/sendibttemplate" + this_tempid + ".htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CUS_SEX}", cussex);
            body = body.Replace("{CUS_NAME}", this_cus);
            body = body.Replace("{THIS_NAME}", this_staff);
            body = body.Replace("{STAFF_NAME}", this_staff_name);
            body = body.Replace("{STAFF_PHONE}", this_staff_phone);
            body = body.Replace("{STAFF_EMAIL}", this_staff_email);
            body = body.Replace("{SIGNATURE}", signature);
            return body;
        }

        public string SendTemplateSV(string this_tempid,
                                        string this_cus,
                                        string this_cus_id,
                                        string this_staff,
                                        string this_staff_email,
                                        string this_staff_fpa,
                                        string this_staff_sale,
                                        string this_result,
                                        string cussex,
                                        string signature)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/survey" + this_tempid + ".htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CUS_SEX}", cussex);
            body = body.Replace("{CUS_NAME}", this_cus);
            body = body.Replace("{CUS_ID}", this_cus_id);
            body = body.Replace("{THIS_STAFF}", this_staff);
            body = body.Replace("{THIS_STAFF_EMAIL}", this_staff_email);
            body = body.Replace("{FPA_STAFF}", this_staff_fpa);
            body = body.Replace("{SALE_STAFF}", this_staff_sale);
            body = body.Replace("{THIS_RESULT}", this_result);
            body = body.Replace("{SIGNATURE}", signature);
            return body;
        }

        public string SentRemind(string CusName, string CusID, string CusSex, string DateRemind, string GiaiDoan, string proDuct)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/sendRemind.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CusSex}", CusSex);
            body = body.Replace("{proDuct}", proDuct);
            body = body.Replace("{CusName}", CusName);
            body = body.Replace("{GiaiDoan}", GiaiDoan);
            body = body.Replace("{DateRemind}", DateRemind);
            body = body.Replace("{CusID}", CusID);
            return body;
        }

        public string SentImmCode(string app_pass)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/sendimmcode.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{IMMCODE}", app_pass);
            return body;
        }

        public string SentInfor(string CusName, string CusEmail, string CusSex, string CusSexUp, string CusPhone, string StaffSign)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/sentinforcus.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CusSex}", CusSex);
            body = body.Replace("{CusSexUp}", CusSexUp);
            body = body.Replace("{UserName}", CusName);
            body = body.Replace("{Time}", mday);
            body = body.Replace("{CusEmail}", CusEmail);
            body = body.Replace("{CusPhone}", CusPhone);
            body = body.Replace("{Signature}", StaffSign);
            return body;
        }

        public string Notification_Staff_Change(string mode, string CusId, string NameStaff_Remove, string NameStaff_Be_Remove)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            string path = "";
            if(mode == "Remove")
            {
                path = "~/email/template/NotiRemoveStaff.htm";
            }
            if (mode == "Add")
            {
                path = "~/email/template/NotiAddStaff.htm";
            }
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{namestaff_remove}", NameStaff_Remove);
            body = body.Replace("{CUS_ID}", CusId);
            return body;
        }

        public string NotiCusInfoChange( 
            string CusId, 
            string NameStaff_Edit,
            string Cus_Name,
            string Cus_Email,
            string Cus_Phone,
            string Cus_Prod)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            string path = "~/email/template/NotiCusInfoChange.htm";
           
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{NameStaff_Edit}", NameStaff_Edit);
            body = body.Replace("{CUS_ID}", CusId);
            body = body.Replace("{CUS_NAME}", Cus_Name);
            body = body.Replace("{CUS_EMAIL}", Cus_Email);
            body = body.Replace("{CUS_PHONE}", Cus_Phone);
            body = body.Replace("{PROD}", Cus_Prod);
            return body;
        }

        public string NotiFileManager(
                string staffname,
                string cusid,
                string cusname,
                string text)
        {
            string body = string.Empty;
            string path = "~/email/template/NotiFileManager.htm";

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                body = reader.ReadToEnd();
            }
            text = text.Replace("{", "");
            text = text.Replace("}", "");
            text = text.Replace("Event arguments:", "");
            text = text.Replace("\"eventName\"", "Thao tác");
            text = text.Replace("\"item\"", "Thao tác trên");
            text = text.Replace("\"items\"", "Thao tác trên");
            text = text.Replace("\"name\"", "Tên");
            text = text.Replace("\"fullPath\"", "Đường dẫn");
            text = text.Replace("\"itemType\"", "Loại");
            text = text.Replace("\"extension\"", "Phần mở rộng");
            text = text.Replace("\"itemNewName\"", "Thay đổi thành");
            body = body.Replace("{NameStaff}", staffname);
            body = body.Replace("{CUS_ID}", cusid);
            body = body.Replace("{CUS_NAME}", cusname);
            body = body.Replace("{TEXT}", text);
            return body;
        }

        public string SentContact(string CusName, string CusEmail, string CusSex, string CusSexUp, string CusPhone, string content, string linkfile)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            content = content.Replace("<p>", "");
            content = content.Replace("</p>", "");
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/addnote.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CusSex}", CusSex);
            body = body.Replace("{CusSexUp}", CusSexUp);
            body = body.Replace("{UserName}", CusName);
            body = body.Replace("{Time}", mday);
            body = body.Replace("{CusEmail}", CusEmail);
            body = body.Replace("{CusPhone}", CusPhone);
            body = body.Replace("{ContentNote}", content);
            if (linkfile != "")
            {
                linkfile = "<br/> Tài liệu đính kèm: " + linkfile;
            }
            body = body.Replace("{FileAttach}", linkfile);
            return body;
        }


        public string SentPass(string CusEmail, string Pass)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/forgotpass.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{USER_EMAIL}", CusEmail);
            body = body.Replace("{USER_PW}", Pass);
            return body;
        }
        public string SentPassCUS(string CusEmail, string Pass)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/forgotpasscus.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{MATKHAU}", Pass);
            return body;
        }


        public string SentInforAgent(string name, string email)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/sendinforAgent.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{AgentName}", name);
            return body;
        }

        public string SentMesPartner(string PartName,string CusID, string CusName)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/addnotetopart.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CusID}", CusID);
            body = body.Replace("{CusName}", CusName);
            body = body.Replace("{UserName}", PartName);
            body = body.Replace("{Time}", mday);
            return body;
        }

        public string SentEmailPrivate(string CusID, string UserName, string CateNote, string content, string staffName, string linkfile)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            content = content.Replace("<p>", "");
            content = content.Replace("</p>", "");
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/addnoteprivate.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", UserName);
            body = body.Replace("{CateNote}", CateNote);
            body = body.Replace("{Time}", mday);
            body = body.Replace("{CusID}", CusID);
            body = body.Replace("{ContentNote}", content);
            body = body.Replace("{staffName}", staffName);
            if (linkfile != "")
            {
                linkfile = "<br/> Tài liệu đính kèm: " + linkfile;
            }
            body = body.Replace("{FileAttach}", linkfile);
            return body;
        }

        public string SentEmailFee(string CusID, string UserName, string content)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/addnotefee.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", UserName);
            body = body.Replace("{Time}", mday);
            body = body.Replace("{CusID}", CusID);
            body = body.Replace("{ContentNote}", content);
            return body;
        }

        public string SentEmailFeedback(string CusID,string CusName, string StaffName)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/addfeedback.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Time}", mday);
            body = body.Replace("{CusID}", CusID);
            body = body.Replace("{CusName}", CusName);
            body = body.Replace("{StaffName}", StaffName);
            return body;
        }

        public string PartnerAddMess(string CusID, string CusName, string StaffName)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/partneraddmess.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Time}", mday);
            body = body.Replace("{CusID}", CusID);
            body = body.Replace("{CusName}", CusName);
            body = body.Replace("{StaffName}", StaffName);
            return body;
        }

        public string SentAssignLeads(string salesleads)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/sendAssignLeads.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{LIST_LEADS}", salesleads);
           
            return body;
        }

        public string CECSentRebackLeads(string salesleads)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/CECSentRebackLeads.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{LIST_LEADS}", salesleads);

            return body;
        }

        public string SentSearchAssignLeads(string salesleads, string note)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/SentSearchAssignLeads.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{LIST_LEADS}", salesleads);
            body = body.Replace("{NOTE}", note);

            return body;
        }

        public string SentRebackLeads(string CusId, string Staff)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/sendRebackLeads.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{SALE_LEADS}", CusId);
            body = body.Replace("{STAFF_NAME}", Staff);

            return body;
        }

        
        public string SentReDelLeads(string CusId, string Staff)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/email/template/sendReDelLeads.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{SALE_LEADS}", CusId);
            body = body.Replace("{STAFF_NAME}", Staff);

            return body;
        }

        public void ClearCustomer()
        {
            Customer.CUS_ID = null;
            Customer.STATUS_ID = null;
            Customer.CUS_NAME_VN = null;
            Customer.CUS_NAME_ENG = null;
            Customer.CUS_EMAIL = null;
            Customer.CUS_PHONE = null;
            Customer.CUS_BIRTH = null;
            Customer.CUS_BIRTH_FULL = DateTime.Parse("01/01/1900");
            Customer.CUS_SEX = null;
            Customer.CUS_MARITAL = null;
            Customer.CUS_ADDRESS = null;
            Customer.CUS_TOTAL_ASSET = null;
            Customer.CUS_PASS = null;
            Customer.CUS_REVIEW = null;
            Customer.NOTE = null;
            Customer.FLAG_ACTIVE = null;
            Customer.CUS_VIP_CODE = null;
            Customer.PARTNER_ID = null;
            Customer.CUS_CHILDREN = null;
            Customer.CUS_RELATIVES = null;
            Customer.CUS_RELATIVES_FOREIGN = null;
            Customer.CUS_RESIDING_ABROAD = null;
            Customer.CUS_APPLIED = null;
            Customer.CUS_ID_OUT = null;
        }

        public void ClearEmaildata()
        {
            EMAIL_data.EMAIL_HEADER = null;
            EMAIL_data.EMAIL_FROM = null;
            EMAIL_data.EMAIL_TO = null;
            EMAIL_data.EMAIL_CC = null;
            EMAIL_data.EMAIL_BCC = null;
            EMAIL_data.EMAIL_SUBJECT = null;
            EMAIL_data.EMAIL_DATE = DateTime.Now;
            EMAIL_data.EMAIL_CONTENT = null;
            EMAIL_data.STAFF_EMAIL = null;
            EMAIL_data.FLAG_ACTIVE = null;
            EMAIL_data.EMAIL_ID_OUT = null;
            EMAIL_data.EMAIL_ID = null;
            EMAIL_data.CUS_ID = null;          
        }

        public void ClearMarInfor()
        {
            Marketing.MAR_ID = null;
            Marketing.CUS_ID = null;
            Marketing.NATION_ID = null;
            Marketing.MAR_CONTACT_DATE = null;
            Marketing.MAR_MEET_CUS_DATE = null;
            Marketing.MAR_CONTRACT_DATE = null;
            Marketing.MAR_VISA_DATE = null;
            Marketing.MAR_CUS_EVALUATION = null;
            Marketing.MAR_CUS_CONTACT_METHOD = null;
            Marketing.MAR_CUS_SOURCE = null;
            Marketing.MAR_CUS_SOURCE_NOTE = null;
            Marketing.MAR_CUS_INTEREST = null;
            Marketing.MAR_PARTNER_ID = null;
            Marketing.MAR_VIP_CARD = null;
            Marketing.MAR_CUS_WARNING = null;
            Marketing.MAR_OFFICE = null;
            Marketing.FLAG_ACTIVE = null;
        }

        public void ClearRelationship()
        {
            Relationship.CUS_ID = null;
            Relationship.RS_CODE1 = null;
            Relationship.RS_CODE2 = null;
            Relationship.RS_NOTE = null;
            Relationship.RS_PREPARED1 = null;
            Relationship.RS_PREPARED2 = null;
        }

        public DateTime Convertbrd(string indate)
        {
            DateTime fullbrd = DateTime.Parse("01/01/1900");
            try
            {
                indate = indate.Substring(3,2) + "/" + indate.Substring(0,2) + "/" + indate.Substring(6,4);
                DateTime dt = DateTime.Parse(indate);
                return dt;
            }
            catch(Exception)
            {

            }
            return fullbrd;    
        }

    }

    public class RegexUtilities
    {
        bool invalid = false;

        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None);
            }
            catch (Exception)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
              return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }   
    }
    public class EmailScan
    {
        DataLinQDataContext db = new DataLinQDataContext();
        public void EmailPOPAll()
        {
            Pop3Client pop3Client;
            pop3Client = new Pop3Client();
            pop3Client.Connect("pop.gmail.com", 995, true);
            var qryUser = (from x in db.M_STAFFs
                           where (x.STAFF_PASS_EMAIL != null && x.STAFF_PASS_EMAIL != "")
                           select new
                           {
                               emailUser = x.STAFF_EMAIL,
                               passUser = x.STAFF_PASS_EMAIL
                           });
            foreach (var a in qryUser)
            {
                try
                {
                    string emailInput = a.emailUser;
                    string passwd = DecryptPassword64(a.passUser);
                    pop3Client.Authenticate("recent:" + emailInput, passwd, AuthenticationMethod.UsernameAndPassword);
                    int count = pop3Client.GetMessageCount();
                    int counter = 0;
                    for (int i = count; i >= 1; i--)
                    {
                        Message message = pop3Client.GetMessage(i);
                        MessagePart body = message.FindFirstHtmlVersion();
                        string nameEmail = message.Headers.From.DisplayName;
                        string fromEmail = message.Headers.From.Address;
                        string emailSubject = message.Headers.Subject;
                        DateTime emailDateSent = message.Headers.DateSent.ToLocalTime();
                        string listto = null;
                        string cc = null;
                        string bcc = null;
                        for (int c = 0; c < message.Headers.To.Count; c++)
                        {
                            if (message.Headers.To[c].MailAddress != null)
                            {
                                listto += message.Headers.To[c].MailAddress.Address + ";";
                            }
                        }
                        for (int c = 0; c < message.Headers.Cc.Count; c++)
                        {
                            if (message.Headers.Cc[c].MailAddress != null)
                            {
                                cc += message.Headers.Cc[c].MailAddress.Address + "; ";
                            }
                        }
                        for (int c = 0; c < message.Headers.Bcc.Count; c++)
                        {
                            if (message.Headers.Bcc[c].MailAddress != null)
                            {
                                bcc += message.Headers.Bcc[c].MailAddress.Address + "; ";
                            }
                        }
                        string emailBody = null;
                        if (body != null)
                        {
                            emailBody = body.GetBodyAsText();
                        }
                        else
                        {
                            body = message.FindFirstPlainTextVersion();
                            if (body != null)
                            {
                                emailBody = body.GetBodyAsText();
                            }
                        }
                        try
                        {
                            string cusid = "";
                            foreach (var row in db.GET_ID_CUS_SCANMAIL(fromEmail))
                            {
                                cusid = row.CUS_ID.ToString();
                            }
                            string counts = "";
                            foreach (var row in db.checkEmailPOP(emailDateSent, emailSubject))
                            {
                                counts = row.E_COUNT.ToString();
                            }
                            //Not email and from email is staff
                            if (counts == "0" && cusid != "" && fromEmail != emailInput)
                            {
                                //db.insertEmailPOP(qryid.idcus, nameEmail, fromEmail, cc, bcc, emailSubject, emailDateSent, ConvertBodyEmail(emailBody), emailInput, listto);
                                int id;
                                foreach (var row in db.EMAIL_HANDLING(WebProfile.g_mode_insert, 99999,
                                                                        Convert.ToInt32(cusid), nameEmail, fromEmail, 
                                                                        listto, cc, bcc, emailSubject, 
                                                                        emailDateSent, ConvertBodyEmail(emailBody), 
                                                                        emailInput, 0, message.Headers.MessageId))
                                {
                                    id =Convert.ToInt32(row.SCOPE_IDENTITY.ToString());
                                }
                           
                                string nameFile = null;
                                string fileType = null;
                                byte[] emailAttach = null;
                                List<MessagePart> attachments = message.FindAllAttachments();
                                foreach (MessagePart attachment in attachments)
                                {
                                    nameFile = attachment.FileName.Replace(",", "");
                                    fileType = attachment.ContentType.MediaType;
                                    emailAttach = attachment.Body;
                                    //string strdocPath;
                                    //strdocPath = Server.MapPath("~/upload/email/19/" + nameFile);
                                    //FileStream objfilestream = new FileStream(strdocPath, FileMode.Create, FileAccess.ReadWrite);
                                    //objfilestream.Write(emailAttach, 0, emailAttach.Length);
                                    //objfilestream.Close();
                                    
                                    //Response.Clear();
                                    //Response.Buffer = true;
                                    //Response.Charset = "";
                                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    //Response.ContentType = contentType;
                                    ////Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                                    ////Response.BinaryWrite(bytes);
                                    //File.WriteAllBytes(Server.MapPath("~/upload/email/19/" + fileName), bytes);
                                    ////Response.Flush();
                                    //Response.End();
                                    //db.insertAttachPOP(id, nameFile, fileType, emailAttach);
                                }
                            }
                                //Email chưa có , 
                            else if (counts == "0" && cusid == "" && fromEmail != emailInput)
                            {
                                int idcode = 0;
                                for (int c = 0; c < message.Headers.To.Count; c++)
                                {
                                    if (message.Headers.To[c].MailAddress != null)
                                    {
                                        var qryidto = (from x in db.M_CUSTOMERs
                                                       where x.CUS_EMAIL.Contains(message.Headers.To[c].MailAddress.Address) && x.FLAG_ACTIVE == 1 //&& !String.IsNullOrEmpty(x.CUS_EMAIL)
                                                       select new
                                                       {
                                                           idcus = x.CUS_ID
                                                       }).FirstOrDefault();
                                        if (qryidto != null)
                                        {
                                            idcode = qryidto.idcus;
                                            break;
                                        }
                                    }
                                }
                                if (idcode == 0)
                                {
                                    for (int c = 0; c < message.Headers.Cc.Count; c++)
                                    {
                                        if (message.Headers.Cc[c].MailAddress != null)
                                        {
                                            var qryidcc = (from x in db.M_CUSTOMERs
                                                           where x.CUS_EMAIL.Contains(message.Headers.Cc[c].MailAddress.Address) && x.FLAG_ACTIVE == 1 //&& !String.IsNullOrEmpty(x.CUS_EMAIL)
                                                           select new
                                                           {
                                                               idcus = x.CUS_ID
                                                           }).FirstOrDefault();
                                            if (qryidcc != null)
                                            {
                                                idcode = qryidcc.idcus;
                                                break;
                                            }
                                        }
                                    }
                                    if (idcode == 0)
                                    {
                                        var qryidfw = (from x in db.M_CUSTOMERs
                                                       join y in db.M_EMAILs on x.CUS_ID equals y.CUS_ID
                                                       where y.EMAIL_FROM == emailInput && ("fw: " + y.EMAIL_SUBJECT.ToLower() == emailSubject.ToLower() || "fwd: " + y.EMAIL_SUBJECT.ToLower() == emailSubject.ToLower() || "forward: " + y.EMAIL_SUBJECT.ToLower() == emailSubject.ToLower()) && x.FLAG_ACTIVE == 1
                                                       orderby y.EMAIL_DATE descending
                                                       select new
                                                       {
                                                           idcus = x.CUS_ID
                                                       }).FirstOrDefault();
                                        if (qryidfw != null)
                                        {
                                            idcode = qryidfw.idcus;
                                        }
                                    }
                                }
                                if (idcode != 0)
                                {                                    
                                    int id;
                                    foreach (var row in db.EMAIL_HANDLING(WebProfile.g_mode_insert, 99999,
                                                                            idcode, nameEmail, fromEmail,
                                                                            listto, cc, bcc, emailSubject,
                                                                            emailDateSent, ConvertBodyEmail(emailBody),
                                                                            emailInput, 0, message.Headers.MessageId))
                                    {
                                        id = Convert.ToInt32(row.SCOPE_IDENTITY.ToString());
                                    }
                                    string nameFile = null;
                                    string fileType = null;
                                    byte[] emailAttach = null;
                                    List<MessagePart> attachments = message.FindAllAttachments();
                                    foreach (MessagePart attachment in attachments)
                                    {
                                        nameFile = attachment.FileName.Replace(",", "");
                                        fileType = attachment.ContentType.MediaType;
                                        emailAttach = attachment.Body;
                                        //db.insertAttachPOP(id, nameFile, fileType, emailAttach);
                                    }
                                }
                                else
                                {
                                    string stemp = emailSubject.ToLower();
                                    if (stemp.Contains("mkh") == true)
                                    {
                                        bool isNumeric = true;
                                        int position = stemp.LastIndexOf("mkh") + 3;
                                        string stempCode = stemp.ToString().Substring(position).Trim();
                                        Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
                                        isNumeric = regex.IsMatch(stempCode);
                                        if (isNumeric == true)
                                        {
                                            int countcus = 0;
                                            foreach (var row in db.CHECK_EXITCUS(int.Parse(stempCode)))
                                            {
                                                countcus =Convert.ToInt32(row.CUSCOUNT);
                                            }
                                            if (countcus == 1)
                                            {
                                                idcode = int.Parse(stempCode);                                                
                                                int id;
                                                foreach (var row in db.EMAIL_HANDLING(WebProfile.g_mode_insert, 99999,
                                                                                        idcode, nameEmail, fromEmail,
                                                                                        listto, cc, bcc, emailSubject,
                                                                                        emailDateSent, ConvertBodyEmail(emailBody),
                                                                                        emailInput, 0, message.Headers.MessageId))
                                                {
                                                    id = Convert.ToInt32(row.SCOPE_IDENTITY.ToString());
                                                }
                                                string nameFile = null;
                                                string fileType = null;
                                                byte[] emailAttach = null;
                                                List<MessagePart> attachments = message.FindAllAttachments();
                                                foreach (MessagePart attachment in attachments)
                                                {
                                                    nameFile = attachment.FileName.Replace(",", "");
                                                    fileType = attachment.ContentType.MediaType;
                                                    emailAttach = attachment.Body;
                                                    //db.insertAttachPOP(id, nameFile, fileType, emailAttach);
                                                }
                                            }
                                        }
                                    }
                                    else if (stemp.Contains("code") == true)
                                    {
                                        bool isNumeric = true;
                                        int position = stemp.LastIndexOf("code") + 4;
                                        string stempCode = stemp.ToString().Substring(position).Trim();
                                        Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
                                        isNumeric = regex.IsMatch(stempCode);
                                        if (isNumeric == true)
                                        {
                                            int countcus = 0;
                                            foreach (var row in db.CHECK_EXITCUS(int.Parse(stempCode)))
                                            {
                                                countcus = Convert.ToInt32(row.CUSCOUNT);
                                            }
                                            if (countcus == 1)
                                            {
                                                idcode = int.Parse(stempCode);
                                                int id;
                                                foreach (var row in db.EMAIL_HANDLING(WebProfile.g_mode_insert, 99999,
                                                                                        idcode, nameEmail, fromEmail,
                                                                                        listto, cc, bcc, emailSubject,
                                                                                        emailDateSent, ConvertBodyEmail(emailBody),
                                                                                        emailInput, 0, message.Headers.MessageId))
                                                {
                                                    id = Convert.ToInt32(row.SCOPE_IDENTITY.ToString());
                                                }
                                                string nameFile = null;
                                                string fileType = null;
                                                byte[] emailAttach = null;
                                                List<MessagePart> attachments = message.FindAllAttachments();
                                                foreach (MessagePart attachment in attachments)
                                                {
                                                    nameFile = attachment.FileName.Replace(",", "");
                                                    fileType = attachment.ContentType.MediaType;
                                                    emailAttach = attachment.Body;
                                                    //db.insertAttachPOP(id, nameFile, fileType, emailAttach);
                                                }
                                            }
                                        }
                                    }
                                    else if (stemp.Contains("file") == true)
                                    {
                                        bool isNumeric = true;
                                        int position = stemp.LastIndexOf("file") + 4;
                                        string stempCode = stemp.ToString().Substring(position).Trim();
                                        Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
                                        isNumeric = regex.IsMatch(stempCode);
                                        if (isNumeric == true)
                                        {
                                            int countcus = 0;
                                            foreach (var row in db.CHECK_EXITCUS(int.Parse(stempCode)))
                                            {
                                                countcus = Convert.ToInt32(row.CUSCOUNT);
                                            }
                                            if (countcus == 1)
                                            {
                                                idcode = int.Parse(stempCode);
                                                int id;
                                                foreach (var row in db.EMAIL_HANDLING(WebProfile.g_mode_insert, 99999,
                                                                                        idcode, nameEmail, fromEmail,
                                                                                        listto, cc, bcc, emailSubject,
                                                                                        emailDateSent, ConvertBodyEmail(emailBody),
                                                                                        emailInput, 0, message.Headers.MessageId))
                                                {
                                                    id = Convert.ToInt32(row.SCOPE_IDENTITY.ToString());
                                                }
                                                string nameFile = null;
                                                string fileType = null;
                                                byte[] emailAttach = null;
                                                List<MessagePart> attachments = message.FindAllAttachments();
                                                foreach (MessagePart attachment in attachments)
                                                {
                                                    nameFile = attachment.FileName.Replace(",", "");
                                                    fileType = attachment.ContentType.MediaType;
                                                    emailAttach = attachment.Body;
                                                    //db.insertAttachPOP(id, nameFile, fileType, emailAttach);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        counter++;
                        if (counter > 10)
                        {
                            break;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
                
        public static string EncryptPassword64(string txtPassword)
        {
            byte[] passBytes = System.Text.Encoding.Unicode.GetBytes(txtPassword);
            string encryptPassword = Convert.ToBase64String(passBytes);
            return encryptPassword;
        }
        public static string DecryptPassword64(string encryptedPassword)
        {
            byte[] passByteData = Convert.FromBase64String(encryptedPassword);
            string originalPassword = System.Text.Encoding.Unicode.GetString(passByteData);
            return originalPassword;
        }

        public static string ConvertBodyEmail(string text)
        {
            text = text.Replace("&lt;", "<");
            text = text.Replace("&gt;", ">");
            text = text.Replace("<;", "<");
            text = text.Replace("<base target=\"_self\" href=\"https://e-aj.my.com/\">", "");
            for (int i = 0; i < 10; i++)
            {
                int stempfirst = text.IndexOf("<!--");
                int stemplast = text.IndexOf("-->");
                if (stempfirst != -1)
                {
                    text = text.Remove(stempfirst, stemplast - stempfirst + 3);
                }
            }
            return text;
        }

    }

    
}
