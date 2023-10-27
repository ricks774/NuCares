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
    public class StudentDailyEditController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "編輯早餐"

        /// <summary>
        /// 編輯早餐
        /// </summary>
        /// <param name="viewDailyMealTime"></param>
        /// <param name="courseId"></param>
        /// <param name="dailyLogId"></param>
        /// <param name="dailyCourseMealTimeId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("course/{courseId}/daily/{dailyLogId}/breakfast/{dailyCourseMealTimeId}")]
        [JwtAuthFilter]
        public IHttpActionResult BreakfastEdit(ViewDailyMealTime viewDailyMealTime, int courseId, int dailyLogId, int dailyCourseMealTimeId)
        {
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

            int breakfastId = db.DailyMealTimes
                .Where(dm => dm.DailyLogId == dailyLogId && dm.MealTime.Equals("早餐"))
                .Select(dm => dm.Id)
                .FirstOrDefault();

            return MealTimeEdit(viewDailyMealTime, dailyCourseMealTimeId, breakfastId);
        }

        #endregion "編輯早餐"

        #region "編輯午餐"

        /// <summary>
        /// 編輯午餐
        /// </summary>
        /// <param name="viewDailyMealTime"></param>
        /// <param name="courseId"></param>
        /// <param name="dailyLogId"></param>
        /// <param name="dailyCourseMealTimeId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("course/{courseId}/daily/{dailyLogId}/lunch/{dailyCourseMealTimeId}")]
        [JwtAuthFilter]
        public IHttpActionResult LunchEdit(ViewDailyMealTime viewDailyMealTime, int courseId, int dailyLogId, int dailyCourseMealTimeId)
        {
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

            int breakfastId = db.DailyMealTimes
                .Where(dm => dm.DailyLogId == dailyLogId && dm.MealTime.Equals("午餐"))
                .Select(dm => dm.Id)
                .FirstOrDefault();

            return MealTimeEdit(viewDailyMealTime, dailyCourseMealTimeId, breakfastId);
        }

        #endregion "編輯午餐"

        #region "編輯晚餐"

        /// <summary>
        /// 編輯晚餐
        /// </summary>
        /// <param name="viewDailyMealTime"></param>
        /// <param name="courseId"></param>
        /// <param name="dailyLogId"></param>
        /// <param name="dailyCourseMealTimeId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("course/{courseId}/daily/{dailyLogId}/dinner/{dailyCourseMealTimeId}")]
        [JwtAuthFilter]
        public IHttpActionResult DinnerEdit(ViewDailyMealTime viewDailyMealTime, int courseId, int dailyLogId, int dailyCourseMealTimeId)
        {
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

            int breakfastId = db.DailyMealTimes
                .Where(dm => dm.DailyLogId == dailyLogId && dm.MealTime.Equals("晚餐"))
                .Select(dm => dm.Id)
                .FirstOrDefault();

            return MealTimeEdit(viewDailyMealTime, dailyCourseMealTimeId, breakfastId);
        }

        #endregion "編輯晚餐"

        #region "編輯三餐資料"

        private IHttpActionResult MealTimeEdit(ViewDailyMealTime viewDailyMealTime, int dailyCourseMealTimeId, int timesId)
        {
            if (timesId != dailyCourseMealTimeId)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 401,
                    Status = "Error",
                    Message = "修改失敗，餐別不符合"
                });
            }

            var breakfastData = db.DailyMealTimes.SingleOrDefault(dt => dt.Id == timesId);

            // 判斷書入的格式是否正確
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 401,
                    Status = "Error",
                    Message = "修改失敗，數值輸入錯誤"
                });
            }

            // 將取得的資料更新到資料庫
            breakfastData.MealDescription = !string.IsNullOrEmpty(viewDailyMealTime.MealDescription) ? viewDailyMealTime.MealDescription : breakfastData.MealDescription;
            breakfastData.MealImgUrl = !string.IsNullOrEmpty(viewDailyMealTime.MealImgUrl) ? viewDailyMealTime.MealImgUrl : breakfastData.MealImgUrl;
            breakfastData.Starch = (int)(viewDailyMealTime.Starch.HasValue ? viewDailyMealTime.Starch : breakfastData.Starch);
            breakfastData.Protein = (int)(viewDailyMealTime.Protein.HasValue ? viewDailyMealTime.Protein : breakfastData.Protein);
            breakfastData.Vegetable = (int)(viewDailyMealTime.Vegetable.HasValue ? viewDailyMealTime.Vegetable : breakfastData.Vegetable);

            if (breakfastData.Starch >= 100 || breakfastData.Protein >= 100 || breakfastData.Vegetable >= 100)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 401,
                    Status = "Error",
                    Message = "修改失敗，數值過大"
                });
            }

            try
            {
                db.SaveChanges();

                // 要回傳的資料格式
                var breakfastLog = db.DailyMealTimes.Where(so => so.Id == timesId)
                    .Select(so => new
                    {
                        so.MealTime,
                        so.MealDescription,
                        so.MealImgUrl,
                        so.Starch,
                        so.Protein,
                        so.Vegetable
                    })
                    .SingleOrDefault();

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "修改早餐成功",
                    Data = breakfastLog
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        #endregion "編輯三餐資料"
    }
}