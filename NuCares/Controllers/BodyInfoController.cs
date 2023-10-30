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
    public class BodyInfoController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "新增身體數值"

        [HttpPost]
        [Route("course/{courseId}/inbody")]
        [JwtAuthFilter]
        public IHttpActionResult BodyInfoEdit(ViewBodyInfo viewBodyInfo, int courseId)
        {
            #region "JwtToken驗證"

            // 取出請求內容，解密 JwtToken 取出資料
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)userToken["Id"];

            bool checkUser = db.Users.Any(n => n.Id == id);
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

            if (!ModelState.IsValid)
            {
                // 回傳錯誤的訊息
                var errors = ModelState.Keys
                    .Select(key =>
                    {
                        var propertyName = key.Split('.').Last(); // 取的屬性名稱
                        var errorMessage = ModelState[key].Errors.First().ErrorMessage; // 取得錯誤訊息
                        return new { PropertyName = propertyName, ErrorMessage = errorMessage };
                    })
                    .ToDictionary(e => e.PropertyName, e => e.ErrorMessage);

                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = errors
                });
            }
            else
            {
                var courseCheck = db.Courses.SingleOrDefault(c => c.Id == courseId);
                //bool course

                // 創建一個新的 BodyInfo 物件，紀錄傳入的數值
                var newUser = new BodyInfo
                {
                    CourseId = courseId,
                    Height = viewBodyInfo.Height,
                    Weight = viewBodyInfo.Weight,
                    BodyFat = viewBodyInfo.BodyFat,
                    VisceralFat = viewBodyInfo.VisceralFat,
                    SMM = viewBodyInfo.SMM,
                    Bmi = viewBodyInfo.Bmi,
                    Bmr = viewBodyInfo.Bmr,
                    InsertDate = DateTime.Now
                };
            }

            return Ok();
        }

        #endregion "新增身體數值"
    }
}