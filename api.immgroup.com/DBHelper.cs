using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using OpenPop.Mime;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using OpenPop.Pop3.Exceptions;
using OpenPop.Common.Logging;
using Message = OpenPop.Mime.Message;
using System.Text.RegularExpressions;
using System.IO;

namespace api.immgroup.com
{
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

        public static void ExecuteQuery(string Query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (cmd = new SqlCommand(Query, connection))
                    {
                        cmd.ExecuteNonQuery();
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
            cmd.CommandText = Query;
            cmd.Connection = con;
            cmd.CommandType = Type;
            con.Open();

            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                RetVal = sdr[ColumnName].ToString();
                break;
            }

            sdr.Close();
            con.Close();
            return RetVal;
        }

        public static string GetColumnVal(string Query, string ColumnName, Dictionary<string, string> Para, System.Data.CommandType Type)
        {
            string RetVal = "";
            cmd.CommandText = Query;
            cmd.Connection = con;
            cmd.CommandType = Type;
            foreach (KeyValuePair<string, string> kvp in Para)
            {
                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
            }
            con.Open();

            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                RetVal = sdr[ColumnName].ToString();
                break;
            }
            cmd.Parameters.Clear();
            sdr.Close();
            con.Close();
            return RetVal;
        }


        public static DataTable DB_ToDataTable(string Query)
        {
            DataTable dt = new DataTable();
            cmd.CommandText = Query;
            cmd.Connection = con;
            con.Open();
            dt.Load(sdr = cmd.ExecuteReader());
            sdr.Close();
            con.Close();
            return dt;
        }

        public static DataTable DB_ToDataTable(string Query, System.Data.CommandType Type)
        {
            DataTable dt = new DataTable();
            cmd.CommandText = Query;
            cmd.Connection = con;
            cmd.CommandType = Type;
            con.Open();
            dt.Load(sdr = cmd.ExecuteReader());
            sdr.Close();
            con.Close();
            return dt;
        }

        public static DataTable DB_ToDataTable(string Query, Dictionary<string, string> Para, System.Data.CommandType Type)
        {
            DataTable dt = new DataTable();
            cmd.CommandText = Query;
            cmd.Connection = con;
            cmd.CommandType = Type;
            foreach (KeyValuePair<string, string> kvp in Para)
            {
                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
            }
            con.Open();
            dt.Load(sdr = cmd.ExecuteReader());
            cmd.Parameters.Clear();
            sdr.Close();
            con.Close();
            return dt;
        }

        public static List<dynamic> DB_ToDynamicList(string Query, System.Data.CommandType Type)
        {
            var dynamicDt = new List<dynamic>();
            cmd.CommandText = Query;
            cmd.Connection = con;
            cmd.CommandType = Type;
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
                    //dic[column.ColumnName] = row[column];
                }
            }

            con.Close();
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

        public static List<dynamic> DB_ToDynamicTable(string Query, Dictionary<object, object> Para, System.Data.CommandType Type)
        {
            var dynamicDt = new List<dynamic>();
            cmd.CommandText = Query;
            cmd.Connection = con;
            cmd.CommandType = Type;

            foreach (KeyValuePair<object, object> kvp in Para)
            {
                cmd.Parameters.AddWithValue(kvp.Key.ToString(), kvp.Value.ToString());
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
    }

    public static class DataTableExtensions
    {
        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                //--------- change from here
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
                //--------- change up to here
            }
            return dynamicDt;
        }
    }

    public static class HashSecurityKey
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

        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes);
        }
    }

    public static class Lib
    {
        public static bool CheckClientIp()
        {
            var result = GetClientIp().Result;
            JObject jObject = JObject.Parse(result);
            string ip = jObject["ip"].ToString();
            if (ip == "118.69.224.243" || ip == "115.73.214.199" || ip == "118.69.224.168" || ip == "118.70.171.215")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private static async Task<string> GetClientIp()
        {
            var url = "https://jsonip.com";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();

                    return strResult;
                }
                else
                {
                    return null;
                }
            }
        }


    }

    public static class Error
    {
        public static string Show(string _language, string _ErrorCode)
        {
            string mes = "";
            string sql = "SELECT MESSAGE_CONTENT FROM M_MESSAGE WHERE MES_PRI_KEY = '" + _language + "' AND MES_SEC_KEY = '" + _ErrorCode + "'";
            mes = DBHelper.GetColumnVal(sql, "MESSAGE_CONTENT", CommandType.Text);
            return mes;
        }
    }

    public static class EmailHelper
    {
        public static void SyncEmail(int _StaffId, string _StaffEmail, string _StaffPassEmail, DateTime _maxDate)
        {
            try
            {
                //Create library 
                //DataLinQDataContext db = new DataLinQDataContext();
                //FunctionCommon lib = new FunctionCommon();

                try
                {
                    string emails = _StaffEmail;
                    //db.UPDATE_SYNC_TIME(_StaffId);

                    Dictionary<int, Message> messages = new Dictionary<int, Message>();
                    Pop3Client pop3Client = new Pop3Client();
                    if (pop3Client.Connected)
                        pop3Client.Disconnect();
                    pop3Client.Connect("pop.gmail.com", 995, true);

                    pop3Client.Authenticate("recent:" + _StaffEmail, HashSecurityKey.DecryptString(_StaffPassEmail), AuthenticationMethod.UsernameAndPassword);
                    
                    int count = pop3Client.GetMessageCount();

                    int success = 0;
                    int fail = 0;
                    int count_date = 0;
                    //DateTime _maxDate = DateTime.Parse(row.DATE_SYNC.ToString());

                    for (int i = count; i >= 1; i -= 1)
                    {
                        try
                        {
                            Message message = pop3Client.GetMessage(i);
                            MessagePart body = message.FindFirstHtmlVersion();
                            string nameEmail = message.Headers.From.DisplayName;
                            string fromEmail = message.Headers.From.Address;
                            string emailSubject = message.Headers.Subject;
                            string _mesageId = message.Headers.MessageId;
                            emailSubject = emailSubject.Replace("\"", "");
                            DateTime emailDateSent = message.Headers.DateSent.ToLocalTime();
                            if (emailDateSent < _maxDate)
                            {
                                count_date++;
                                if (count_date > 1)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                string value = "Thông báo có giao dịch mới | IMM Group [Mã GD:7546436436";
                                int position = value.IndexOf("[Mã GD:");
                                if (position < 0)
                                    continue;
                                Console.WriteLine("Key: {0}, Value: '{1}'",
                                               value.Substring(0, position),
                                               value.Substring(position + 1));
                                #region Tam close
                                //if (CheckListEmailNotGet(emailSubject, fromEmail) != true)
                                //{
                                //    if (ExitsEmaiDB(_mesageId, emailDateSent, emailSubject) != true)
                                //    {
                                //        // Add the message to the dictionary from the messageNumber to the Message
                                //        messages.Add(i, message);

                                //        string listto = null;
                                //        string cc = null;
                                //        string bcc = null;
                                //        for (int c = 0; c < message.Headers.To.Count; c++)
                                //        {
                                //            if (message.Headers.To[c].MailAddress != null)
                                //            {
                                //                listto += message.Headers.To[c].MailAddress.Address + ";";
                                //            }
                                //        }
                                //        for (int c = 0; c < message.Headers.Cc.Count; c++)
                                //        {
                                //            if (message.Headers.Cc[c].MailAddress != null)
                                //            {
                                //                cc += message.Headers.Cc[c].MailAddress.Address + "; ";
                                //            }
                                //        }
                                //        for (int c = 0; c < message.Headers.Bcc.Count; c++)
                                //        {
                                //            if (message.Headers.Bcc[c].MailAddress != null)
                                //            {
                                //                bcc += message.Headers.Bcc[c].MailAddress.Address + "; ";
                                //            }
                                //        }

                                //        //Body Email START
                                //        string emailBody = null;
                                //        if (body != null)
                                //        {
                                //            emailBody = body.GetBodyAsText();
                                //        }
                                //        else
                                //        {
                                //            body = message.FindFirstPlainTextVersion();
                                //            if (body != null)
                                //            {
                                //                emailBody = body.GetBodyAsText();
                                //            }
                                //        }
                                //        //Body Email END                                   

                                //        //Check role sender start
                                //        var result = LookupRoleSender(fromEmail);
                                //        string _roleSender = result._role;
                                //        string _staffId = result._staffId;
                                //        string _cusId = result._cusId;
                                //        int idemail = 0;

                                //        //Người gửi là khách hàng
                                //        if (_roleSender == "isCus")
                                //        {
                                //            SaveEmailtoDB(message, Convert.ToInt32(_cusId), _StaffEmail, 0, "in");
                                //        }
                                //        //Người gửi là nhân viên
                                //        else if (_roleSender == "isStaff")
                                //        {
                                //            for (int c = 0; c < message.Headers.To.Count; c++)
                                //            {
                                //                if (message.Headers.To[c].MailAddress != null)
                                //                {
                                //                    listto = message.Headers.To[c].MailAddress.Address;
                                //                    Dictionary<string, string> para = new Dictionary<string, string>() { { "p_EMAIL", listto } };
                                //                    foreach (DataRow row in (DBHelper.DB_ToDataTable("[dbo].[GET_ID_CUS_SCANMAIL]", para, CommandType.StoredProcedure)).Rows)
                                //                    {
                                //                        _cusId = row["CUS_ID"].ToString();
                                //                    }

                                //                }
                                //            }
                                //            if (_cusId != "")
                                //            {
                                //                SaveEmailtoDB(message, Convert.ToInt32(_cusId), _StaffEmail, 4, "out");
                                //            }
                                //            else if (_cusId == "")
                                //            {
                                //                int idcode = 0;
                                //                for (int c = 0; c < message.Headers.Cc.Count; c++)
                                //                {
                                //                    if (message.Headers.Cc[c].MailAddress != null)
                                //                    {
                                //                        var qryidcc = (from x in db.M_CUSTOMERs
                                //                                       where x.CUS_EMAIL.Contains(message.Headers.Cc[c].MailAddress.Address) && x.FLAG_ACTIVE == 1
                                //                                       select new
                                //                                       {
                                //                                           idcus = x.CUS_ID
                                //                                       }).FirstOrDefault();
                                //                        if (qryidcc != null)
                                //                        {
                                //                            idcode = qryidcc.idcus;
                                //                            break;
                                //                        }
                                //                    }
                                //                }
                                //                if (idcode != 0)
                                //                {
                                //                    SaveEmailtoDB(message, Convert.ToInt32(idcode), _StaffEmail, 4, "out");
                                //                }
                                //                else
                                //                {
                                //                    string stemp = emailSubject.ToLower();
                                //                    if (stemp.Contains("mkh") == true)
                                //                    {
                                //                        SaveEmailtoDB(message, int.Parse(CheckSubjectAndSave(message, stemp, "mkh", 3, _StaffEmail)), _StaffEmail, 4, "out");
                                //                    }
                                //                    else if (stemp.Contains("code") == true)
                                //                    {
                                //                        SaveEmailtoDB(message, int.Parse(CheckSubjectAndSave(message, stemp, "code", 4, _StaffEmail)), _StaffEmail, 4, "out");
                                //                    }
                                //                    else if (stemp.Contains("file") == true)
                                //                    {
                                //                        SaveEmailtoDB(message, int.Parse(CheckSubjectAndSave(message, stemp, "file", 4, _StaffEmail)), _StaffEmail, 4, "out");
                                //                    }
                                //                }
                                //            }
                                //        }
                                //        else
                                //        {
                                //            int idcode = 0;
                                //            for (int c = 0; c < message.Headers.To.Count; c++)
                                //            {
                                //                if (message.Headers.To[c].MailAddress != null)
                                //                {
                                //                    var qryidto = (from x in db.M_CUSTOMERs
                                //                                   where x.CUS_EMAIL.Contains(message.Headers.To[c].MailAddress.Address) && x.FLAG_ACTIVE == 1
                                //                                   select new
                                //                                   {
                                //                                       idcus = x.CUS_ID
                                //                                   }).FirstOrDefault();
                                //                    if (qryidto != null)
                                //                    {
                                //                        idcode = qryidto.idcus;
                                //                        break;
                                //                    }
                                //                }
                                //            }
                                //            if (idcode == 0)
                                //            {
                                //                for (int c = 0; c < message.Headers.Cc.Count; c++)
                                //                {
                                //                    if (message.Headers.Cc[c].MailAddress != null)
                                //                    {
                                //                        var qryidcc = (from x in db.M_CUSTOMERs
                                //                                       where x.CUS_EMAIL.Contains(message.Headers.Cc[c].MailAddress.Address) && x.FLAG_ACTIVE == 1 //&& !String.IsNullOrEmpty(x.CUS_EMAIL)
                                //                                       select new
                                //                                       {
                                //                                           idcus = x.CUS_ID
                                //                                       }).FirstOrDefault();
                                //                        if (qryidcc != null)
                                //                        {
                                //                            idcode = qryidcc.idcus;
                                //                            break;
                                //                        }
                                //                    }
                                //                }
                                //                if (idcode == 0)
                                //                {
                                //                    var qryidfw = (from x in db.M_CUSTOMERs
                                //                                   join y in db.M_EMAILs on x.CUS_ID equals y.CUS_ID
                                //                                   where y.EMAIL_FROM == _StaffEmail && ("fw: " + y.EMAIL_SUBJECT.ToLower() == emailSubject.ToLower() || "fwd: " + y.EMAIL_SUBJECT.ToLower() == emailSubject.ToLower() || "forward: " + y.EMAIL_SUBJECT.ToLower() == emailSubject.ToLower()) && x.FLAG_ACTIVE == 1
                                //                                   orderby y.EMAIL_DATE descending
                                //                                   select new
                                //                                   {
                                //                                       idcus = x.CUS_ID
                                //                                   }).FirstOrDefault();
                                //                    if (qryidfw != null)
                                //                    {
                                //                        idcode = qryidfw.idcus;
                                //                    }
                                //                }
                                //            }
                                //            if (idcode != 0)
                                //            {
                                //                SaveEmailtoDB(message, Convert.ToInt32(_cusId), _StaffEmail, 0, "in");
                                //            }
                                //            else
                                //            {
                                //                string stemp = emailSubject.ToLower();

                                //                if (stemp.Contains("mkh") == true)
                                //                {
                                //                    SaveEmailtoDB(message, int.Parse(CheckSubjectAndSave(message, stemp, "mkh", 3, _StaffEmail)), _StaffEmail, 0, "in");
                                //                }
                                //                else if (stemp.Contains("code") == true)
                                //                {
                                //                    SaveEmailtoDB(message, int.Parse(CheckSubjectAndSave(message, stemp, "code", 4, _StaffEmail)), _StaffEmail, 0, "in");
                                //                }
                                //                else if (stemp.Contains("file") == true)
                                //                {
                                //                    SaveEmailtoDB(message, int.Parse(CheckSubjectAndSave(message, stemp, "file", 4, _StaffEmail)), _StaffEmail, 0, "in");
                                //                }
                                //            }
                                //        }
                                //        success++;
                                //    }
                                //}
                                #endregion
                            }
                        }
                        catch (Exception e)
                        {
                            fail++;
                        }

                        //progressBar.Value = (int)(((double)(count - i) / count) * 100);
                    }
                    messages.Clear();

                }
                catch (Exception e)
                {

                }

            }

            catch (Exception e)
            {
                //MessageBox.Show(this, "Error occurred retrieving mail. " + e.Message, "POP3 Retrieval");
            }
        }



        private static bool ExitsEmaiDB(string _mesageId, DateTime emailDateSent, string emailSubject)
        {
            bool flag = false;
            string MessageId = "";
            //DataLinQDataContext db = new DataLinQDataContext();
            MessageId = DBHelper.GetColumnVal("SELECT MessageID FROM [ImmiCRM].[dbo].[M_EMAIL] WHERE MessageID = '" + _mesageId + "'", "MessageID");
            if (MessageId != "")
            {
                return true;
            }
            string emsubject = "sbnull";
            if (emailSubject != null)
            {
                emsubject = emailSubject;
            }

            Dictionary<object, object> para = new Dictionary<object, object>() { { "p_DATE", emailDateSent }, { "p_SUBJECT", emsubject } };
            foreach (DataRow row in DBHelper.DB_ToDynamicTable("[dbo].[_2019_checkExitsEmailPOP]", para, CommandType.StoredProcedure))
            {
                if (_mesageId != row["MessageID"].ToString() && row["MessageID"].ToString() == "")
                {
                    DBHelper.ExecuteQuery("UPDATE [ImmiCRM].[dbo].[M_EMAIL] SET MessageID = '" + _mesageId + "' WHERE EMAIL_ID = " + row["EMAIL_ID"].ToString());
                    return true;
                }
            }           
            return flag;
        }

        static (string _role, string _staffId, string _cusId) LookupRoleSender(string fromEmail)
        {
            DataLinQDataContext db = new DataLinQDataContext();
            string _role = ""; string _staffId = ""; string _cusId = "";
            //Check who is sender start
            foreach (var s in (from st in db.M_STAFFs where st.STAFF_EMAIL == fromEmail select st))
            {
                _role = "isStaff";
                _staffId = s.STAFF_ID.ToString();
                return (_role, _staffId, _cusId);
            }
            foreach (var c in db.GET_ID_CUS_SCANMAIL(fromEmail))
            {
                _role = "isCus";
                _cusId = c.CUS_ID.ToString();
                return (_role, _staffId, _cusId);
            }
            //Check who is sender end
            return (_role, _staffId, _cusId);
        }

        private static string CheckSubjectAndSave(Message _message, string _stemp, string _str, int _num, string _staffEmail)
        {
            DataLinQDataContext db = new DataLinQDataContext();
            string _cusId = "";
            bool isNumeric = true;
            int position = _stemp.LastIndexOf(_str) + _num;
            string stempCode = _stemp.ToString().Substring(position).Trim();
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
                    _cusId = stempCode;
                }
            }
            return _cusId;
        }

        private static void SaveEmailtoDB(Message message, int _cusId, string _staffId, int flag, string inout_flag)
        {
            //DataLinQDataContext db = new DataLinQDataContext();
            int idemail = 0;
            MessagePart body = message.FindFirstHtmlVersion();
            string emailSubject = message.Headers.Subject;
            string _mesageId = message.Headers.MessageId;
            emailSubject = emailSubject.Replace("\"", "");

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

            //Body Email START
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
            //Body Email END  
            Dictionary<object, object> para = new Dictionary<object, object>() { 
                { "@P_Mode", 1 },
                { "@P_EMAIL_ID", idemail },
                { "@P_CUS_ID", _cusId },
                { "@P_EMAIL_HEADER", message.Headers.From.DisplayName },
                { "@P_EMAIL_FROM", message.Headers.From.Address },
                { "@P_EMAIL_TO", listto },
                { "@P_EMAIL_CC", cc },
                { "@P_EMAIL_BCC", bcc },
                { "@P_EMAIL_SUBJECT", emailSubject },
                { "@P_EMAIL_DATE", message.Headers.DateSent.ToLocalTime() },
                { "@P_EMAIL_CONTENT", ConvertBodyEmail(emailBody) },
                { "@P_STAFF_EMAIL", _staffId },
                { "@P_FLAG_ACTIVE", flag } ,
                { "@P_MESSAGEID", _mesageId }
            };
            foreach (DataRow row in DBHelper.DB_ToDynamicTable("[dbo].[EMAIL_HANDLING]", para, CommandType.StoredProcedure))
            {                
                idemail = Convert.ToInt32(row["SCOPE_IDENTITY"].ToString());
                if (idemail != 0)
                {
                    string nameFile = null;
                    string fileType = null;
                    byte[] emailAttach = null;
                    List<MessagePart> attachments = message.FindAllAttachments();
                    foreach (MessagePart attachment in attachments)
                    {
                        nameFile = attachment.FileName.Replace(",", "");
                        fileType = attachment.ContentType.MediaType;
                        emailAttach = attachment.Body;
                        string folder = DateTime.Now.ToString("ddMMyyyyhhmmss");
                        string sDirPath = "C:\\inetpub\\wwwroot\\crm.imm.group\\files\\email\\" + _cusId + "\\" + inout_flag + "\\" + folder;
                        DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);
                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        FileStream objfilestream = new FileStream(sDirPath + "/" + nameFile, FileMode.Create, FileAccess.ReadWrite);
                        objfilestream.Write(emailAttach, 0, emailAttach.Length);
                        objfilestream.Close();
                        db.ATTACH_HANDLING(idemail, 99999, nameFile, fileType, "/files/email/" + _cusId + "/" + inout_flag + "/" + folder + "/" + nameFile, 1, "", "");
                    }
                }
            }
        }

        private static bool CheckListEmailNotGet(string _subject, string _emailfrom)
        {
            bool flag = false;
            switch (_emailfrom)
            {
                //workplace noti email
                case "notification@fbworkmail.com":
                    flag = true;
                    break;
                case "calendar-notification@google.com":
                    flag = true;
                    break;
            }

            switch (_subject)
            {
                //workplace noti email
                case "Thông báo: khách hàng có trao đổi qua CRM":
                    flag = true;
                    break;
                case "Có nhân viên chỉnh sửa thông tin khách hàng":
                    flag = true;
                    break;
                case "Có nhân viên vừa thao tác file hồ sơ của khách hàng":
                    flag = true;
                    break;
            }

            return flag;
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
