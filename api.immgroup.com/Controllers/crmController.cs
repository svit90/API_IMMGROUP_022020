using System;
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
using JsonResult = Library.JsonResult;
using Newtonsoft.Json.Linq;

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
        [Route("crm/get/report/digital/{mode}/from/{fd}/{fm}/{fy}/to/{td}/{tm}/{ty}/")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult GetDIGTotalCountEveryReport(string mode, string fd, string fm, string fy, string td, string tm, string ty)
        {
            try
            {
                string dfrom = fd + "/" + fm + "/" + fy;
                string dto = td + "/" + tm + "/" + ty;
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_GetFormWebsiteCountAll]";
                    var items = db.Query<dynamic>(sql: sql, param: new { @P_Mode = mode, @P_Begindate = dfrom, @P_Enddate = dto }, commandType: CommandType.StoredProcedure).ToList();
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

        [Route("crm/get/export/customer/staff/{id}/following")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public IActionResult ExportListStaffFollowing(string id)
        {
            try
            {
                string _sql = "_0620_Workbase_Get_Customer_Following_ByStaffId";
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

        #region FUNCTION API EXT

        [Produces("application/json")]
        [Route("crm/func/todo/addnew")]
        [ProducesResponseType(200, Type = typeof(JsonResult))]
        [AllowAnonymous]
        public async Task<IActionResult> AddNewTodoList([FromBody] TodoListModel data)
        {
            try
            {
                using (var db = new SqlConnection(DBHelper.connectionString))
                {
                    const string sql = "[dbo].[_0620_Workbase_TodoList_AddNew]";
                    var items = db.Query<dynamic>(sql: sql,
                        param: new
                        {
                            @Owner = data.Owner,
                            @TaskDetails = data.TaskDetails,
                            @Rate = data.Rate,
                            @Active = data.Active
                        },
                        commandType: CommandType.StoredProcedure).ToList();

                    // Response
                    var response = new
                    {
                        ok = true,
                        description = "004",
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

        #endregion
    }
}