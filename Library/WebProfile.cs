using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Library
{
    public class JsonResult
    {
        public string Index { get; set; }
        public string Key { get; set; }
        public string Val { get; set; }
    }
    public class WebProfile
    {
        public const int g_mode_insert = 1;
        public const int g_mode_update = 2;
        public const int g_mode_delete = 3;
        public const int g_mode_deleteRSIF = 4;
        public const int g_mode_deleteRSMK = 5;
        public const int g_mode_deleteRSALL = 6;
        public static string g_error = "";
        public static Boolean g_stt = false;
        public static string g_idcus = "";
        public static string g_userid = "";
        public static string g_username = "";
        public static string g_usermail = "";
        public static string g_userpicture = "";
        public static string g_feedbackid = "";
    }

    public class CLASSIFICATION_data
    {
        public string code { get; set; }
        public string code1 { get; set; }
        public string code2 { get; set; }
        public string name { get; set; }
        public string keyword { get; set; }
        public string f_active { get; set; }
        public string url { get; set; }
        public string note { get; set; }
        public string repared1 { get; set; }
        public string repared2 { get; set; }
    }

    public class EMAIL_COUNT
    {
        public string pendding { get; set; }
        public string dratfs { get; set; }
        public string survey { get; set; }
    }

    public class EMAIL_data
    {
        public static string EMAIL_ID;
        public static string CUS_ID;
        public static string EMAIL_HEADER;
        public static string EMAIL_FROM;
        public static string EMAIL_TO;
        public static string EMAIL_CC;
        public static string EMAIL_BCC;
        public static string EMAIL_SUBJECT;
        public static DateTime EMAIL_DATE = DateTime.Now;
        public static string EMAIL_CONTENT;
        public static string STAFF_EMAIL;
        public static string FLAG_ACTIVE;
        public static string EMAIL_ID_OUT;

    }

    public class Customer
    {
        public static string CUS_ID;
        public static string STATUS_ID;
        public static string CUS_NAME_VN;
        public static string CUS_NAME_ENG;
        public static string CUS_EMAIL;
        public static string CUS_PHONE;
        public static string CUS_BIRTH;
        public static DateTime CUS_BIRTH_FULL;
        public static string CUS_SEX;
        public static string CUS_MARITAL;
        public static string CUS_ADDRESS;
        public static string CUS_TOTAL_ASSET;
        public static string CUS_PASS;
        public static string CUS_REVIEW;
        public static string NOTE;
        public static string FLAG_ACTIVE;
        public static string CUS_VIP_CODE;
        public static string PARTNER_ID;
        public static string CUS_CHILDREN;
        public static string CUS_RELATIVES;
        public static string CUS_RELATIVES_FOREIGN;
        public static string CUS_RESIDING_ABROAD;
        public static string CUS_APPLIED;
        public static string CUS_ID_OUT;
    }

    public class Marketing
    {
        public static string MAR_ID;
        public static string CUS_ID;
        public static string NATION_ID;
        public static string MAR_CONTACT_DATE;
        public static string MAR_MEET_CUS_DATE;
        public static string MAR_CONTRACT_DATE;
        public static string MAR_VISA_DATE;
        public static string MAR_CUS_EVALUATION;
        public static string MAR_CUS_CONTACT_METHOD;
        public static string MAR_CUS_SOURCE;
        public static string MAR_CUS_SOURCE_NOTE;
        public static string MAR_CUS_INTEREST;
        public static string MAR_PARTNER_ID;
        public static string MAR_VIP_CARD;
        public static string MAR_CUS_WARNING;
        public static string MAR_OFFICE;
        public static string FLAG_ACTIVE;
    }

    public class Relationship
    {
        public static string CUS_ID;
        public static string RS_CODE1;
        public static string RS_CODE2;
        public static string RS_NOTE;
        public static string RS_PREPARED1;
        public static string RS_PREPARED2;
    }

    public class Education
    {
        public static string EDU_ID;
        public static string CUS_ID;
        public static string NATION_ID;
        public static string EDU_NAME;
        public static string EDU_START_DATE;
        public static string EDU_END_DATE;
        public static string EDU_MAJOR;
        public static string EDU_GRADE;
        public static string EDU_DOCUMENT;
    }

    public class STAFF_data
    {
        public int STAFF_ID { get; set; }
        public string STAFF_NAME { get; set; }
        public string STAFF_NAME_OTHER { get; set; }
        public DateTime STAFF_BIRTH { get; set; }
        public string STAFF_EMAIL { get; set; }
        public string STAFF_PASS_CRM { get; set; }
        public string STAFF_PASS_EMAIL { get; set; }
        public string STAFF_PERMISSION { get; set; }
        public string STAFF_AVATAR_LINK { get; set; }
        public string STAFF_PHONE { get; set; }
        public string STAFF_POSITION { get; set; }
        public string EMAIL_HEADER { get; set; }
        public string SIGNATURE_EMAIL { get; set; }
        public string STAFF_OFFICE { get; set; }
        public string STAFF_GROUP { get; set; }
        public string STAFF_GROUP_ON { get; set; }
        public int FLAG_ACTIVE { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
    }

    public class PARTNER_data
    {
        public int PARTNER_ID { get; set; }
        public string PARTNER_NAME { get; set; }
        public string PARTNER_EMAIL { get; set; }
        public string PARTNER_PHONE { get; set; }
        public string PARTNER_ADDRESS { get; set; }
        public string PARTNER_PASS { get; set; }
        public string PARTNER_PREPARED1 { get; set; }
        public string PARTNER_PREPARED2 { get; set; }
        public int FLAG_ACTIVE { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
    }

    public class EVENT_data
    {
        public int EVENT_CODE1 { get; set; }
        public string EVENT_CODE2 { get; set; }
        public string EVENT_NAME { get; set; }
        public int EVENT_POSITION { get; set; }
        public DateTime EVENT_DATE { get; set; }
        public string EVENT_KEYWORD { get; set; }
        public int EVENT_FLAG_ENABLE { get; set; }
        public string EVENT_URL { get; set; }
        public string EVENT_NOTE { get; set; }
        public string EVENT_PREPARED1 { get; set; }
        public string EVENT_PREPARED2 { get; set; }
    }
}
