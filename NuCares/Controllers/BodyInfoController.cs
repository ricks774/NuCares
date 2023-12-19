using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NSwag.Annotations;
using NuCares.Models;
using NuCares.Security;
using NuCares.helper;

namespace NuCares.Controllers
{
    [OpenApiTag("Student", Description = "學員")]
    public class BodyInfoController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "新增身體數值"

        /// <summary>
        /// 新增當天身體數值
        /// </summary>
        /// <param name="viewBodyInfo"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
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

            #region "可能有多筆錯誤發生時的錯誤訊息"

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

            #endregion "可能有多筆錯誤發生時的錯誤訊息"

            #region "判斷課程是否已付款&狀態&權限"

            var coursesData = db.Courses.FirstOrDefault(c => c.Id == courseId && c.Order.IsPayment);

            if (coursesData == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "查無此課程"
                });
            }

            if (!(coursesData.Order.Plan.Nutritionist.UserId == id || coursesData.Order.UserId == id))
            {
                return Content(HttpStatusCode.Unauthorized, new
                {
                    StatusCode = 403,
                    Status = "Error",
                    Message = new { Auth = "您沒有權限" }
                });
            }

            if (coursesData.CourseState == 0 || coursesData.CourseStartDate == null || coursesData.CourseEndDate == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { Course = "課程尚未開始" }
                });
            }

            #endregion "判斷課程是否已付款&狀態&權限"

            // 判斷當天是否已以新增過紀錄
            bool bodyData = db.BodyInfos.Any(bi => bi.InsertDate == DateTime.Today && bi.CourseId == courseId);
            if (bodyData)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "新增失敗，今天已經有紀錄"
                });
            }

            #region "計算BMI & BMR公式"

            // 計算BMI
            double bmiHeight = (double)(viewBodyInfo.Height / 100);
            double bmiWeight = (double)viewBodyInfo.Weight;
            double bmi = bmiWeight / (bmiHeight * bmiHeight);

            // 計算BMR
            // 取得年齡
            var userdate = db.Users.SingleOrDefault(u => u.Id == id);
            DateTime today = DateTime.Today;
            int age = today.Year - userdate.Birthday.Year;
            if (today < userdate.Birthday.AddYears(age))
            {
                age--;
            }

            // BMR公式
            double bmrHeight = (double)viewBodyInfo.Height;
            int bmr = 0;
            if (userdate.Gender == 0)
            {
                bmr = (int)(66 + (13.7 * bmiWeight + 5 * bmrHeight - 6.8 * age));
            }
            else
            {
                bmr = (int)(655 + (9.6 * bmiWeight + 1.8 * bmrHeight - 4.7 * age));
            }

            #endregion "計算BMI & BMR公式"

            // 創建一個新的 BodyInfo 物件，紀錄傳入的數值
            var newUser = new BodyInfo
            {
                CourseId = courseId,
                Height = viewBodyInfo.Height,
                Weight = viewBodyInfo.Weight,
                BodyFat = viewBodyInfo.BodyFat,
                VisceralFat = viewBodyInfo.VisceralFat,
                SMM = viewBodyInfo.SMM,
                Bmi = (decimal?)bmi,
                Bmr = bmr,
                InsertDate = DateTime.Today
            };

            db.BodyInfos.Add(newUser);

            try
            {
                db.SaveChanges();

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "新增身體數值成功",
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion "新增身體數值"
    }
}