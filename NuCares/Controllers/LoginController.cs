using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NuCares.Models;
using NuCares.Security;

namespace NuCares.Controllers
{
    [OpenApiTag("Login", Description = "登入")]
    public class LoginController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();
        private readonly Argon2Verify ag2Verify = new Argon2Verify();

        #region "登入API"

        /// <summary>
        /// 使用者登入驗證
        /// </summary>
        /// <param name="viewUserLogin">使用者登入</param>
        /// <returns></returns>
        [HttpGet]
        [Route("auth/login")]
        public IHttpActionResult Login(ViewUserLogin viewUserLogin)
        {
            string account = viewUserLogin.Email;
            string password = viewUserLogin.Password;

            // 篩選特定資料
            var userData = db.Users
                .Where(u => u.Email == viewUserLogin.Email)
                // 如果需要指定欄位
                //.Select(u => new
                //{
                //    u.Id,
                //    u.UserName,
                //    u.Email,
                //    u.ImgUrl,
                //    u.IsNutritionist
                //})
                .FirstOrDefault();

            // 取得資料庫中的資料
            string salt = userData.Salt;
            string cps = userData.Password;

            // 加鹽解密判斷
            bool passwordCheck = ag2Verify.Argon2_Login(salt, password, cps);

            if (userData != null)
            {
                if (passwordCheck)
                {
                    // 生成新 JwtToken
                    JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
                    string jwtToken = jwtAuthUtil.GenerateToken(userData.Id);

                    // 判斷目前使用者身分
                    string userStatus = userData.IsNutritionist ? "nutritionist" : "student";

                    var result = new
                    {
                        StatusCode = 200,
                        Status = "Success",
                        Message = "登入成功",
                        Token = jwtToken,
                        Data = new
                        {
                            userData.Id,
                            userData.UserName,
                            userData.Email,
                            userData.ImgUrl,
                            userData.IsNutritionist,
                            UserCurrentStatus = userStatus
                        }
                    };
                    return Ok(result);
                }
                else
                {
                    return Content(
                        HttpStatusCode.BadRequest, new
                        {
                            StatusCode = 400,
                            Status = "Error",
                            Message = "信箱不存在"
                        });
                }
            }
            else
            {
                return Content(
                    HttpStatusCode.BadRequest, new
                    {
                        StatusCode = 400,
                        Status = "Error",
                        Message = "密碼錯誤"
                    });
            }
        }

        #endregion "登入API"
    }
}