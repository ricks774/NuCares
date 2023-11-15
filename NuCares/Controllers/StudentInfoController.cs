using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NSwag.Annotations;
using NuCares.Models;
using NuCares.Security;
using static NuCares.Models.EnumList;
using NuCares.helper;

namespace NuCares.Controllers
{
    [OpenApiTag("Stdent", Description = "學員")]
    public class StudentInfoController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();
        private readonly Argon2Verify ag2Verify = new Argon2Verify();

        #region "取得使用者資訊"

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

            var userData = db.Users.Where(u => u.Id == userId).AsEnumerable().Select(u => new
            {
                u.Id,
                u.UserName,
                ImgUrl = ImageUrl.GetImageUrl(u.ImgUrl),
                u.Email,
                Birthday = u.Birthday.ToString("yyyy/MM/dd"),
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

        #endregion "取得使用者資訊"

        #region "修改使用者資訊"

        /// <summary>
        /// 修改使用者資訊
        /// </summary>
        /// <param name="viewUserInfoEdit"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("user/profile")]
        [JwtAuthFilter]
        public IHttpActionResult UserInfoEdit(ViewUserInfoEdit viewUserInfoEdit)
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

            var userData = db.Users.FirstOrDefault(u => u.Id == userId);

            // 判斷書入的格式是否正確
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "修改失敗，數值輸入錯誤"
                });
            }

            // 將取得的資料更新到資料庫
            userData.UserName = !string.IsNullOrEmpty(viewUserInfoEdit.UserName) ? viewUserInfoEdit.UserName : userData.UserName;
            userData.ImgUrl = !string.IsNullOrEmpty(viewUserInfoEdit.ImgUrl) ? viewUserInfoEdit.ImgUrl : userData.ImgUrl;
            userData.Birthday = viewUserInfoEdit.Birthday != null ? viewUserInfoEdit.Birthday : userData.Birthday;
            userData.Gender = !string.IsNullOrEmpty(viewUserInfoEdit.Gender) ? (GenderType)Enum.Parse(typeof(GenderType), viewUserInfoEdit.Gender) : userData.Gender;
            userData.Phone = !string.IsNullOrEmpty(viewUserInfoEdit.Phone) ? viewUserInfoEdit.Phone : userData.Phone;

            try
            {
                db.SaveChanges();

                var newUserData = db.Users.Where(u => u.Id == userId).AsEnumerable().Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.ImgUrl,
                    u.Email,
                    Birthday = u.Birthday.ToString("yyyy/MM/dd"),
                    Gender = u.Gender.ToString(),
                    u.Phone
                }).FirstOrDefault();

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "修改使用者資訊成功",
                    Data = newUserData
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion "修改使用者資訊"

        #region "變更密碼"

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="viewUserPassChange"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("user/update-password")]
        [JwtAuthFilter]
        public IHttpActionResult UserPassChange(ViewUserPassChange viewUserPassChange)
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

            var userData = db.Users.FirstOrDefault(u => u.Id == userId);

            // 取得輸入的舊密碼
            string oldPassword = viewUserPassChange.OldPassword;

            // 取得資料庫中的資料
            string salt = userData.Salt;
            string hashPassword = userData.Password;

            // 判斷書入的格式是否正確
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "修改失敗，數值輸入錯誤"
                });
            }

            // 判斷密碼是否正確
            var passwordCheck = ag2Verify.VerifyPassword(oldPassword, salt, hashPassword);

            if (passwordCheck)
            {
                if (viewUserPassChange.OldPassword == viewUserPassChange.Password)
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        StatusCode = 400,
                        Status = "Error",
                        Message = "新密碼不可與舊密碼相同"
                    });
                }
                else
                {
                    if (viewUserPassChange.Password == viewUserPassChange.RePassword)
                    {
                        // 密碼hash加密
                        var getHash = ag2Verify.PasswordHash(viewUserPassChange.Password);
                        userData.Password = getHash.hashPassword;
                        userData.Salt = getHash.salt;
                    }
                    else
                    {
                        return Content(HttpStatusCode.BadRequest, new
                        {
                            StatusCode = 400,
                            Status = "Error",
                            Message = "密碼不一致"
                        });
                    }
                }
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "密碼錯誤"
                });
            }

            try
            {
                db.SaveChanges();

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "修改密碼成功"
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion "變更密碼"
    }
}