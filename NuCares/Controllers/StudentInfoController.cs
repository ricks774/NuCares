using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NSwag.Annotations;
using NuCares.Models;
using NuCares.Security;

namespace NuCares.Controllers
{
    [OpenApiTag("Stdent", Description = "學員")]
    public class StudentInfoController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "取得學員資訊"

        /// <summary>
        /// 取得使用者資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user/profile")]
        [JwtAuthFilter]
        public IHttpActionResult GetUserInfo()
        {
            #region "JwtToken驗證"

            // 取出請求內容，解密 JwtToken 取出資料
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int userId = (int)userToken["Id"];

            bool checkUser = db.Users.Any(n => n.Id == userId);
            if (!checkUser)
            {
                return Content(HttpStatusCode.Unauthorized, new
                {
                    StatusCode = 401,
                    Status = "Error",
                    Message = "請重新登入"
                });
            }

            #endregion "JwtToken驗證"

            var userData = db.Users.Where(u => u.Id == userId).Select(u => new
            {
                u.Id,
                u.UserName,
                u.ImgUrl,
                u.Email,
                u.Birthday,
                Gender = u.Gender.ToString(),
                u.Phone
            }).FirstOrDefault();

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "取得使用者資訊",
                Data = userData
            };
            return Ok(result);
        }

        #endregion "取得學員資訊"
    }
}