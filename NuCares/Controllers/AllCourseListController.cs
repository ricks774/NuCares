using NuCares.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NuCares.Models;
using NSwag.Annotations;

namespace NuCares.Controllers
{
    [OpenApiTag("Stdent", Description = "學員")]
    public class AllCourseListController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "學員-我的課程列表清單API"

        /// <summary>
        /// 課程列表清單
        /// </summary>
        /// <param name="page">學員的課程列表清單</param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/courses")]
        [JwtAuthFilter]
        public IHttpActionResult GetCoursesList(int page = 1)
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

            int pageSize = 10; // 每頁顯示的記錄數

            var totalRecords = db.Courses.Where(c => c.Order.UserId == id).Count(); // 計算符合條件的記錄總數
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize); // 計算總頁數

            var coursesData = db.Courses
                .Where(c => c.Order.UserId == id)
                .OrderBy(c => c.Id) // 根據需要的屬性進行排序
                .Skip(((int)page - 1) * pageSize) // 跳過前面的記錄
                .Take(pageSize) // 每頁顯示的記錄數
                .AsEnumerable() // 使查詢先執行,再在記憶體中處理
                .Select(c => new
                {
                    c.Id,
                    c.Order.OrderNumber,
                    c.Order.Plan.Nutritionist.Title,
                    c.Order.Plan.CourseName,
                    CourseStartDate = c.CourseStartDate.HasValue ? c.CourseStartDate.Value.ToString("yyyy-MM-dd") : null,
                    CourseEndDate = c.CourseEndDate.HasValue ? c.CourseStartDate.Value.ToString("yyyy-MM-dd") : null,
                    CourseState = c.CourseState.ToString(),
                    c.IsQuest,
                    c.IsComment,
                });

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "取得課程列表成功",
                Data = coursesData,
                Pagination = new
                {
                    Current_page = page,
                    Total_pages = totalPages
                }
            };
            return Ok(result);
        }

        #endregion "學員-我的課程列表清單API"
    }
}