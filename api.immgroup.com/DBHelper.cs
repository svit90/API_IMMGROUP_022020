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
using Library;

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
    }
}
