﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using api.immgroup.com;
using api.immgroup.com.Models;
using Microsoft.AspNetCore.Cors;
using Library;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text;

namespace api.immgroup.com.Controllers
{
    [ApiController]
    public class crmController : ControllerBase
    {
        [Produces("application/json")]
        [Route("crm/")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public string default_string()
        {
            try
            {
                return "Welcome To IMM GROUP API";
            }
            catch (Exception)
            {
                return "Welcome To IMM GROUP API";
            }
        }

        [Produces("application/json")]
        [Route("crm/get/message/{lang}/{code}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetMessageByCode(string lang, string code)
        {
            //[_0620_Workbase_GetMessage_Lang_Code]
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetMessage_Lang_Code]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_language = lang, @P_code = code }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/emailheader/{mode}/{email}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetEmailHeader(string mode, string email)
        {
            try
            {
                const string sql = "[dbo].[_0620_Workbase_GetEmailHeader_ByEmail]";

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_mode = mode, @P_email = email }, commandType: CommandType.StoredProcedure).ToList();

                    var response = new
                    {
                        ok = true,
                        customers = items,
                    };

                    return new OkObjectResult(response);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }


        [Produces("application/json")]
        [Route("crm/login/{email}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetStaff(string email)
        {
            try
            {
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[wb_FUNCTION_Login_Oop_define]";

                    var items = db.Query<dynamic>(sql: sql, param: new { @Email = email }, commandType: CommandType.StoredProcedure).ToList();

                    var response = new
                    {
                        ok = true,
                        customers = items,
                    };

                    return new OkObjectResult(response);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }


        #region SEARCH BAR ON TOP
        /*FUNCTION API FOR SEARCH BAR START*/
        [Produces("application/json")]
        [Route("crm/search/{key}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetInfoUserById(string key)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_012020_CRM_V3_FUNC_Search_All_Cus]";

                    var items = db.Query<dynamic>(sql: sql, param: new { @Key = key }, commandType: CommandType.StoredProcedure).ToList();

                    var response = new
                    {
                        ok = true,
                        customers = items,
                    };

                    return new OkObjectResult(response);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/unlimited/search/{key}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetInfoUserByIdUnLimited(string key)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_012020_CRM_V3_FUNC_Search_All_Cus]";

                    var items = db.Query<dynamic>(sql: sql, param: new { @Key = key }, commandType: CommandType.StoredProcedure).ToList();

                    var response = new
                    {
                        ok = true,
                        customers = items,
                    };

                    return new OkObjectResult(response);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/{staff}/search/{key}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetInfoUserByIdLimited(string key, string staff)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_012020_CRM_V3_FUNC_Search_Limited_Cus]";

                    var items = db.Query<dynamic>(sql: sql, param: new { @Key = key, @StaffId = staff}, commandType: CommandType.StoredProcedure).ToList();

                    var response = new
                    {
                        ok = true,
                        customers = items,
                    };

                    return new OkObjectResult(response);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("om/search/{key}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetOmtopic(string key)
        {
            try
            {
                Function fc = new Function();
                key = fc.ConvertName(key);
                string _dataFolder ="0";
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[OM_Search_Topic]";

                    var items = db.Query<dynamic>(sql: sql, param: new { @Key = key, @Folder = _dataFolder }, commandType: CommandType.StoredProcedure).ToList();

                    var response = new
                    {
                        ok = true,
                        customers = items,
                    };

                    return new OkObjectResult(response);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [HttpPost("om/search-memo/")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public async Task<IActionResult> OmSearchMemo([FromBody] dynamic body)
        {
            dynamic para = JObject.Parse(body.ToString());
            Function fc = new Function();
            string _key = para._key;
            string _dataFolder = para._dataFolder;
            if (_key != "" && _key != null)
            {
                _key = fc.ConvertName(_key);
            }
            else
            {
                _key = "searchOMall";
            }
            
            using (var db = new SqlConnection(DBHelper.connectionString))
            {
                const string sql = "[dbo].[OM_Search_Topic]";

                var items = db.Query<dynamic>(sql: sql, param: new { @Key = _key, @Folder = _dataFolder }, commandType: CommandType.StoredProcedure).ToList();

                var response = new
                {
                    ok = true,
                    customers = items,
                };

                return new OkObjectResult(response);
            }
        }
        /*FUNCTION API FOR SEARCH BAR END*/
        #endregion

        #region FUNCTION API FOR MENU
        /*FUNCTION API FOR MENU START*/
        [Produces("application/json")]
        [Route("crm/menu/all/{token}/")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetAllMenu(string token)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_012020_CRM_V3_api_get_All_Menu]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @Token = token }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        /*FUNCTION API FOR MENU END*/
        #endregion

        #region FUNCTION API FOR GET DATA

        #region Nhân viên và Team
        [Produces("application/json")]
        [Route("crm/get/staff/all")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetAllStaff()
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetAllStaff]";
                    var items = db.Query<dynamic>(sql: sql, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/team/all")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetTeams()
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetAllStaff]";
                    var items = db.Query<dynamic>(sql: sql, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/team/{id}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetStaffofTeams(string id)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetAllStaff]";
                    var items = db.Query<dynamic>(sql: sql, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/teams/{token}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetTeamsDetails(string token)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetAllTeam_ByToken]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Token = token }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        [Produces("application/json")]
        [Route("crm/get/teams/product/{token}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetTeamsProductDetails(string token)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetAllTeamProduct_ByToken]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Token = token }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        #endregion

        #region Quản lý tài sản
        [Produces("application/json")]
        [Route("crm/get/device/all")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetDeviceAll()
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetAllDevice]";
                    var items = db.Query<dynamic>(sql: sql, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        [Produces("application/json")]
        [Route("crm/get/device/{key}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetDeviceByKey(string key)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetDeviceByKey]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @Barcode = key }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/basiccode/{key}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetBasicCodeAll(string key)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetBasicCodeCommon]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Mode = key }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/basiccode/search/{key}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetBasicCodeSearchByKey(string key)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetBasicCodeCommonByKey]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_CODE1 = key }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region IMM GROUP DRIVE STORAGE
        [Produces("application/json")]
        [Route("crm/get/staff/permisson/{folder}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetStaffPermissFolder(string folder)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetAllStaffPermiss_byFoldername]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_FolderNAme = folder }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region Notification
        [Produces("application/json")]
        [Route("crm/get/notification/{mode}/{rowid}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetConversation_ByStaff(string mode, string rowid)
        {
            try
            {
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetNotification_ByMode]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Mode = mode, @P_Token = rowid }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/noticenter/{mode}/{rowid}/{cur}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetConversation_ByStaff(string mode, string rowid, string from, string cur)
        {
            try
            {
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetNotification_ByMode_LoadMore]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Mode = mode, @P_Token = rowid, @P_CurId = cur }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/notification/{mode}/{rowid}/{lastindex}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetNotification_ByStaff_WithLastIndex(string mode, string rowid, int lastindex)
        {
            try
            {
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetNotification_ByMode_WithLastIndex]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Mode = mode, @P_Token = rowid, @LastIndex = lastindex }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region Setting System
        [Produces("application/json")]
        [Route("crm/get/setting/function/{code}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetFunctionDetails(string code)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_Setting_GetAllFunction]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Key = code }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region Template Email
        [Produces("application/json")]
        [Route("crm/get/template/details/{code}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetTeamPlateDetails(string code)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[2019_LOAD_EMAIL_TEMPLATE_BYID]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_STAFF_ID = code }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region Khách Hàng
        [Produces("application/json")]
        [Route("crm/get/cus/profile/{id}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetCusProfileDetails(int id)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[LOAD_CUS_INFOR]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_CUSID = id }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region Chia Lead
        [Produces("application/json")]
        [Route("crm/get/sas/total-count/{id}/byproduct")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetSASTotalCountEveryStaff(string id)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_SAS_Allocation_GetTotalCount_ByProduct]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @Product = id }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region Export Khách và Report
        [Produces("application/json")]
        [Route("crm/get/report/{mode}/contact/chart/all/week")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetcContactChartAllWeek(string mode)
        {
            try
            {
                string sql = "";
                if (mode == "c")
                {
                    sql = "[dbo].[REPORT_CUS_CONTACT_ALL_WEEK_COUNT]";
                }
                if (mode == "s")
                {
                    sql = "[dbo].[REPORT_STAFF_CONTACT_ALL_WEEK_COUNT]";
                }
                using (var db = new SqlConnection(DBHelper.connectionString))
                {    

                    var items = db.Query<dynamic>(sql: sql, commandType: CommandType.StoredProcedure).ToList();

                    var response = new
                    {
                        ok = true,
                        Version = items,
                    };

                    return new OkObjectResult(response);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/report/digital/{mode}/from/{fd}/{fm}/{fy}/to/{td}/{tm}/{ty}/{office}/{flag1}/{flag2}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetDIGTotalCountEveryReport(string mode, string fd, string fm, string fy, string td, string tm, string ty, string office, string flag1, string flag2)
        {
            try
            {
                if (office == "all") { office = ""; }
                if (office == "hanoi"){office = "Hà Nội";}
                if (office == "hochiminh") { office = "Hồ Chí Minh"; }
                string dfrom = fd + "/" + fm + "/" + fy;
                string dto = td + "/" + tm + "/" + ty;
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetFormWebsiteCountAll]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Mode = mode, @P_Begindate = dfrom, @P_Enddate = dto , @P_Area = office, @P_Flag1 = flag1, @P_Flag2 = flag2 }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Route("crm/get/export/customer/itemquery")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        [HttpGet]
        [HttpPost]
        public IActionResult ExportListCus(string Product, string ProfileStatus, string SeriousRate, string Evaluation)
        {
            try
            {
                Function Lib = new Function();
                Product = Lib.SplitAndAddSeparator(Product);
                ProfileStatus = Lib.SplitAndAddSeparator(ProfileStatus);
                SeriousRate = Lib.SplitAndAddSeparator(SeriousRate);
                Evaluation = Lib.SplitAndAddSeparator(Evaluation);
                string _view = "[V_Workbase_Export_Cus_ByQuery]";
                string _sql = " SELECT ";
                _sql += "  CusId ";
                _sql += " ,CusName ";
                _sql += " ,CusEmail ";
                _sql += " ,CusPhone ";
                _sql += " ,CusGenderName ";
                _sql += " ,RegoinOfficeName ";
                _sql += " ,ProductName ";
                _sql += " ,ProfileStatusName ";
                _sql += " ,SeriousRateName ";
                _sql += " ,EvaluationStatusName ";
                _sql += " FROM " + _view + " WHERE 1 = 1 ";
                if (Product != "" && Product != "'*'")
                {
                    _sql += " AND ProductCode IN ( " + Product + " ) ";
                }
                if (ProfileStatus != "" && ProfileStatus != "'*'")
                {
                    _sql += " AND ProfileStatus IN ( " + ProfileStatus + " ) ";
                }
                if (SeriousRate != "" && SeriousRate != "'*'")
                {
                    _sql += " AND SeriousRate IN ( " + SeriousRate + " ) ";
                }
                if (Evaluation != "" && Evaluation != "'*'")
                {
                    _sql += " AND EvaluationStatus IN ( " + Evaluation + " ) ";
                }
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    var items = db.Query<dynamic>(sql: _sql,
                        commandType: CommandType.Text).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    message = "Error",
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Route("crm/get/export/get-data/{prod}/{cusstt}/{cusrc}/{cchk}/{c_s_d}/{c_s_m}/{c_s_y}/{c_e_d}/{c_e_m}/{c_e_y}/{khdchk}/{khd_s_d}/{khd_s_m}/{khd_s_y}/{khd_e_d}/{khd_e_m}/{khd_e_y}/{staffs}")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        [HttpGet]
        [HttpPost]
        public IActionResult ExportGetDataListCus(
            string prod, string cusstt, string cusrc, string order_prod, string cchk,
            string c_s_d, string c_s_m, string c_s_y, string c_e_d, string c_e_m, string c_e_y,
            string khd_s_d, string khd_s_m, string khd_s_y, string khd_e_d, string khd_e_m, string khd_e_y, string khdchk, string staffs
            )
        {
            try
            {
                Function Lib = new Function();
                string Product = Lib.SplitAndAddSeparator(prod);
                string ProfileStatus = Lib.SplitAndAddSeparator(cusstt);
                string ResourceCus = Lib.SplitAndAddSeparator(cusrc);
                string cocfrom = c_s_d + "/" + c_s_m + "/" + c_s_y;
                string cocto = c_e_d + "/" + c_e_m + "/" + c_e_y;
                string khdfrom = khd_s_d + "/" + khd_s_m + "/" + khd_s_y;
                string khdto = khd_e_d + "/" + khd_e_m + "/" + khd_e_y;

                string _view = "[V_Workbase_Export_Cus_ByQuery]";
                string _sql = " SELECT ";
                _sql += "  CusId ";
                _sql += " ,CusName ";
                _sql += " ,CusEmail ";
                _sql += " ,CusPhone ";
                _sql += " ,CusGenderName ";
                _sql += " ,RegoinOfficeName ";
                _sql += " ,ProductName ";
                _sql += " ,CusResourceName ";
                _sql += " ,ProfileStatusName ";
                _sql += " ,SeriousRateName ";
                _sql += " ,EvaluationStatusName ";
                _sql += " ,DepositContactDate ";
                _sql += " ,SignedContractDate ";
                _sql += " ,SaleAvatar ";
                _sql += " FROM " + _view ;
                if (staffs != "" && staffs != "All")
                {
                    _sql += " INNER JOIN M_RELASIONSHIP ON CUS_ID = CusId	AND RS_CODE1 = 'CS'  AND RS_CODE2 = '"+ staffs + "' ";
                }
                _sql += " WHERE 1 = 1 ";
                if (Product != "" && Product != "'All'")
                {
                    _sql += " AND ProductCode IN ( " + Product + " ) ";
                }
                if (ProfileStatus != "" && ProfileStatus != "'All'")
                {
                    _sql += " AND ProfileStatus IN ( " + ProfileStatus + " ) ";
                }
                if (ResourceCus != "" && ResourceCus != "'All'")
                {
                    _sql += " AND CusResourceCode IN ( " + ResourceCus + " ) ";
                }
              
                if (cchk == "on")
                {
                    _sql += " AND CONVERT(datetime,[O_DATE02] ,103) BETWEEN CONVERT(datetime,'"+ cocfrom + "',103)  AND  CONVERT(datetime,'" + cocto + "',103) ";
                }
                if (khdchk == "on")
                {
                    _sql += " AND CONVERT(datetime,[O_DATE03] ,103) BETWEEN CONVERT(datetime,'" + khdfrom + "',103)  AND  CONVERT(datetime,'" + khdto + "',103) ";
                }

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    var items = db.Query<dynamic>(sql: _sql,
                        commandType: CommandType.Text).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    message = "Error",
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }


        [Produces("application/json")]
        [Route("crm/get-product-list/{id}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetProductToList(int id)
        {
            string _sql = "SELECT dbo.F_GetNameBasicCode(O_PRODUCT_CODE) AS O_PRODUCT_NAME FROM M_ORDERS WHERE O_CUS_ID = @id AND O_FLAG_ACTIVE = '1'";
            string RetVal = "";

            using (SqlConnection connection = new SqlConnection(DBHelper.connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand(_sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.CommandType = System.Data.CommandType.Text;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RetVal += " - " + reader["O_PRODUCT_NAME"].ToString();
                            }
                        }
                    }

                    var response = new
                    {
                        ok = true,
                        Product = RetVal,
                    };
                    return new OkObjectResult(response);
                }
                catch (Exception e)
                {
                    var response = new
                    {
                        ok = false,
                        message = "Error",
                        error = e.Message
                    };
                    return new BadRequestObjectResult(response);
                }
            }
        }


        [Produces("application/json")]
        [Route("crm/get-staffcs-list/{id}/{mode}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public string GetStaffCsToList(int id, string mode)
        {
            string _sql = @"
        SELECT ROWID AS StaffRowId, 
               (STAFF_NAME + ' (' + STAFF_NAME_OTHER + ')') AS StaffName 
        FROM M_RELASIONSHIP 
        LEFT JOIN M_STAFF ON STAFF_ID = RS_CODE2 
        WHERE (RS_CODE1 = 'CS') AND (CUS_ID = @id)";

            string RetValId = "";
            string RetValName = "";
            string _reval = "";

            using (SqlConnection connection = new SqlConnection(DBHelper.connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand(_sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.CommandType = System.Data.CommandType.Text;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RetValId += $"<img alt=\"image\" width=\"38\" class=\"rounded-circle\" src=\"/Content/img/avatar/{reader["StaffRowId"]}.jpg\">";
                                RetValName += " - " + reader["StaffName"].ToString();
                            }
                        }

                        _reval = mode == "a" ? RetValId : RetValName;
                    }
                }
                catch (Exception e)
                {
                    _reval = e.Message;
                }
            }

            return _reval;
        }


        [Route("crm/get/export/customer/staff/{id}/following/{mode}")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult ExportListStaffFollowing(string id, string mode)
        {
            try
            {
                string _sql = "_0620_Workbase_Get_Customer_Following_ByStaffId";
                if (mode == "lite")
                {
                    _sql = "_0620_Workbase_Get_Customer_Following_ByStaffId_Lite";
                }               
                
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    var items = db.Query<dynamic>(sql: _sql, param: new { @P_STAFF = id },
                        commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    message = "Error",
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/get/export/customer/list-new/from/{fd}/{fm}/{fy}/to/{td}/{tm}/{ty}/")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult ExportListCusNew(string fd, string fm, string fy, string td, string tm, string ty)
        {
            try
            {
               
                string dfrom = fd + "/" + fm + "/" + fy;
                string dto = td + "/" + tm + "/" + ty;
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_Export_Cus_ListNew]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Begindate = dfrom, @P_Enddate = dto}, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Route("crm/get/export/customer/blocked/{id}/{office}/{sale}")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult ExportBlockedListStaffNotFollowing(string id, string office, string sale)
        {
            try
            {
                string _sql = "_0620_Workbase_Get_ListCus_TeamFlw_Over_Day";
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    var items = db.Query<dynamic>(sql: _sql, param: new { @P_Vp = office, @P_Staff = id, @P_Sale = sale },
                        commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    message = "Error",
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Route("crm/get/customer/blocked/{id}/{sale}")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult CheckBlockedCusStaffHandling(string id, string sale)
        {
            try
            {
                string _sql = "_0620_Workbase_Check_SaleHandle_BlockedCus";
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    var items = db.Query<dynamic>(sql: _sql, param: new { @P_CusId = id, @P_StaffId = sale },
                        commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    message = "Error",
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("om/get/memo/{id}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetOmtopicVersion(int id)
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[OM_Get_Topic_Version_ByVersionId]";

                    var items = db.Query<dynamic>(sql: sql, param: new { @VerID = id }, commandType: CommandType.StoredProcedure).ToList();

                    var response = new
                    {
                        ok = true,
                        Version = items,
                    };

                    return new OkObjectResult(response);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region PRM API
       
        [Produces("application/json")]
        [Route("prm/get-bank-list")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetAllBank()
        {
            try
            {

                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[PRM_Get_Bank_List]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @Para = "" },  commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("prm/get-bank-name/{id}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetBankName(string id)
        {
            try
            {
                if (id == "" || id == null)
                {
                    id = "";
                }
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[PRM_Get_Bank_List]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @Para = id }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region HRM API
        [Produces("application/json")]
        [Route("hrm/candidate/add-new")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public async Task<IActionResult> HrmCandidateAdd([FromBody] dynamic body)
        {
            string sql =""; 
            dynamic para = JObject.Parse(body.ToString());  
            using (SqlConnection connection = new SqlConnection(DBHelper.connectionString))
            {
                connection.Open();
                try
                {
                    sql = " INSERT INTO M_HRM_CANDIDATE(CANDIDATE_TOKEN ,CANDIDATE_NAME ,CANDIDATE_EMAIL ,CANDIDATE_PHONE ,CANDIDATE_GENDER ,CANDIDATE_BIRTHDAY ,CANDIDATE_POSITION ,CANDIDATE_LINKDRIVE ,CANDIDATE_STATUS ,CANDIDATE_RC ,CANDIDATE_NOTE ,CANDIDATE_APPLY_DATE ,FLAG_ACTIVE ,UPDATE_DATE ,COMPANY_APPLY) ";
                    sql += " VALUES ( ";
                    sql += " newid() , ";
                    sql += " N'" + para.rq_name + "', ";
                    sql += " N'" + para.rq_email + "', ";
                    sql += " N'" + para.rq_phone + "', ";
                    sql += " '" + para.rq_gender + "', ";
                    sql += " '', ";
                    sql += " N'" + para.rq_position + "', ";
                    sql += " '','', ";
                    sql += " N'" + para.rq_source + "', ";
                    sql += " N'" + para.rq_noted + "', ";
                    sql += " CONVERT(nvarchar(10), GETDATE(), 103) , 1, GETDATE(), ";
                    sql += " N'" + para.rq_company + "'";                  
                    sql += " );";
                    await connection.ExecuteAsync(sql, commandType: CommandType.Text);
                    var response = new { ok = true, message = "Success", error = "Thao tác hoàn tất" };
                    return new OkObjectResult(response);
                }
                catch (Exception e)
                {
                    var response = new
                    {
                        ok = false,
                        message = "Error",
                        error = e.Message
                    };

                    return new BadRequestObjectResult(response);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #endregion

        #region FUNCTION API CHECK

        [Produces("application/json")]
        [Route("crm/check/{token}/{verifycode}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult VerifySessionLoginStaff(string token, string verifycode)
        {
            try
            {
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_Verify_Login_Session_Staff]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @token = token, @verifycode = verifycode }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/check/func/{token}/{code}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult VerifyFunctionAsignedStaff(string token, string code)
        {
            try
            {
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_Setting_GetAllFunction_AsignedUser]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Token = token, @P_Code = code }, commandType: CommandType.StoredProcedure).ToList();
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }

        [Produces("application/json")]
        [Route("crm/check/access/{cusid}/{token}/{staffid}")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult VerifynAccessStaffToProfile(int staffid, string token, int cusid)
        {
            try
            {
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_Check_StatusAccessToProfile]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_StaffId = staffid, @P_Token = token, @P_CusId = cusid }, commandType: CommandType.StoredProcedure).ToList();
                    //Check
                    return new OkObjectResult(items);
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    ok = false,
                    error = e.Message
                };

                return new BadRequestObjectResult(response);
            }
        }
        #endregion

        #region FUNCTION API EXC

        [Produces("application/json")]
        [Route("crm/func/todo/addnew")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult AddNewTodoList([FromBody] TodoListModel data)
        {
            using (SqlConnection connection = new SqlConnection(DBHelper.connectionString))
            {
                connection.Open();
                try
                {
                    string RetVal = "";
                    string Query = "[dbo].[_0620_Workbase_TodoList_AddNew]";

                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_Owner", data.Owner);
                        cmd.Parameters.AddWithValue("@P_Todo", data.TaskDetails);
                        cmd.Parameters.AddWithValue("@P_Rate", data.Rate);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                RetVal = reader["TODO_ID"].ToString();
                            }
                        }

                        var response = new
                        {
                            TODO_ID = RetVal,
                        };
                        return new OkObjectResult(response);
                    }
                }
                catch (Exception e)
                {
                    var response = new
                    {
                        ok = false,
                        message = "Error",
                        error = e.Message
                    };

                    return new BadRequestObjectResult(response);
                }
            }
        }



        [HttpPost("crm/func/todolist/addnew")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public async Task<IActionResult> NewTodoList([FromBody] dynamic body)
        {
            try
            { 
                string sql = "[dbo].[_0620_Workbase_AddNew_Todolist]";
                dynamic para = JObject.Parse(body.ToString());
                int Owner = para.Owner;
                string TaskDetails = para.TaskDetails;
                int Rate = para.Rate;
                var dp = new DynamicParameters();
                dp.Add("@P_Owner", Owner);
                dp.Add("@P_Todo", TaskDetails);
                dp.Add("@P_Rate", Rate);
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    var rowAffected = await db.ExecuteAsync(sql, dp, commandType: CommandType.StoredProcedure);
                    // RETURN
                    var response = new { ok = true, rowAffected };
                    return new OkObjectResult(response);
                }

            }
            catch (Exception error)
            {
                var response = new { ok = false, error, body };
                return BadRequest(response);
            }
        }

        [HttpPost("crm/func/info/f/web/submit")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public async Task<IActionResult> InfoSubmitFromWebSite([FromBody] dynamic body)
        {
            dynamic para = JObject.Parse(body.ToString());
            RegexUtilities util = new RegexUtilities();
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            Function fc = new Function();
            int _s = 1;string _flag = "";
            string utm_source_char  = para.rq_utmSource;
            string utm_source = para.rq_utmSource;
            string _e = para.rq_email;
            string _p = para.rq_phone;
            string _n = para.rq_cusname;
            string _prod = para.rq_product;
            string Cusid = "";

            string _nextturn = DBHelper.GetColumnVal("SELECT TOP 1 TPD_STAFF_ID FROM [M_TEAMS_PRODUCTS] p INNER JOIN [M_TEAMS_PRODUCTS_DETAILS] pd ON p.TP_PROD_CODE = pd.TPD_PROD_CODE INNER JOIN M_STAFF s ON s.STAFF_ID = pd.TPD_STAFF_ID	WHERE TP_PROD_CODE = '" + para.rq_product + "' AND TPD_FLAG_MAIN = 'checked' AND ';' + p.TP_MEMBERS_ID + ';' LIKE '%;' + Convert(nvarchar,pd.TPD_STAFF_ID) + ';%' AND TPD_FLAG_ACTIVE = 1 AND TP_FLAG_ACTIVE = 1 AND s.FLAG_ACTIVE = 1	ORDER BY TPD_SAS_COUNT ASC", "TPD_STAFF_ID");

            if (_e != "")
            {
                if (util.IsValidEmail(_e.Trim()))
                {
                    DataTable dt = new DataTable();
                    dt = DBHelper.DB_ToDataTable("SELECT CUS_ID,CUS_EMAIL FROM M_CUSTOMER WHERE 1 = 1 AND CUS_EMAIL LIKE '%" + _e + "%'  AND FLAG_ACTIVE = 1");
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        string[] emailist = dtRow["CUS_EMAIL"].ToString().Split(';');
                        foreach (var eml in emailist)
                        {
                            if (eml.Trim() == _e.Trim())
                            {
                                Cusid = dtRow["CUS_ID"].ToString();
                            }
                        }
                    }
                }
            }
            if (Cusid == "" && _p != "")
            {
                if (_p.Trim() != "")
                {
                    if (regex.IsMatch(_p) == true)
                    {
                        DataTable dt = new DataTable();
                        dt = DBHelper.DB_ToDataTable("SELECT CUS_ID,CUS_PHONE FROM M_CUSTOMER WHERE 1 = 1 AND CUS_PHONE LIKE '%" + _p + "%'  AND FLAG_ACTIVE = 1");
                        foreach (DataRow dtRow in dt.Rows)
                        {
                            string[] emailist = dtRow["CUS_PHONE"].ToString().Split(';');
                            foreach (var eml in emailist)
                            {
                                if (eml.Trim() == _p.Trim())
                                {
                                    Cusid = dtRow["CUS_ID"].ToString();
                                }
                            }
                        }
                    }
                }
            }      
            string sql = "";
            utm_source = fc.CheckResourceCodeByUtm_CSV(utm_source);

            if (_e != "" || _p != "")
            {

                if (para.rq_sex != "Nam") { _s = 0; }

                if (Cusid == "" || Cusid == null)
                {
                    _flag = "new";
                    string str = "";
                    string[] AppEmail = null;
                    str = _e.ToString();
                    char[] splitchar = { ';' };
                    AppEmail = str.Split(splitchar);
                    string pass = fc.GetUniqueKey(8);
                    sql += "INSERT INTO M_CUSTOMER";
                    sql += "(STATUS_ID, CUS_NAME_VN, CUS_NAME_ENG, CUS_EMAIL, CUS_PHONE, CUS_BIRTH, CUS_BIRTH_FULL, CUS_SEX, CUS_MARITAL, CUS_ADDRESS, CUS_TOTAL_ASSET,";
                    sql += "    CUS_PASS, CUS_REVIEW, NOTE, FLAG_ACTIVE, CUS_VIP_CODE, PARTNER_ID, CUS_RELATIVES, CUS_RELATIVES_FOREIGN, CUS_CHILDREN, CUS_RESIDING_ABROAD,";
                    sql += "   CUS_APPLIED, INSERT_DATE, UPDATE_DATE, PASSCUS, APP_EMAIL, APP_USER, APP_EDIT_FLAG, APP_PASS, APP_PHONE, APP_ADDRESS, ROWID, WM_FLAG, FLAG_SEND_NOTI,";
                    sql += "   IBT_FLAG, CAC_FLAG, AVATAR_IMG, TEAM_LEADER_ASSIGNED, TEAM_MEMBER_ASSIGNED, STAFF_HANDLING)";

                    sql += " VALUES";
                    sql += " ( ";
                    sql += "'',";
                    sql += "N'" + _n + "',";
                    sql += "N'" + fc.ConvertName(_n) + "',";
                    sql += "N'" + fc.formatpm(_e.ToLower()) + "',";
                    sql += "N'" + fc.formatpm(_p) + "',";
                    sql += "N'" + DateTime.Now.ToString() + "',";
                    sql += "GETDATE(),";
                    sql += _s + ",";
                    sql += "0,";
                    sql += "'',";
                    sql += "'',";
                    sql += "'7C222FB2927D828AF22F592134E8932480637C0D',";
                    sql += "'CR01',";
                    sql += "'',";
                    sql += "1,";
                    sql += "'',";
                    sql += "'',";
                    sql += "0,";
                    sql += "'',";
                    sql += "'',";
                    sql += "'',";
                    sql += "'',";
                    sql += "GETDATE(),";
                    sql += "GETDATE(),";
                    sql += "'" + pass + "',";
                    sql += "N'" + _e.ToLower() + "',";
                    sql += "'" + AppEmail[0] + "',";
                    sql += "'0',";
                    sql += "CONVERT(VARCHAR(max), HASHBYTES('MD5', '" + pass + "'), 2),";
                    sql += "N'" + _p + "',";
                    sql += "'',";
                    sql += "(select NEWID()),";
                    sql += "'0','0','0','0','/img/avatar/cus_default_avatar.png',0,0,''";
                    sql += " ); ";
                    DBHelper.ExecuteQuery(sql);
                    Cusid = DBHelper.GetColumnVal("SELECT TOP 1 CUS_ID FROM [M_CUSTOMER] ORDER BY CUS_ID DESC", "CUS_ID");
                }
            }
            
            if (Cusid != "" && Cusid != null)
            {
                sql = " INSERT INTO M_SUBMIT_FROM_WEBSITE ( ";
                sql += " CUS_ID, ";
                sql += " S_WEB_CONTENT, ";
                sql += " S_WEB_TITLE, ";
                sql += " S_WEB_SOURCE, ";
                sql += " S_WEB_LINK, ";
                sql += " FLAG_ACTIVE, ";
                sql += " S_WEB_NOTE_1, ";
                sql += " S__WEB_NOTE_2, ";
                sql += " S_WEB_DATE";
                sql += " ) ";
                sql += " VALUES ( ";
                sql += "'" + Cusid + "', ";
                sql += " N'" + para.rq_content + "', ";
                sql += " N'" + para.rq_titleProduct + "', ";
                sql += " N'" + utm_source + "', ";
                sql += " N'" + para.rq_getLink + "', ";
                sql += " '1', ";
                sql += " N'" + para.rq_area + "', ";
                if (_flag == "new")
                {
                    sql += " N'', ";
                }
                else
                {
                    sql += " N'" + para.rq_info + "', ";
                }
                sql += " GETDATE());";
                DBHelper.ExecuteQuery(sql);
                string _wid = DBHelper.GetColumnVal("SELECT TOP 1 S_WEB_ID FROM M_SUBMIT_FROM_WEBSITE WHERE CUS_ID = " + Cusid+" ORDER BY S_WEB_ID DESC", "S_WEB_ID");
                if (_wid != "" && _wid != null && _flag == "new")
                {
                    string apiUrl = "https://system.immgroup.com/dept/saleleads-allocation/submit/" + _wid;
                    var formData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("t_cId", Cusid?.ToString() ?? ""),
                        new KeyValuePair<string, string>("sl_prod", para.rq_product?.ToString() ?? ""),
                        new KeyValuePair<string, string>("t_cusinfo",
                            $"{para.rq_cusname?.ToString() ?? ""}, {para.rq_email?.ToString() ?? ""}, {para.rq_phone?.ToString() ?? ""}, {para.rq_sex?.ToString() ?? ""}"),
                        new KeyValuePair<string, string>("t_cresource", utm_source_char?.ToString() ?? ""),
                        new KeyValuePair<string, string>("t_noted", "Khách đăng ký trên web, hệ thống chia leads tự động. Nguồn <br>" +  para.rq_utmSource),
                        new KeyValuePair<string, string>("sl_office", "OFFICE01"),
                        new KeyValuePair<string, string>("sl_location", para.rq_area?.ToString() ?? ""),
                        new KeyValuePair<string, string>("t_nextturn", _nextturn?.ToString() ?? ""),
                        new KeyValuePair<string, string>("sl_agentrf", "9"),
                        new KeyValuePair<string, string>("_webId", _wid?.ToString() ?? ""),
                        new KeyValuePair<string, string>("t_staffblock", ""),
                        new KeyValuePair<string, string>("t_feedback", para.rq_content?.ToString() ?? "")
                    };
                    HttpContent _dtcontent = new FormUrlEncodedContent(formData); 
                    HttpClient _httpClient = new HttpClient();
                    HttpResponseMessage resp = await _httpClient.PostAsync(apiUrl, _dtcontent);
                }                
            }
            var response = new { ok = true, message = "Success", error = "Thao tác hoàn tất" };
            return new OkObjectResult(response);

        }

        [HttpPost("canada/func/info/f/web/submit")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public async Task<IActionResult> InfoSubmitFromCanWebSite([FromBody] dynamic body)
        {
           string connectionStringCan = "Data Source=103.252.252.254;Initial Catalog=ImmiCan;Persist Security Info=True;User ID=sa;Password=Mekongdelta@2018";
            SqlCommand cmd_can = new SqlCommand();
            SqlDataAdapter sda_Can;
            SqlDataReader sdr_Can;
            DataSet ds_can = new DataSet();

            dynamic para = JObject.Parse(body.ToString());
            Function fc = new Function();
            int _s = 1; string _flag = "";
            string utm_source = para.rq_utmSource;
            string _e = para.rq_email;
            string _p = para.rq_phone;
            string _n = para.rq_cusname;
            string Cusid = "";
            string sql = "";
            utm_source = fc.CheckResourceCodeByUtm_CSV(utm_source);
            using (SqlConnection sqlConnection = new SqlConnection(connectionStringCan))
            {
                sqlConnection.Open();
                try
                {
                    using (cmd_can = new SqlCommand("SELECT CUS_ID FROM [M_CUSTOMER] WHERE CUS_EMAIL LIKE '%" + _e + "%' OR CUS_PHONE LIKE '%" + _p + "%'", sqlConnection))
                    {
                        sdr_Can = cmd_can.ExecuteReader();
                        if (sdr_Can.Read())
                        {
                            Cusid = sdr_Can["CUS_ID"].ToString();                            
                        }
                    }
                }
                catch { }
                finally{sqlConnection.Close();}
            }
            if (_e != "" || _p != "")
            {

                if (para.rq_sex != "Nam") { _s = 0; }

                if (Cusid == "" || Cusid == null)
                {
                    _flag = "new";
                    string str = "";
                    string[] AppEmail = null;
                    str = _e.ToString();
                    char[] splitchar = { ';' };
                    AppEmail = str.Split(splitchar);
                    string pass = fc.GetUniqueKey(8);
                    sql += "INSERT INTO M_CUSTOMER";
                    sql += "(STATUS_ID, CUS_NAME_VN, CUS_NAME_ENG, CUS_EMAIL, CUS_PHONE, CUS_BIRTH, CUS_BIRTH_FULL, CUS_SEX, CUS_MARITAL, CUS_ADDRESS, CUS_TOTAL_ASSET,";
                    sql += "    CUS_PASS, CUS_REVIEW, NOTE, FLAG_ACTIVE, CUS_VIP_CODE, PARTNER_ID, CUS_RELATIVES, CUS_RELATIVES_FOREIGN, CUS_CHILDREN, CUS_RESIDING_ABROAD,";
                    sql += "   CUS_APPLIED, INSERT_DATE, UPDATE_DATE, PASSCUS, APP_EMAIL, APP_USER, APP_EDIT_FLAG, APP_PASS, APP_PHONE, APP_ADDRESS, ROWID, WM_FLAG, FLAG_SEND_NOTI,";
                    sql += "   IBT_FLAG, CAC_FLAG, AVATAR_IMG, TEAM_LEADER_ASSIGNED, TEAM_MEMBER_ASSIGNED, STAFF_HANDLING)";

                    sql += " VALUES";
                    sql += " ( ";
                    sql += "'',";
                    sql += "N'" + _n + "',";
                    sql += "N'" + fc.ConvertName(_n) + "',";
                    sql += "N'" + _e + "',";
                    sql += "N'" + _p + "',";
                    sql += "N'" + DateTime.Now.ToString() + "',";
                    sql += "GETDATE(),";
                    sql += _s + ",";
                    sql += "0,";
                    sql += "'',";
                    sql += "'',";
                    sql += "'7C222FB2927D828AF22F592134E8932480637C0D',";
                    sql += "'CR01',";
                    sql += "'',";
                    sql += "1,";
                    sql += "'',";
                    sql += "'',";
                    sql += "0,";
                    sql += "'',";
                    sql += "'',";
                    sql += "'',";
                    sql += "'',";
                    sql += "GETDATE(),";
                    sql += "GETDATE(),";
                    sql += "'" + pass + "',";
                    sql += "N'" + _e + "',";
                    sql += "'" + AppEmail[0] + "',";
                    sql += "'0',";
                    sql += "CONVERT(VARCHAR(max), HASHBYTES('MD5', '" + pass + "'), 2),";
                    sql += "N'" + _p + "',";
                    sql += "'',";
                    sql += "(select NEWID()),";
                    sql += "'0','0','0','0','/img/avatar/cus_default_avatar.png',0,0,''";
                    sql += " ); ";
                    using (SqlConnection sqlConnection = new SqlConnection(connectionStringCan))
                    {
                        sqlConnection.Open();
                        try
                        {
                            using (cmd_can = new SqlCommand(sql, sqlConnection))
                            {
                                cmd_can.ExecuteNonQuery();
                            }
                            using (cmd_can = new SqlCommand("SELECT TOP 1 CUS_ID FROM [M_CUSTOMER] ORDER BY CUS_ID DESC", sqlConnection))
                            {
                                sdr_Can = cmd_can.ExecuteReader();
                                if (sdr_Can.Read())
                                {
                                    Cusid = sdr_Can["CUS_ID"].ToString();
                                }                                
                            }
                        }
                        catch { }
                        finally { sqlConnection.Close(); }
                    }               
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionStringCan))
            {
                connection.Open();
                try
                {
                    sql = " INSERT INTO M_SUBMIT_FROM_WEBSITE ( ";
                    sql += " CUS_ID, ";
                    sql += " S_WEB_CONTENT, ";
                    sql += " S_WEB_TITLE, ";
                    sql += " S_WEB_SOURCE, ";
                    sql += " S_WEB_LINK, ";
                    sql += " FLAG_ACTIVE, ";
                    sql += " S_WEB_NOTE_1, ";
                    sql += " S__WEB_NOTE_2, ";
                    sql += " S_WEB_DATE";
                    sql += " ) ";
                    sql += " VALUES ( ";
                    sql += "'" + Cusid + "', ";
                    sql += " N'" + para.rq_content + "', ";
                    sql += " N'" + para.rq_titleProduct + "', ";
                    sql += " N'" + utm_source + "', ";
                    sql += " N'" + para.rq_getLink + "', ";
                    sql += " '1', ";
                    sql += " N'" + para.rq_area + "', ";
                    if (_flag == "new")
                    {
                        sql += " N'', ";
                    }
                    else
                    {
                        sql += " N'" + para.rq_info + "', ";
                    }
                    sql += " GETDATE());";

                    string _sql = " INSERT INTO M_FEEDBACK (CUS_ID,STAFF_ID,FEEDBACK_DATE,FEEDBACK_CONTENT,FEEDBACK_PREPARED1,FEEDBACK_PREPARED2,FLAG_ACTIVE,INSERT_DATE,UPDATE_DATE,FLAG_SEEN,MessageID,FEEDBACK_SUBJECT,FromEmail,ToEmail,CcEmail,BccEmail,FLAG_SEND_OUT) VALUES(" + Cusid + ", 1, GETDATE(), N'Khách điền form: "+ para.rq_content + "', 'C', 'Public', 1, GETDATE(), GETDATE(), 0, '', '', '', '', '', '', 0);";
                    await connection.ExecuteAsync(sql, commandType: CommandType.Text);
                    await connection.ExecuteAsync(_sql, commandType: CommandType.Text);
                    var response = new { ok = true, message = "Success", error = "Thao tác hoàn tất" };
                    return new OkObjectResult(response);

                }
                catch (Exception e)
                {
                    var response = new
                    {
                        ok = false,
                        message = "Error",
                        error = e.Message
                    };

                    return new BadRequestObjectResult(response);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private bool ValidateSecret(string value)
        {
            return value.Equals("12C1F7EF9AC8E288FBC2177B7F54D", StringComparison.OrdinalIgnoreCase);
        }

        private void CheckAndAddCus(string _e, string _p , string _n, int _s)
        {
            string Cusid = "";
            DataTable dt = new DataTable();
            Function fc = new Function();
            Cusid = DBHelper.GetColumnVal("SELECT CUS_ID FROM [M_CUSTOMER] WHERE CUS_EMAIL LIKE '%" + _e + "%' OR CUS_PHONE LIKE '%" + _p + "%'","CUS_ID");
          
            if (Cusid == "" || Cusid == null) {           
                string str = "";
                string[] AppEmail = null;
                str = _e.ToString();
                char[] splitchar = { ';' };
                AppEmail = str.Split(splitchar);
                string pass = fc.GetUniqueKey(8);
                string _sql = "INSERT INTO M_CUSTOMER";
                _sql += "(STATUS_ID, CUS_NAME_VN, CUS_NAME_ENG, CUS_EMAIL, CUS_PHONE, CUS_BIRTH, CUS_BIRTH_FULL, CUS_SEX, CUS_MARITAL, CUS_ADDRESS, CUS_TOTAL_ASSET,";
                _sql += "    CUS_PASS, CUS_REVIEW, NOTE, FLAG_ACTIVE, CUS_VIP_CODE, PARTNER_ID, CUS_RELATIVES, CUS_RELATIVES_FOREIGN, CUS_CHILDREN, CUS_RESIDING_ABROAD,";
                _sql += "   CUS_APPLIED, INSERT_DATE, UPDATE_DATE, PASSCUS, APP_EMAIL, APP_USER, APP_EDIT_FLAG, APP_PASS, APP_PHONE, APP_ADDRESS, ROWID, WM_FLAG, FLAG_SEND_NOTI,";
                _sql += "   IBT_FLAG, CAC_FLAG, AVATAR_IMG, TEAM_LEADER_ASSIGNED, TEAM_MEMBER_ASSIGNED, STAFF_HANDLING)";

                _sql += " VALUES";
                _sql += " ( ";
                _sql += "'',";
                _sql += "N'" + _n + "',";
                _sql += "N'" + fc.ConvertName(_n) + "',";
                _sql += "N'" + _e + "',";
                _sql += "N'" + _p + "',";
                _sql += "N'" + DateTime.Now.ToString() + "',";
                _sql += "N'" + DateTime.Now + "',";
                _sql += _s + ",";
                _sql += "0,";
                _sql += "'',";
                _sql += "'',";
                _sql += "'7C222FB2927D828AF22F592134E8932480637C0D',";
                _sql += "'CR01',";
                _sql += "'',";
                _sql += "1,";
                _sql += "'',";
                _sql += "'',";
                _sql += "0,";
                _sql += "'',";
                _sql += "'',";
                _sql += "'',";
                _sql += "'',";
                _sql += "'" +  pass + "',";
                _sql += "'" + AppEmail[0] + "',";
                _sql += " ) ";
                
            }
        }
        #endregion

        #region EmailHelper
        [EnableCors("WebPolicy")]
        [HttpPost("crm/func/sendmail-async")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonResult))]
        public IActionResult SendMailAsync([FromBody] dynamic data)
        {
            try
            {
                EmailHelper emh = new EmailHelper();
                // SEND EMAIL
                // TODO: 
                //if (data.CustomerId != 12419)
                {
                    
                    var mailHeader = data.MailHeader.Value;
                    var senderEmail = data.SenderEmail.Value;
                    var senderPassword = data.SenderPassword.Value;
                    var toEmail = data.ToEmail.Value;
                    var ccEmail = data.CcEmail.Value;
                    var bccEmail = data.BccEmail.Value;
                    var subject = data.Subject.Value;
                    var bodyEmail = data.BodyEmail.Value;

                    var from = "crm@immgroup.com";
                    var password = "duyhhwbuverzqkac";

                    if (senderEmail != "" && senderEmail != from)
                    {
                        from = senderEmail;
                        password = senderPassword;
                    }

                    //SEND EMAIL
                    //EmailHelper.Send(from, password, toEmail, ccEmail, bccEmail, subject, bodyEmail, System.Net.Mail.MailPriority.High, null, mailHeader);
                    emh.SendEmailAsync(mailHeader, from, password, toEmail, ccEmail, bccEmail, subject, bodyEmail, null);
                }

                var response = new
                {
                    status = "OK",
                    message = "Email Send",
                };

                return new OkObjectResult(response);
            }
            catch (Exception e)
            {
                var response = new
                {
                    status = "Error",
                    message = e.Message
                };

                return BadRequest(response);
            }
        }
        #endregion

        #region 07-2024 API - Form submit from immgroup.com
       
        [HttpPost("crm/func/agent/register/submit")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public async Task<IActionResult> AgentRegisterSubmit([FromBody] dynamic body)
        {
            dynamic para = JObject.Parse(body.ToString());
            RegexUtilities util = new RegexUtilities();
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            Function fc = new Function();
            string utm_source = para.rq_utmSource;
            string _e = para.rq_email;
            string _p = para.rq_phone;
            string _n = para.rq_agentname;
            string _address = para.rq_address;
            string _cccd = para.rq_cccd;
            string _cccdDate = para.rq_cccdDate;
            string _company = para.rq_company;
            string _currentJob = para.rq_currentJob;
            string _fieldOfwork = para.rq_fieldOfwork;
            string _partchannel = para.rq_partchannel;
            string _cussource = para.rq_cussource;
            string _gender = para.rq_gender;
            
            //if (_e != "")
            //{
            //    if (util.IsValidEmail(_e.Trim()))
            //    {
            //        DataTable dt = new DataTable();
            //        dt = DBHelper.DB_ToDataTable("SELECT PARTNER_ID,PARTNER_EMAIL FROM M_PARTNER WHERE 1 = 1 AND PARTNER_EMAIL LIKE '%" + _e + "%'  AND FLAG_ACTIVE = 1");
            //        foreach (DataRow dtRow in dt.Rows)
            //        {
            //            string[] emailist = dtRow["PARTNER_EMAIL"].ToString().Split(';');
            //            foreach (var eml in emailist)
            //            {
            //                if (eml.Trim() == _e.Trim())
            //                {
            //                    _AgenId = dtRow["PARTNER_ID"].ToString();
            //                }
            //            }
            //        }
            //    }
            //}
            //if (_AgenId == "" && _p != "")
            //{
            //    if (_p.Trim() != "")
            //    {
            //        if (regex.IsMatch(_p) == true)
            //        {
            //            DataTable dt = new DataTable();
            //            dt = DBHelper.DB_ToDataTable("SELECT PARTNER_ID,PARTNER_PHONE FROM M_PARTNER WHERE 1 = 1 AND PARTNER_PHONE LIKE '%" + _p + "%'  AND FLAG_ACTIVE = 1");
            //            foreach (DataRow dtRow in dt.Rows)
            //            {
            //                string[] emailist = dtRow["PARTNER_PHONE"].ToString().Split(';');
            //                foreach (var eml in emailist)
            //                {
            //                    if (eml.Trim() == _p.Trim())
            //                    {
            //                        _AgenId = dtRow["PARTNER_ID"].ToString();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            string sql = "";
           
            using (SqlConnection connection = new SqlConnection(DBHelper.connectionString))
            {
                connection.Open();
                try
                {
                    if (_e != "" && _p != "" && _cccd != "")
                    {
                        sql += @"INSERT INTO M_PARTNER (PARTNER_NAME,PARTNER_EMAIL,PARTNER_PHONE,PARTNER_ADDRESS,PARTNER_PASS,PARTNER_PREPARED1,PARTNER_PREPARED2,FLAG_ACTIVE
                        ,INSERT_DATE,DATE_CONTACT,PART_CHANEL,PART_SOURCE,PART_PORTFOLIO,PART_CREATOR,ROWID,AVATAR_IMG,PARTNER_CCCD,PARTNER_CCCD_DATE,VERIFY_STEP, PARTNER_GENDER)";
                        sql += " VALUES";
                        sql += " ( ";
                        sql += "N'" + _n + "',";
                        sql += "N'" + fc.formatpm(_e.ToLower()) + "',";
                        sql += "N'" + fc.formatpm(_p) + "',";
                        sql += "N'" + _address + "',";
                        sql += "'191DC397C57B606A818664BA16B085EB06432653',";
                        sql += "N'" + _company + "',";
                        sql += "N'" + _currentJob + "',";
                        sql += "1,";
                        sql += "GETDATE(),";
                        sql += "GETDATE(),";
                        sql += "N'" + _partchannel + "',";
                        sql += "N'" + _cussource + "',";
                        sql += "N'" + _fieldOfwork + "',";
                        sql += "1,";
                        sql += "(select NEWID()),";
                        sql += "'/img/avatar/partner_default_avatar.png',";
                        sql += "N'" + _cccd + "',";
                        sql += "N'" + _cccdDate + "',";
                        sql += "0,";
                        sql +=  _gender;
                        sql += " ); ";
                        await connection.ExecuteAsync(sql, commandType: CommandType.Text);
                    }                    
                    var response = new { ok = true, message = "Success", error = "Thao tác hoàn tất" };
                    return new OkObjectResult(response);
                }
                catch (Exception e)
                {
                    var response = new
                    {
                        ok = false,
                        message = "Error",
                        error = e.Message
                    };

                    return new BadRequestObjectResult(response);
                }
                finally
                {
                    connection.Close();
                }
            }
            

        }
        #endregion

        #region API FOR AI 2025

        [HttpGet("om/regulations/all")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetAllTopicForAI()
        {
            try
            {
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[OM_Get_AllTopic_ForAI]";

                    var items = db.Query<dynamic>(sql: sql,commandType: CommandType.StoredProcedure).ToList();

                    var response = new
                    {
                        ok = true,
                        regulations = items,
                    };

                    return new OkObjectResult(response);
                }
            }
            catch (SqlException ex)
            {
                return new BadRequestObjectResult(new { ok = false, error = "Lỗi cơ sở dữ liệu" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { ok = false, error = "Lỗi không xác định" });
            }
        }
        #endregion
    }
}