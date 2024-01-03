using NSwag.Annotations;
using NuCares.helper;
using NuCares.Models;
using NuCares.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NuCares.Controllers
{
    [OpenApiTag("Nutritionist", Description = "營養師")]
    public class NuCourseListController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "營養師 - 我的學員列表 API"

        /// <summary>
        /// 營養師 - 我的學員列表
        /// </summary>
        /// <param name="page">頁數</param>
        /// <returns></returns>
        [HttpGet]
        [Route("nu/courses")]
        [JwtAuthFilter]
        public IHttpActionResult GetNuCourseList(int page = 1)
        {
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)userToken["Id"];
            bool isNutritionist = (bool)userToken["IsNutritionist"];
            bool checkUser = db.Nutritionists.Any(n => n.UserId == id);
            if (!isNutritionist || !checkUser)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 403,
                    Status = "Error",
                    Message = new { Auth = "您沒有營養師權限" }
                });
            }
            int pageSize = 10; // 每頁顯示的記錄數
            var totalRecords = db.Courses.Where(c => c.Order.Plan.Nutritionist.UserId == id && c.Order.IsPayment).Count(); // 計算符合條件的記錄總數
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize); // 計算總頁數
            var today = DateTime.Today;
            var coursesData = db.Courses
                .Where(c => c.Order.Plan.Nutritionist.UserId == id && c.Order.IsPayment)
                .OrderBy(c => c.CourseState)
                .ThenByDescending(c => c.CreateDate)
                .Skip(((int)page - 1) * pageSize) // 跳過前面的記錄
                .Take(pageSize) // 每頁顯示的記錄數
                .AsEnumerable()
                .Select(c => new
                {
                    c.Id,
                    c.Order.OrderNumber,
                    c.Order.Plan.CourseWeek,
                    c.Order.Plan.CourseName,
                    c.Order.UserName,
                    CourseStartDate = c.CourseStartDate.HasValue ? c.CourseStartDate.Value.ToString("yyyy/MM/dd") : null,
                    CourseEndDate = c.CourseEndDate.HasValue ? c.CourseEndDate.Value.ToString("yyyy/MM/dd") : null,
                    CourseState = c.CourseEndDate < today ? "結束" : c.CourseState.ToString(),
                    c.IsQuest,
                    SortOrder = c.CourseState == 0 ? 0 : c.CourseEndDate >= today ? 1 : 2
                })
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.CourseStartDate);

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "取得我的學員列表成功",
                Data = coursesData,
                Pagination = new
                {
                    Current_page = page,
                    Total_pages = totalPages
                }
            };
            return Ok(result);
        }

        #endregion "營養師 - 我的學員列表 API"

        #region "取得課程期間"

        /// <summary>
        /// 取得課程期間
        /// </summary>
        /// <param name="courseId">課程 Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("course/{courseId}/time")]
        [JwtAuthFilter]
        public IHttpActionResult GetCourseTime(int courseId)
        {
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)userToken["Id"];
            bool isNutritionist = (bool)userToken["IsNutritionist"];
            bool checkUser = db.Nutritionists.Any(n => n.UserId == id);
            if (!isNutritionist || !checkUser)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 403,
                    Status = "Error",
                    Message = new { Auth = "您沒有營養師權限" }
                });
            }
            var coursesData = db.Courses
                .FirstOrDefault(c => c.Order.Plan.Nutritionist.UserId == id && c.Order.IsPayment && c.Id == courseId);
            if (coursesData == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { Course = "查無此課程" }
                });
            }
            var CourseStartDate = DateTime.Now.ToString("yyyy/MM/dd");
            var CourseEndDate = DateTime.Now.AddDays(coursesData.Order.Plan.CourseWeek * 7).ToString("yyyy/MM/dd");
            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "取得課程起訖日成功",
                Data = new
                {
                    CourseName = coursesData.Order.Plan.CourseName,
                    CourseStartDate,
                    CourseEndDate,
                }
            };
            return Ok(result);
        }

        #endregion "取得課程期間"

        #region "課程開始"

        /// <summary>
        /// 課程開始
        /// </summary>
        /// <param name="courseId">課程 Id</param>
        /// <param name="viewCourseTime">課程時間</param>
        /// <returns></returns>
        [HttpPut]
        [Route("course/{courseId}/start")]
        [JwtAuthFilter]
        public IHttpActionResult EditCourseTime(int courseId, [FromBody] ViewCourseTime viewCourseTime)
        {
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)userToken["Id"];
            bool isNutritionist = (bool)userToken["IsNutritionist"];
            bool checkUser = db.Nutritionists.Any(n => n.UserId == id);
            if (!isNutritionist || !checkUser)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 403,
                    Status = "Error",
                    Message = "您沒有營養師權限"
                });
            }
            var coursesData = db.Courses
                .FirstOrDefault(c => c.Order.Plan.Nutritionist.UserId == id && c.Order.IsPayment && c.Id == courseId);
            if (coursesData == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { Course = "查無此課程" }
                });
            }
            if (!coursesData.IsQuest)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { IsQuest = "課程尚未填寫問卷" }
                });
            }
            if (coursesData.CourseState == EnumList.CourseState.進行中)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { CourseState = "課程已經開始" }
                });
            }
            if (viewCourseTime == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { Course = "未輸入起訖日" }
                });
            }
            coursesData.CourseStartDate = viewCourseTime.CourseStartDate;
            coursesData.CourseEndDate = viewCourseTime.CourseEndDate;
            coursesData.CourseState = EnumList.CourseState.進行中;
            try
            {
                db.SaveChanges();
                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "課程起訖日更新成功",
                    Data = new
                    {
                        CourseName = coursesData.Order.Plan.CourseName,
                        CourseStartDate = coursesData.CourseStartDate,
                        CourseEndDate = coursesData.CourseEndDate,
                    }
                };

                //  通知訊息
                int channelId = coursesData.Order.UserId;  // 傳送通知給哪個學員
                int noticeId = Notice.AddNotice(db, channelId, "開始課程", courseId.ToString());   // 紀錄通知訊息

                // 取得 connectionId
                string userId = coursesData.Order.UserId.ToString();  // 傳送通知給哪個學員
                var connectionId = NotificationHub.Users.ConnectionIds.FirstOrDefault(u => u.Key == userId).Value;

                // Signal R通知
                if (connectionId != null)
                {
                    Notice.GetNotice(db, connectionId, noticeId, coursesData);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion "課程開始"
    }
}