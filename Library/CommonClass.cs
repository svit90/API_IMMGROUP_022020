using Microsoft.Net.Http.Headers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Library
{
    public class CommonConstants
    {
        public static int ADD = 1;
        public static int UPD = 2;
        public static int DEL = 3;

        public static string MEMBER_GROUP = "AUTHSTAF";
        public static string ADMIN_GROUP = "AUTHADM";
        public static string MOD_GROUP = "AUTHMOD";

        public static string USER_SESSION = "USER_SESSION";
        public static string SESSION_CREDENTIALS = "SESSION_CREDENTIALS";
        public static string CREDENTIALS = "CREDENTIALS";
        public static string CartSession = "CartSession";

        public const string initVector = "h7g3e4m3t5st5zjw";
        public const int keysize = 256;

        public static string CurrentCulture { set; get; }
    }   
    public class CookieHelper {

        public static void CreateCookie(string CookieName,
                                        string KeyName ,
                                        string CookieValue,
                                        DateTime? expirationDate)
        {
            System.Web.HttpCookie cookie = HttpContext.Current.Response.Cookies.AllKeys.Contains(CookieName)
                ? HttpContext.Current.Response.Cookies[CookieName]
                : HttpContext.Current.Request.Cookies[CookieName];
            if (cookie == null)
            {
                cookie = new System.Web.HttpCookie(CookieName);
                cookie.Values.Add(KeyName, CookieValue);
                cookie.Expires = expirationDate.Value;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }               
        }

        public static void CreateCookie(string CookieName,
                                        string CookieValue,
                                        DateTime? expirationDate)
        {
            System.Web.HttpCookie cookie = HttpContext.Current.Response.Cookies.AllKeys.Contains(CookieName)
                ? HttpContext.Current.Response.Cookies[CookieName]
                : HttpContext.Current.Request.Cookies[CookieName];
            if (cookie == null)
            {
                cookie = new System.Web.HttpCookie(CookieName);
                cookie.Value = CookieValue;
                cookie.Expires = expirationDate.Value;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void CreateCookie(object obj,
                                       DateTime? expirationDate)
        {            
            // cast to an IDictionary<string, object>
            IDictionary<object, object> expDict = (IDictionary<object, object>)obj;
            System.Web.HttpCookie cookie = HttpContext.Current.Response.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName)
                ? HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName]
                : HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null)
            {
                foreach (KeyValuePair<object, object> kvp in expDict)
                {
                    cookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName);
                    cookie.Values.Add(kvp.Key.ToString(), kvp.Value.ToString());
                    cookie.Expires = expirationDate.Value;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }
        public static string GetCookie(string CookieName, string KeyName)
        {
            System.Web.HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie != null)
            {
                string val = (!String.IsNullOrEmpty(KeyName)) ? cookie[KeyName] : cookie.Value;
                if (!String.IsNullOrEmpty(val)) return Uri.UnescapeDataString(val);
            }
            return null;
        }

        public static string GetCookie(string CookieName)
        {
            System.Web.HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie != null)
            {
                string val = cookie.Value;
                if (!String.IsNullOrEmpty(val)) return Uri.UnescapeDataString(val);
            }
            return null;
        }

        public static void RemoveCookie(string keyName)
        {
            if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                System.Web.HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (String.IsNullOrEmpty(keyName))
                {
                    cookie.Expires = DateTime.UtcNow.AddYears(-1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
                }
                else
                {
                    cookie.Values.Remove(keyName);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }


        public static bool CookieExist(string CookieName)
        {         
            HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;           
            if(cookies[CookieName] != null)
            {
                return true;
            }
            return false;
        }

        public static void ClearCookie()
        {
            string[] myCookies = HttpContext.Current.Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
    public static class HashString
    {
        public static string EncryptString(string stringToSecret)
        {
            byte[] passBytes = System.Text.Encoding.Unicode.GetBytes(stringToSecret);
            string EncryptedString = Convert.ToBase64String(passBytes);
            return EncryptedString;
        }
        public static string DecryptString(string EncryptedString)
        {
            byte[] passByteData = Convert.FromBase64String(EncryptedString);
            string originalString = System.Text.Encoding.Unicode.GetString(passByteData);
            return originalString;
        }
    }
    public class Function
    {

        /// Encrypt - Decrypt String Function
        public string Encrypt(string text)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(text.Trim(), "SHA1");
#pragma warning restore CS0618 // Type or member is obsolete
        }
        public string EncryptString(string stringToSecret)
        {
            byte[] passBytes = System.Text.Encoding.Unicode.GetBytes(stringToSecret);
            string EncryptedString = Convert.ToBase64String(passBytes);
            return EncryptedString;
        }
        public string DecryptString(string EncryptedString)
        {
            byte[] passByteData = Convert.FromBase64String(EncryptedString);
            string originalString = System.Text.Encoding.Unicode.GetString(passByteData);
            return originalString;
        }
        public string EncryptUtf8(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public string DecryptUtf8(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public string ConvertArray(string text)
        {
            text = text.Replace(",", ";");
            text = text.Replace(" ", "");
            text = text.Replace("/", ";");
            text = text.Replace(";;", ";");
            return text.ToLower();
        }

        public string ConvertLinkUtf8(string text)
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
        public string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }
        public string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes);
        }
        public string ConvertName(string text)
        {
            string name = "";
            for (int i = 32; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            name = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
            return name;
        }
        public DateTime Convertbrd(string indate)
        {
            DateTime fullbrd = DateTime.Parse("01/01/1980");
            try
            {
                indate = indate.Substring(3, 2) + "/" + indate.Substring(0, 2) + "/" + indate.Substring(6, 4);
                DateTime dt = DateTime.Parse(indate);
                return dt;
            }
            catch (Exception)
            {

            }
            return fullbrd;
        }
        public string formatpm(string inObject)
        {
            string outObject = "";
            try
            {
                inObject = inObject.Replace(" ", "");

                string[] words = inObject.Split(';');
                foreach (var line in words)
                {
                    if (line != "")
                    {
                        outObject += line + ";";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return outObject;
        }
        public string Isphone(string phones)
        {
            LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
            string strp = formatpm(phones);
            bool isNumeric = true;
            try
            {
                string[] words = strp.Split(';');
                foreach (string phone in words)
                {
                    if (phone != "")
                    {
                        Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
                        isNumeric = regex.IsMatch(phone);

                        if (isNumeric != true)
                        {
                            return Message.Content.MES011 + " [" + phone + "] ";
                        }
                        else
                        {
                            foreach (var row in db._0620_Workbase_Check_ExitsPhoneCus(phone))
                            {
                                if (row.CUSCOUNT != 0)
                                {
                                    return Message.Content.MES012 + " [" + phone + "] ";
                                }
                            }
                        }
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public string Isphone(int _cusId, string _phones)
        {
            LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
            string strp = formatpm(_phones);
            bool isNumeric = true;
            try
            {
                string[] words = strp.Split(';');
                foreach (string phone in words)
                {
                    if (phone != "")
                    {
                        Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
                        isNumeric = regex.IsMatch(phone);

                        if (isNumeric != true)
                        {
                            return Message.Content.MES011 + " [" + phone + "] ";
                        }
                        else
                        {
                            foreach (var row in db._0620_Workbase_Check_ExitsPhoneCus_Udp(_cusId, phone))
                            {
                                string[] p = row.CUS_PHONE.Split(';');
                                foreach (string _p in p)
                                {
                                    if (_p == phone)
                                    {
                                        return Message.Content.MES012 + " [" + phone + "] ";
                                    }
                                }
                            }
                        }
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public string Ismail(string emails)
        {
            LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
            RegexUtilities util = new RegexUtilities();
            string strm = formatpm(emails);
            bool isEmail = false;
            try
            {
                string[] words = strm.Split(';');
                foreach (var email in words)
                {
                    if (email.Trim() != "")
                    {
                        if (util.IsValidEmail(email.Trim()))
                        {
                            isEmail = true;
                            foreach (var row in db._0620_Workbase_Check_ExitsEmailCus(email.Trim()))
                            {
                                if (row.CUSCOUNT != 0)
                                {
                                    return Message.Content.MES012 + " [" + email + "] ";
                                }
                            }
                        }
                        else
                        {
                            return Message.Content.MES011 + " [" + email + "] ";
                        }
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string Ismail(int _cusId, string emails)
        {
            LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
            RegexUtilities util = new RegexUtilities();
            string strm = formatpm(emails);
            bool isEmail = false;
            try
            {
                string[] words = strm.Split(';');
                foreach (var email in words)
                {
                    if (email.Trim() != "")
                    {
                        if (util.IsValidEmail(email.Trim()))
                        {
                            isEmail = true;
                            foreach (var row in db._0620_Workbase_Check_ExitsEmailCus_udp(_cusId, email.Trim()))
                            {
                                string[] p = row.CUS_EMAIL.Split(';');
                                foreach (string _p in p)
                                {
                                    if (_p == email)
                                    {
                                        return Message.Content.MES012 + " [" + email + "] ";
                                    }
                                }
                            }
                        }
                        else
                        {
                            return Message.Content.MES011 + " [" + email + "] ";
                        }
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public bool VerifyLogin()
        {
            LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
            Function fc = new Function();
            HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;

            string _tokenDate = fc.ToHexString(DateTime.Now.ToShortDateString());

            if (cookies[CommonConstants.SESSION_CREDENTIALS] != null
                && cookies[CommonConstants.SESSION_CREDENTIALS].Value == _tokenDate + fc.ToHexString(cookies["Token"].Value))
            {
                var _email = cookies["Email"].Value;
                var _pwd = fc.DecryptString(cookies["HashedPassword"].Value);
                var _role = cookies["RoleName"].Value;
                bool flag = false;
                foreach (var per in db._012020_CRM_V3_FUNC_User_Login(_role, _email, _pwd))
                {
                    return true;
                }
                if (flag == false)
                {
                    string[] myCookies = HttpContext.Current.Request.Cookies.AllKeys;
                    foreach (string cookie in myCookies)
                    {
                        HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                    }
                    return false;
                }
            }
            return false;
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
        public string SplitAndAddSeparator(string inObject)
        {
            string outObject = "";
            try
            {
                inObject = inObject.Replace(" ", "");
                string[] words = inObject.Split(',');
                foreach (var line in words)
                {
                    if (line != "")
                    {
                        outObject += ",'" + line + "'";
                    }
                }
                outObject = outObject.Substring(1);
            }
            catch (Exception ex)
            {
                return "";
            }
            return outObject;
        }
        //General random character code
        public string GetVerifyCode(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
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
        //Get list files attach
        public string GetFilesAttach(string _mode, int _id)
        {
            string _files = "";
            LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
            if (_mode == "id")
            {
                foreach (var file in db._0620_Workbase_Get_AttachFiles_ByFeedId(_id))
                {   
                    _files += file.AttachId + ",";
                }
                if (_files != "")
                    _files = _files.Substring(0, _files.Length - 1);
            }else if(_mode == "name")
            {
                foreach (var file in db._0620_Workbase_Get_AttachFiles_ByAttachId(_id))
                {
                    _files = file.FileAttachName;
                }
            }            
            return _files;
        }

        //Get list files attach
        public string GetListCSStaff(int _Cid)
        {
            string _list = "";
            LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
            foreach (var cs in db.p_API_CUSTOMERS_GetCsStaffs(_Cid))
            {
                _list += cs.AvatarImg + ";";
            }
            if (_list != "")
                _list = _list.Substring(0, _list.Length - 1);
            return _list;
        }

        public string GetMainSubProduct(string _token, string _staff)
        {
            string _flag = "";
            LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
            foreach (var res in db._0620_Workbase_GetAllTeamProduct_ByToken(_token))
            {
                if(_staff == res.STAFF_ID.ToString())
                {
                    _flag = res.FlagMainSub;
                }
            }
           
            return _flag;
        }
       
        //public void SendNotificationToUser_Notuse(string _casemode, int _cusid,int _sender, int _receiver, string _descript, string _details)
        //{
        //    EmailTemplate template = new EmailTemplate();
        //    LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
        //    EmailHelper eh = new EmailHelper();

        //    string _senderName = DecryptString(CookieHelper.GetCookie("FullNameHash"));
        //    string _cusName = "";
        //    string _emailsreciver = "";string _subject = ""; string _content = ""; string _footer = "";
        //    switch (_casemode)
        //    {
        //        case CaseMode.CTS:
        //            // Cus to staff - _sender alway = 23;
        //            _cusName = DecryptString(CookieHelper.GetCookie("FullNameHash"));
        //            foreach (var res in db._0620_Workbase_GetAllStaffFollow_ByCusId(Convert.ToInt32(_cusid)))
        //            {
        //                _cusName = res.CUS_NAME;
        //                if (_sender != res.STAFF_ID)
        //                {
        //                    _emailsreciver += res.STAFF_EMAIL + ";";
        //                    db._0620_Workbase_AddNotification_System(_cusid, _sender, res.STAFF_ID, _casemode, _descript, _details);
        //                }
        //            }
        //            _subject = "Khách hàng " + _senderName + " vừa có phản hồi mới ";
        //            _content = _senderName + " " + _descript + "<br>" + _details;
        //            _footer = "Vui lòng truy cập hồ sơ khách <a href=\"https://workplace.immgroup.com/customer/profile/" + _cusid + "\">" + _cusName + "</a> để xem chi tiết";
        //            Dictionary<string, string> para = new Dictionary<string, string>() {
        //                    { "{HEADER}", _subject } ,
        //                    { "{DETAILS}", _content } ,
        //                    { "{FOOTER}", _footer}
        //                };
        //            string _body = template.SystemToUser(para, "Notification");
        //            //SendMessageMailKit("[Hệ thống hồ sơ - Imm Group]", "crm@immgroup.com", "xnmpyehltkznxedc", ConvertArray(_emailsreciver), "", "", _subject, _body);
        //            eh.SendEmailAsync("",ConvertArray(_emailsreciver),"","",_subject,_body,null);
        //            break;
        //        case CaseMode.SHC:
        //            // Staff handle cus profile                              
        //            foreach (var res in db._0620_Workbase_GetAllStaffFollow_ByCusId(Convert.ToInt32(_cusid)))
        //            {
        //                _cusName = res.CUS_NAME;
        //                if (_sender != res.STAFF_ID)
        //                {
        //                    _emailsreciver += res.STAFF_EMAIL + ";";
        //                    db._0620_Workbase_AddNotification_System(_cusid, _sender, res.STAFF_ID, _casemode, _descript, _details);
        //                }
        //            }
        //            string leader = NVL(DBHelper.GetColumnVal("SELECT TOP 1 ISNULL(T_LEADER_ID,'') AS T_LEADER_ID FROM M_TEAMS WHERE  T_MEMBERS_ID  LIKE '%;" + _sender + ";%' AND T_FLAG_ACTIVE = 1 ORDER BY T_LEADER_ID DESC", "T_LEADER_ID"));
        //            if(leader != "")
        //            {
        //                db._0620_Workbase_AddNotification_System(_cusid, _sender, Convert.ToInt32(leader), _casemode, _descript, _details);
        //            }
        //            ActivityLog(_casemode, _cusid, _sender, _descript);
        //            _subject = _senderName + " vừa thao tác hồ sơ khách hàng " + _cusName;
        //            _content = _senderName + " " + _descript + "<br>" + _details;
        //            _footer = "Vui lòng truy cập hồ sơ khách <a href=\"https://system.immgroup.com/customer/profile/" + _cusid + "\">" + _cusName + "</a> để xem chi tiết";
        //            break;
        //        case CaseMode.SHS:
        //            // Staff asign staff
        //            db._0620_Workbase_AddNotification_System(1, _sender, _receiver, _casemode, _descript, _details);
        //            ActivityLog(_casemode, _receiver, _sender, _descript);
        //            _subject = _senderName + " " + _descript;
        //            _content = _senderName + " " + _descript + "<br>" + _details;
        //            _footer = "";
        //            break;
        //        case CaseMode.SYSTS:
        //            // System handle staff

        //            break;                  
        //    }           
           
        //}
        
    }
    public class CaseMode
    {
        public const string SHC = "Staff-handle-Cus";
        public const string SHS = "Staff-handle-Staff";
        public const string SYSTS = "System-to-Staffs";
        public const string CTS = "Customer-to-Staff";
        public const string OMTS = "OM-to-Staffs";
        public const string DRIVE = "Staffs-handle-DriveStorage";
    }
    public class Message
    {
        public JsonResult Show(string _MessCode)
        {
            string Query = "SELECT * FROM M_MESSAGE WHERE MES_PRI_KEY = 'VN' AND MES_SEC_KEY = '" + _MessCode + "'";
            JsonResult response = new JsonResult();

            using (SqlConnection connection = new SqlConnection(DBHelper.connectionString))
            {
                connection.Open();
                try
                {
                    using (DBHelper.cmd = new SqlCommand(Query, connection))
                    {
                        DBHelper.cmd.ExecuteNonQuery();
                        SqlDataReader Reader = DBHelper.cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(Reader);
                        foreach (DataRow row in dt.Rows)
                        {
                            response.Data = new
                            {
                                mess = row[4],
                                type = row[2],
                                header = row[3],
                                pos = row[5]
                            };
                        }
                        Reader.Close();
                    }
                }
                catch
                {
                    connection.Close();
                }
                finally
                {
                    connection.Close();
                }
            }
            return response;
        }

        public JsonResult Success(string message)
        {            
            return Show(message, Library.Message.Type.SUCCESS, "Thông báo", Library.Message.Position.TOP_C);
            
        }
        public JsonResult Info(string message)
        {
            return Show(message, Library.Message.Type.INFO, "Thông báo", Library.Message.Position.TOP_C);

        }
        public JsonResult Error(string message)
        {           
            return Show(message, Library.Message.Type.ERROR, "Something Wrong!", Library.Message.Position.TOP_C);
        }
        public JsonResult Warning(string message)
        {
            return Show(message, Library.Message.Type.WARNING, "Thông báo", Library.Message.Position.TOP_C);
        }

        public JsonResult Success()
        {
            return Show(Library.Message.Content.MES005, Library.Message.Type.SUCCESS, "Thông báo", Library.Message.Position.TOP_C);

        }
        public JsonResult Error()
        {
            return Show(Library.Message.Content.MES006, Library.Message.Type.ERROR, "Something Wrong!", Library.Message.Position.TOP_C);
        }
        public JsonResult Show(string _Mess, string _Type, string _Header, string _Position)
        {
            JsonResult response = new JsonResult();
            response.Data = new
            {
                mess = _Mess,
                type = _Type,
                header = _Header,
                pos = _Position
            };
            return response;
        }
        public class Type
        {
            public const string ERROR = "error";
            public const string SUCCESS = "success";
            public const string WARNING = "warning";
            public const string INFO = "info";
        }
        public class Language
        {
            public const string VN = "VN";
            public const string ENG = "ENG";
        }
        public class Position
        {
            public const string TOP_R = "toast-top-right";
            public const string TOP_L = "toast-top-left";
            public const string BOT_R = "toast-bottom-right";
            public const string BOT_L = "toast-bottom-left";
            public const string TOP_F = "toast-top-full-width";
            public const string BOT_F = "toast-bottom-full-width";
            public const string TOP_C = "toast-top-center";
        }
        public class Content
        {
            public const string MES000 = "Bạn chưa được cấp quyền để sử dụng chức năng này";
            public const string MES001 = "Thông tin nhập vào không được để trống";
            public const string MES002 = "Bạn không có mặt tại văn phòng, không thể đăng nhập vào hệ thống.";
            public const string MES003 = "Thông tin tài khoản không đúng. Vui lòng kiểm tra email và mật khẩu.";
            public const string MES004 = "Nội dung nhập vào không được để trống.";
            public const string MES005 = "Thao tác hoàn tất";
            public const string MES006 = "Thao tác thất bại";
            public const string MES007 = "Đã có lỗi xảy ra trong quá trình xử lý. Đội ngũ chúng tôi đang cố gắng khắc phục vấn đề này.";
            public const string MES008 = "Vui lòng nhập mã xác minh được gửi qua email của bạn để đăng nhập vào hệ thống.";
            public const string MES009 = "Phiên đăng nhập của bạn đã hết hạn. Vui lòng đăng nhập lại";
            public const string MES010 = "Bạn chưa được cấp quyền sử dụng chức năng này.";
            public const string MES011 = "Nội dung nhập vào không đúng định dạng";
            public const string MES012 = "Nội dung nhập vào đã tồn tại";
        }
    }
    public class CateAlert
    {
        public const string C001 = "vừa cập nhật thông tin hồ sơ khách";
        public const string C002 = "vừa cập nhật thông tin người phụ thuộc của khách";
        public const string C003 = "vừa thêm một ghi chú nội bộ";
        public const string C004 = "vừa có trao đổi mới với khách hàng";
        public const string C005 = "vừa có trao đổi mới với luật sư hoặc đối tác";
        public const string C006 = "vừa có phản hồi mới";
        public const string C007 = "Luật sư hoặc đối tác có phản hồi mới";
    }
    public class DBHelper
    {
        public static SqlCommand cmd = new SqlCommand();
        public static SqlDataAdapter sda;
        public static SqlDataReader sdr;
        public static DataSet ds = new DataSet();
        public static SqlConnection con = new SqlConnection("Data Source=103.252.252.254;Initial Catalog=ImmiCRM;Persist Security Info=True;User ID=sa;Password=Mekongdelta@2018");
        public static DataTable dt = new DataTable();
        public static readonly string connectionString = "Data Source=103.252.252.254;Initial Catalog=ImmiCRM;Persist Security Info=True;User ID=sa;Password=Mekongdelta@2018";

        public static bool IsExist(string Query)
        {
            bool check = false;
            using (cmd = new SqlCommand(Query, con))
            {
                con.Open();
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                    check = true;
            }
            sdr.Close();
            con.Close();
            return check;

        }

        public static bool ExecuteQuery(string Query)
        {
            bool flg = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (cmd = new SqlCommand(Query, connection))
                    {
                        cmd.ExecuteNonQuery();
                        flg = true;
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flg;
        }

        public static string GetColumnVal(string Query, string ColumnName)
        {
            string RetVal = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (cmd = new SqlCommand(Query, connection))
                    {
                        sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            RetVal = sdr[ColumnName].ToString();
                            break;
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    connection.Close();
                }
            }
            return RetVal;
        }

        public static string GetColumnVal(string Query, string ColumnName, System.Data.CommandType Type)
        {
            string RetVal = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (cmd = new SqlCommand(Query, connection))
                    {                       
                        cmd.CommandType = Type;
                        sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            RetVal = sdr[ColumnName].ToString();
                            break;
                        }
                        cmd.Parameters.Clear();
                        sdr.Close();
                    }
                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
            }           
            return RetVal;
        }

        public static string GetColumnVal(string Query, string ColumnName, Dictionary<string, string> Para, System.Data.CommandType Type)
        {
            string RetVal = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (cmd = new SqlCommand(Query, connection))
                    {
                        foreach (KeyValuePair<string, string> kvp in Para)
                        {
                            cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                        }
                        cmd.CommandType = Type;
                        sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            RetVal = sdr[ColumnName].ToString();
                            break;
                        }
                        cmd.Parameters.Clear();
                        sdr.Close();
                    }
                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
            }           
            return RetVal;
        }


        public static DataTable DB_ToDataTable(string Query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (cmd = new SqlCommand(Query, connection))
                    {
                        dt.Load(sdr = cmd.ExecuteReader());
                        cmd.Parameters.Clear();
                        sdr.Close();
                    }
                    return dt;
                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }

        public static DataTable DB_ToDataTable(string Query, System.Data.CommandType Type)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (cmd = new SqlCommand(Query, connection))
                    {                       
                        cmd.CommandType = Type;
                        dt.Load(sdr = cmd.ExecuteReader());
                        cmd.Parameters.Clear();
                        sdr.Close();
                    }
                    return dt;
                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }

        public static DataTable DB_ToDataTable(string Query, Dictionary<string, string> Para, System.Data.CommandType Type)
        {

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (cmd = new SqlCommand(Query, connection))
                    {
                        foreach (KeyValuePair<string, string> kvp in Para)
                        {
                            cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                        }
                        cmd.CommandType = Type;
                        dt.Load(sdr = cmd.ExecuteReader());
                        cmd.Parameters.Clear();
                        sdr.Close();
                    }
                    return dt;
                }
                catch
                {
                    
                }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }

        public static List<dynamic> DB_ToDynamicList(string Query, System.Data.CommandType Type)
        {
            var dynamicDt = new List<dynamic>();
            SqlDataReader sqlReader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (cmd = new SqlCommand(Query, connection))
                    {                        
                        cmd.CommandType = Type;
                        dt.Load(sqlReader = cmd.ExecuteReader());
                        foreach (DataRow row in dt.Rows)
                        {
                            dynamic dyn = new ExpandoObject();
                            dynamicDt.Add(dyn);
                            foreach (DataColumn column in dt.Columns)
                            {
                                var dic = (IDictionary<string, object>)dyn;
                            }
                        }
                        cmd.Parameters.Clear();
                        sqlReader.Close();
                    }
                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            return dynamicDt;
        }

        public static List<dynamic> DB_ToDynamicList(string Query, Dictionary<string, string> Para, System.Data.CommandType Type)
        {
            var dynamicDt = new List<dynamic>();
            cmd.CommandText = Query;
            cmd.Connection = con;
            cmd.CommandType = Type;

            foreach (KeyValuePair<string, string> kvp in Para)
            {
                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
            }

            con.Open();

            SqlDataReader sqlReader = cmd.ExecuteReader();
            //DataTable schemaTable = sqlReader.GetSchemaTable();
            DataTable dt = new DataTable();
            dt.Load(sqlReader);
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            cmd.Parameters.Clear();
            sdr.Close();
            con.Close();
            return dynamicDt;
        }

        public async Task<bool> ExecuteQueryBySqlString(string _SqlqueryString)
        {
            try
            {
                if (!String.IsNullOrEmpty(_SqlqueryString))
                {
                    await Task.Run(() =>
                        DBHelper.ExecuteQuery(_SqlqueryString)
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Excute query error: " + ex.Message);
            }
            return true;
        }
    }
    public class Res
    {
        public JsonResult Val(string _Value)
        {
            JsonResult response = new JsonResult();
            response.Data = new { val = _Value };
            return response;
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
    public class EmailTemplate
    {
        public string ReplyWhenCusSubmitWebForm(string formname,
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
            string path = "~/Extensions/EmailTemplate/WebFormReply" + formname+".htm";

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

        public string SentImmCode(string app_pass)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Extensions/EmailTemplate/VerifyLoginCode.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{IMMCODE}", app_pass);
            return body;
        }

        public string SystemToUser(Dictionary<string, string> _para, string _template)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            if (_template == "")
            {
                _template = "FromSystem.htm";
            }
            string _template_path = "~/Extensions/EmailTemplate/" + _template + ".htm";

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(_template_path)))
            {
                body = reader.ReadToEnd();
            }
            foreach (KeyValuePair<string, string> kvp in _para)
            {
                body = body.Replace(kvp.Key, kvp.Value);
            }
            return body;
        }

        public string SentInforToCus(string CusName, string CusEmail, string CusSex, string CusSexUp, string CusPhone, string StaffSign)
        {
            string body = string.Empty;
            DateTime mmday = DateTime.Now;
            string format = "hh:mm:ss tt dd-MM-yyyy";
            string mday = mmday.ToString(format);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Extensions/EmailTemplate/sentinforcus.htm")))
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
    }
    public class EmailHelper
    {
        public void SendEmailAsync(string _emailHeader,
                                          string _from, 
                                          string _password, 
                                          string _to, 
                                          string _cc, 
                                          string _bcc, 
                                          string _subject, 
                                          string _msg,
                                          string _attachFile
                                          //Attachment _attachFile
                                          )
        {
            try
            {
                var message = new MailMessage();
                // CC.
                if (string.IsNullOrEmpty(_cc) == false)
                {
                    foreach (var item in _cc.Split(';'))
                    {
                        message.CC.Add(new MailAddress(item));
                    }
                }
                // BCC.
                if (string.IsNullOrEmpty(_bcc) == false)
                {
                    foreach (var item in _bcc.Split(';'))
                    {
                        message.Bcc.Add(new MailAddress(item));
                    }
                }
                // TO.
                if (string.IsNullOrEmpty(_to) == false)
                {
                    foreach (var item in _to.Split(';'))
                    {
                        if (item != "")
                            message.To.Add(new MailAddress(item));
                    }
                }
                // FROM
                if (string.IsNullOrEmpty(_from) == false)
                {
                    message.From = new MailAddress(_from);
                }

                message.From = new MailAddress(message.From.Address, _emailHeader);
                message.Subject = _subject;
                message.Body = _msg;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Priority = System.Net.Mail.MailPriority.High;

                if (!String.IsNullOrEmpty(_attachFile))
                {
                    string[] Attmails = _attachFile.Split(';');
                    foreach (string attid in Attmails)
                    {
                        if (!String.IsNullOrEmpty(attid))
                        {
                            message.Attachments.Add(new Attachment(attid));
                        }
                    }
                }

                //if (_attachFile != null)
                //{
                //    message.Attachments.Add(_attachFile);
                //}

                Task t = Task.Run(async () =>
                {
                    // You should use using block so .NET can clean up resources
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new System.Net.NetworkCredential
                        {
                            UserName = _from,
                            Password = _password
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                });            
            }
            catch (Exception ex)
            {
                throw new Exception("Sending mail error: " + ex.Message);
            }
        }
        public void SendEmailAsync(string _emailHeader,
                                          string _to,
                                          string _cc,
                                          string _bcc,
                                          string _subject,
                                          string _msg,
                                          string _attachFile
                                          //Attachment _attachFile
                                          )
        {
            try
            {
                var message = new MailMessage();
                // CC.
                if (string.IsNullOrEmpty(_cc) == false)
                {
                    foreach (var item in _cc.Split(';'))
                    {
                        message.CC.Add(new MailAddress(item));
                    }
                }
                // BCC.
                if (string.IsNullOrEmpty(_bcc) == false)
                {
                    foreach (var item in _bcc.Split(';'))
                    {
                        message.Bcc.Add(new MailAddress(item));
                    }
                }
                // TO.
                if (string.IsNullOrEmpty(_to) == false)
                {
                    foreach (var item in _to.Split(';'))
                    {
                        if (item != "")
                            message.To.Add(new MailAddress(item));
                    }
                }
                var _from = "crm@immgroup.com";
                var _password = "duyhhwbuverzqkac";
                var _header = "[Hệ thống - Imm Group]";
                if (_emailHeader != "") _header = _emailHeader;
                message.From = new MailAddress(_from, _header);
                message.Subject = _subject;
                message.Body = _msg;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Priority = System.Net.Mail.MailPriority.High;

                if (!String.IsNullOrEmpty(_attachFile))
                {
                    string[] Attmails = _attachFile.Split(';');
                    foreach (string attid in Attmails)
                    {
                        if (!String.IsNullOrEmpty(attid))
                        {
                            message.Attachments.Add(new Attachment(attid));
                        }
                    }
                }

                //if (_attachFile != null)
                //{
                //    message.Attachments.Add(_attachFile);
                //}

                Task t = Task.Run(async () =>
                {
                    // You should use using block so .NET can clean up resources
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new System.Net.NetworkCredential
                        {
                            UserName = _from,
                            Password = _password
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Sending mail error: " + ex.Message);
            }
        }        
        public string SendLoginInforToCus(int _cusid)
        {
            LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
            EmailTemplate template = new EmailTemplate();
            Function fc = new Function();
            string status = "";
            try
            {
                DateTime mmday = DateTime.Now;
                string format = "hh:mm:ss tt dd-MM-yyyy";
                string mday = mmday.ToString(format);
                var _email = CookieHelper.GetCookie("Email");
                var _pwd = fc.Encrypt(fc.DecryptString(CookieHelper.GetCookie("HashedPassword")));
                string _passmail = "";
                string _mailheader = "";
                string _staffmail = "";
                string _toemail = "";
                string _relativeemail = "";
                string _body = "";
                string _cusSex = "";
                string _cusSexup = "";
                string _cusName = "";
                string _signature = "";
                string _appuser = "";
                string _apppass = fc.GetUniqueKey(8);

                foreach (var row in db.CHECK_LOGIN(_email, _pwd))
                {
                    _passmail = fc.DecryptString(row.STAFF_PASS_EMAIL);
                    _mailheader = row.EMAIL_HEADER;
                    _staffmail = row.STAFF_EMAIL;
                    _signature = row.SIGNATURE_EMAIL;
                }
                foreach (var row in db.LOAD_CUS_INFOR(_cusid))
                {
                    _toemail = row.CUS_EMAIL;
                    string[] array = _toemail.Split(';');
                    string phone = row.CUS_PHONE;
                    if (phone.Contains(";"))
                    {
                        string[] arrayphone = phone.Split(';');
                        _apppass = arrayphone[0];
                    }
                    else
                    {
                        if (phone != "")
                        {
                            _apppass = phone;
                        }
                    }

                    _appuser = array[0];
                    if (_appuser == "")
                    {
                        _appuser = _toemail;
                    }
                    if (_toemail != "")
                    {
                        _cusName = row.CUS_NAME_VN;
                        db.TEMP_UDP_PASS(row.CUS_ID, _appuser, _apppass, row.APP_PHONE, row.APP_ADDRESS);

                        foreach (var relative in db._0620_Workbase_GetCusDependent_ById(_cusid))
                        {
                            _relativeemail += relative.CUS_EMAIL;
                        }

                        if (row.CUS_SEX == 0) { _cusSex = "chị"; _cusSexup = "Chị"; }
                        else if (row.CUS_SEX == 1) { _cusSex = "anh"; _cusSexup = "Anh"; }
                        else { _cusSex = "anh/chị"; _cusSexup = "Anh/Chị"; }

                        Dictionary<string, string> para = new Dictionary<string, string>() {
                            { "{CusSex}", _cusSex } ,
                            { "{CusSexUp}", _cusSexup } ,
                            { "{UserName}", row.CUS_NAME_VN } ,
                            { "{Time}", mday } ,
                            { "{CusEmail}", _appuser } ,
                            { "{CusPhone}", _apppass } ,
                            { "{Signature}", _signature }
                        };
                        _body = template.SystemToUser(para, "SendLoginInfoToCus");
                        //SendMessageMailKit(_mailheader, _staffmail, _passmail, ConvertArray(_toemail), ConvertArray(_relativeemail), _staffmail, "Thông tin đăng nhập liên lạc với IMM Group", _body);
                        SendEmailAsync(_mailheader, _staffmail, _passmail, fc.ConvertArray(_toemail), fc.ConvertArray(_relativeemail), "", "Thông tin đăng nhập liên lạc với IMM Group", _body, "");
                        status = "Email thông tin đăng nhập đã được gửi đến khách hàng";
                    }
                    else
                    {
                        status = "Email không được để trống.";
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /* Không sử dụng nưa - gom lại thành 1 hàm
        public static void SendAsync(string from, string password, string to, string cc, string bcc, string subject, string msg, Attachment attachFile, string emailHeader = "Hệ thống hồ sơ | IMM Group")
        {
            try
            {
                var message = new MailMessage();

                // CC.
                if (string.IsNullOrEmpty(cc) == false)
                {
                    foreach (var item in cc.Split(';'))
                    {
                        message.CC.Add(new MailAddress(item));
                    }
                }

                // BCC.
                if (string.IsNullOrEmpty(bcc) == false)
                {
                    foreach (var item in bcc.Split(';'))
                    {
                        message.Bcc.Add(new MailAddress(item));
                    }
                }

                // TO.
                if (string.IsNullOrEmpty(to) == false)
                {
                    foreach (var item in to.Split(';'))
                    {
                        if (item != "")
                            message.To.Add(new MailAddress(item));
                    }
                }
                // FROM
                if (string.IsNullOrEmpty(from) == false)
                {
                    message.From = new MailAddress(from);
                }

                message.From = new MailAddress(message.From.Address, emailHeader);
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                message.Priority = System.Net.Mail.MailPriority.High;

                if (attachFile != null)
                {
                    message.Attachments.Add(attachFile);
                }
                Task t = Task.Run(async () =>
                {
                    // You should use using block so .NET can clean up resources
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new System.Net.NetworkCredential
                        {
                            UserName = from,
                            Password = password
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                });

                //t.Wait();               
            }
            catch (Exception ex)
            {
                throw new Exception("Sending mail error: " + ex.Message);
            }
        }
        public void SendMessageStandard(string MailHeader, string SenderEmail, string SenderPassword, string ToEmail, string CcEmail, string BccEmail, string Subject, string BodyEmail, string Attach)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(SenderEmail, MailHeader);
            string[] Tomails = ToEmail.Replace(" ", "").Split(';');
            foreach (string email in Tomails)
            {
                if (!String.IsNullOrEmpty(email))
                {
                    message.To.Add(new MailAddress(email));
                }
            }
            string[] CcEmails = CcEmail.Replace(" ", "").Split(';');
            foreach (string email in CcEmails)
            {
                if (!String.IsNullOrEmpty(email))
                {
                    message.CC.Add(new MailAddress(email));
                }
            }
            string[] BccEmails = BccEmail.Replace(" ", "").Split(';');
            foreach (string email in BccEmails)
            {
                if (!String.IsNullOrEmpty(email))
                {
                    message.Bcc.Add(new MailAddress(email));
                }
            }
            message.Subject = Subject;
            message.Body = BodyEmail;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            if (!String.IsNullOrEmpty(Attach))
            {
                //Attachment att = new Attachment(attach);
                string[] Attmails = Attach.Split(';');
                foreach (string attid in Attmails)
                {
                    if (!String.IsNullOrEmpty(attid))
                    {
                        message.Attachments.Add(new Attachment(attid));
                    }
                }
            }
            Task t = Task.Run(async () =>
            {
                // You should use using block so .NET can clean up resources
                using (var smtp = new SmtpClient())
                {
                    var credential = new System.Net.NetworkCredential
                    {
                        UserName = SenderEmail,
                        Password = SenderPassword
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
            });
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(SenderEmail, SenderPassword);
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.Send(message);
            //client.Dispose();
        }
         

        public void SendMessageMailKit_backup_notuse(string MailHeader, string SenderEmail, string SenderPassword, string ToEmail, string CcEmail, string BccEmail, string Subject, string BodyEmail)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(SenderEmail, MailHeader);
            string[] Tomails = ToEmail.Replace(" ", "").Split(';');
            foreach (string email in Tomails)
            {
                if (!String.IsNullOrEmpty(email))
                {
                    message.To.Add(new MailAddress(email));
                }
            }
            string[] CcEmails = CcEmail.Replace(" ", "").Split(';');
            foreach (string email in CcEmails)
            {
                if (!String.IsNullOrEmpty(email))
                {
                    message.CC.Add(new MailAddress(email));
                }
            }
            string[] BccEmails = BccEmail.Replace(" ", "").Split(';');
            foreach (string email in BccEmails)
            {
                if (!String.IsNullOrEmpty(email))
                {
                    message.Bcc.Add(new MailAddress(email));
                }
            }
            message.Subject = Subject;
            message.Body = BodyEmail;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(SenderEmail, SenderPassword);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(message);
            client.Dispose();
            //            var client = new RestSharp.RestClient("http://api.crm.imm.group/imm/api");
            //            var request = new RestSharp.RestRequest("sendmail-web", Method.POST);
            //            var sendData = new
            //            {
            //                MailHeader = MailHeader,
            //                SenderEmail = SenderEmail,
            //                SenderPassword = SenderPassword,
            //                ToEmail = ToEmail,
            //                CcEmail = CcEmail,
            //                BccEmail = BccEmail,
            //                Subject = Subject,
            //                BodyEmail = BodyEmail
            //            };
            //            request.AddJsonBody(sendData);
            //#pragma warning disable CS0618 // Type or member is obsolete
            //            var asyncHandle = client.ExecuteAsync(request, response =>
            //            {
            //                Console.WriteLine(response.Content);
            //            });
            //#pragma warning restore CS0618 // Type or member is obsolete
        }
      
        */
    }    
    public class Notification
    {
        EmailTemplate template = new EmailTemplate();
        LinQtoSqlClassDataContext db = new LinQtoSqlClassDataContext();
        EmailHelper eh = new EmailHelper();
        SQLQueryHelper sqlquery = new SQLQueryHelper();
        Function fc = new Function();
        DBHelper dbh = new DBHelper();

        public void Add(string _casemode, int _cusid, int _sender, int _receiver, string _descript, string _details)
        {
            

            string _senderName = fc.DecryptString(CookieHelper.GetCookie("FullNameHash"));
            string _cusName = "Khách hàng";
            string _emailsreciver = ""; 

            string _subject = ""; 
            string _content = ""; 
            string _footer = "";
            string _sql = "";
            switch (_casemode)
            {
                case CaseMode.CTS:  //Cus to staff - _sender alway = 23;
                    foreach (var res in db._0620_Workbase_GetAllStaffFollow_ByCusId(Convert.ToInt32(_cusid)))
                    {
                        _cusName = res.CUS_NAME;
                        if (_sender != res.STAFF_ID)
                        {
                            _emailsreciver += res.STAFF_EMAIL + ";";
                            _sql += sqlquery.InsertNotification(_cusid,_sender, res.STAFF_ID, _casemode, _descript, _details);
                            //db._0620_Workbase_AddNotification_System(_cusid, _sender, res.STAFF_ID, _casemode, _descript, _details);
                        }
                    }
                    _subject = "Khách hàng " + _senderName + " vừa có phản hồi mới ";
                    _content = _senderName + " " + _descript + "<br>" + _details;
                    _footer = "Vui lòng truy cập hồ sơ khách <a href=\"https://system.immgroup.com/customer/profile/" + _cusid + "\">" + _cusName + "</a> để xem chi tiết";
                    Dictionary<string, string> para = new Dictionary<string, string>() {
                            { "{HEADER}", _subject } ,
                            { "{DETAILS}", _content } ,
                            { "{FOOTER}", _footer}
                        };
                    string _body = template.SystemToUser(para, "Notification");
                    //SendMessageMailKit("[Hệ thống hồ sơ - Imm Group]", "crm@immgroup.com", "xnmpyehltkznxedc", ConvertArray(_emailsreciver), "", "", _subject, _body);
                    eh.SendEmailAsync("", fc.ConvertArray(_emailsreciver), "", "", _subject, _body, null);
                    break;
                case CaseMode.SHC:
                    // Staff handle cus profile                              
                    foreach (var res in db._0620_Workbase_GetAllStaffFollow_ByCusId(Convert.ToInt32(_cusid)))
                    {
                        _cusName = res.CUS_NAME;
                        if (_sender != res.STAFF_ID)
                        {
                            _emailsreciver += res.STAFF_EMAIL + ";";
                            _sql += sqlquery.InsertNotification(_cusid, _sender, res.STAFF_ID, _casemode, _descript, _details);
                            //db._0620_Workbase_AddNotification_System(_cusid, _sender, res.STAFF_ID, _casemode, _descript, _details);
                        }
                    }
                    string leader = fc.NVL(DBHelper.GetColumnVal("SELECT TOP 1 ISNULL(T_LEADER_ID,'') AS T_LEADER_ID FROM M_TEAMS WHERE  T_MEMBERS_ID  LIKE '%;" + _sender + ";%' AND T_FLAG_ACTIVE = 1 ORDER BY T_LEADER_ID DESC", "T_LEADER_ID"));
                    if (leader != "")
                    {                        
                        _sql += sqlquery.InsertNotification(_cusid, _sender, Convert.ToInt32(leader), _casemode, _descript, _details);
                        _sql += sqlquery.InsertActivityLog(_casemode, _cusid, _sender, _descript);
                        //db._0620_Workbase_AddNotification_System(_cusid, _sender, Convert.ToInt32(leader), _casemode, _descript, _details);
                    }                  
                    break;
                case CaseMode.SHS:
                    // Staff asign staff
                    _sql += sqlquery.InsertNotification(1, _sender, _receiver, _casemode, _descript, _details);
                    //db._0620_Workbase_AddNotification_System(1, _sender, _receiver, _casemode, _descript, _details);
                    _sql += sqlquery.InsertActivityLog(_casemode, _receiver, _sender, _descript);
                   
                    break;
                case CaseMode.DRIVE:
                   
                    break;
                case CaseMode.SYSTS:
                    // System handle staff

                    break;
            }
            var async = dbh.ExecuteQueryBySqlString(_sql);
        }     

        
    }
    public class SQLQueryHelper
    {
        public string GetMenu()
        {
            string sql = " SELECT ";
            sql += "  MENU_KEY ";
            sql += ", MENU_PARENT_KEY ";
            sql += ", MENU_NAME ";
            sql += ", MENU_URL ";
            sql += ", FLAG_ACTIVE ";
            sql += ", MENU_NOTE ";
            sql += " FROM M_MENU ";
            return sql;
        }

        public string InsertNotification(int _CusId, int _Sender, int _Receive, string _CaseMess, string _Mesage, string _MessageDetails)
        {
            string sql = "INSERT INTO [dbo].[M_NOTIFICATION_SYSTEM] ";
            sql += " (CustomerId";
            sql += " ,CateMess";
            sql += " ,Message";
            sql += " ,MessageDetails";
            sql += " ,StaffSend";
            sql += " ,StaffReceive)";
            sql += " VALUES ";
            sql += " ( ";
            sql += _CusId + ",";
            sql += "N'" + _CaseMess + "',";
            sql += "N'" + _Mesage + "',";
            sql += "N'" + _MessageDetails + "',";
            sql += _Sender + ",";
            sql += _Receive;
            sql += "); ";
            return sql;
        }

        public string InsertActivityLog(string _modeHandle, int _tohandleId, int _fromhandleId, string _act)
        {
            return "INSERT INTO M_ACTIVITY(STAFF_ID, CUS_ID, ACT_NOTE, INSERT_DATE, ACT_MODE) VALUES(" + _fromhandleId + "," + _tohandleId + ", N'" + _act + "', GETDATE(),'" + _modeHandle + "')";
        }

        #region PRM SQL STRING
        public string UpdateReceiverIdToRequest(string Request_No)
        {
            string sql = " UPDATE PRM_REQUEST " +
                " SET RECEIVER_ID = (SELECT r.RECEIVER_ID FROM PRM_RECEIVER r WHERE r.RECEIVER_BANK_ID = PRM_REQUEST.RECEIVER_BANK AND r.RECEIVER_STK = PRM_REQUEST.RECEIVER_STK) " +
                " WHERE REQUEST_NO = '" + Request_No + "' ;";
            return sql;
        }
        public string AddPaymentRequestActivity(string Request_No, int Owner, string Noted)
        {
            string sql = "";
            sql += " INSERT INTO PRM_REQUEST_ACTIVITY (REQUEST_NO ,R_A_OWNER ,R_A_NOTED ,FLAG_ACTIVE ,INSERT_DATE ,UPDATE_DATE) ";
            sql += " VALUES ( ";
            sql += "'" + Request_No + "'";
            sql += ", " + Owner;
            sql += ", N'" + Noted + "'";
            sql += ", 1, GETDATE(), GETDATE()); ";
            return sql;
        }
        #endregion
    }
    public class Utils
    {
        /// 

        /// Chuyển phần nguyên của số thành chữ
        /// 
        /// Số double cần chuyển thành chữ
        /// Chuỗi kết quả chuyển từ số
        public string NumberToText(double inputNumber, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString("#");
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            return UpperFirstChar(result + (suffix ? " đồng chẵn" : ""));
        }
        private string UpperFirstChar(string str)
        {
            string str1;
            if (str != "")
            {
                str1 = str.Substring(0, 1).ToUpper() + str.Substring(1);
                //char[] charArray = str.ToCharArray();
                //charArray[0] = char.ToUpper(charArray[0]);
                //str1 = new string(charArray);
            }
            else
            {
                str1 = str;
            }
            return str1;
        }

        public string FormatMoney(double money, string _unit)
        {
            if (_unit.Trim() != "VND")
            {
                return String.Format("{0:n}", money);  // Output: 1,234.00
            }
            return String.Format("{0:n0}", money);
        }
    }

    public class PrmFunc
    {
        public DataTable GetReQuestActivity(string _rqNo)
        {
            DataTable _dt = new DataTable();
            Dictionary<string, string> Para = new Dictionary<string, string>();
            Para.Add("@RequestNo", _rqNo);
            _dt = DBHelper.DB_ToDataTable("PRM_Get_Activity_ByRequestNo", Para, CommandType.StoredProcedure);
            return _dt;
        }
        
       
    }
}
