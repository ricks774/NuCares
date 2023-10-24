using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using NSwag.Annotations;
using NuCares.Models;
using NuCares.Security;

namespace NuCares.Controllers
{
    [OpenApiTag("SingUp", Description = "註冊")]
    public class SingUpController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();
        private readonly Argon2Verify ag2Verify = new Argon2Verify();

        #region "註冊API"

        /// <summary>
        /// 新會員註冊
        /// </summary>
        /// <param name="viewUser">註冊新會員</param>
        /// <returns></returns>
        [HttpPost]
        [Route("auth/signup")]
        public IHttpActionResult SignUp(ViewUser viewUser)
        {
            if (ModelState.IsValid)
            {
                // Hash 加鹽加密
                var salt = ag2Verify.CreateSalt();
                string saltStr = Convert.ToBase64String(salt);
                var hash = ag2Verify.HashPassword(viewUser.Password, salt);
                string hashPassword = Convert.ToBase64String(hash);

                // 判斷Email是否有重複
                bool emailCheck = db.Users.Any(u => u.Email == viewUser.Email);
                if (emailCheck)
                {
                    return BadRequest("Email重複!");
                }

                // 創建一個新的 User 物件，紀錄傳入的數值
                var newUser = new User
                {
                    UserName = viewUser.UserName,
                    Password = hashPassword,
                    Salt = saltStr,
                    Email = viewUser.Email,
                    Birthday = viewUser.Birthday,
                    Gender = viewUser.Gender,
                    Phone = viewUser.Phone
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "註冊成功"
                };
                return Ok(result);
            }
            else
            {
                // TODO 目前回傳200，有空研究是否可以回傳400
                // 回傳錯誤的訊息
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Ok(new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "註冊失敗",
                    Errors = errors
                });
            }
        }

        #endregion "註冊API"

        #region "判斷信箱是否重複"

        /// <summary>
        /// 信箱重複驗證
        /// </summary>
        /// <param name="viewUserCheck">判斷信箱是否重複</param>
        /// <returns></returns>
        [HttpPost]
        [Route("auth/checkEmail")]
        public IHttpActionResult CheckEmail(ViewUserCheck viewUserCheck)
        {
            // 判斷Email是否有重複
            bool emailCheck = db.Users.Any(u => u.Email == viewUserCheck.Email);
            if (emailCheck)
            {
                return BadRequest("Email重複!");
            }
            return Ok(new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "Email可以使用"
            });
        }

        #endregion "判斷信箱是否重複"
    }
}