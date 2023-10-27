using NSwag.Annotations;
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
            int pageSize = 5; // 每頁顯示的記錄數
            var totalRecords = db.Courses.Where(c => c.Order.Plan.Nutritionist.UserId == id && c.Order.IsPayment).Count(); // 計算符合條件的記錄總數
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize); // 計算總頁數

            var coursesData = db.Courses
                .Where(c => c.Order.Plan.Nutritionist.UserId == id && c.Order.IsPayment)
                .OrderBy(c => c.Order.CreateDate) // 根據需要的屬性進行排序
                .Skip(((int)page - 1) * pageSize) // 跳過前面的記錄
                .Take(pageSize) // 每頁顯示的記錄數
                .Select(c => new
                {
                    c.Id,
                    c.Order.OrderNumber,
                    c.Order.Plan.Nutritionist.Title,
                    c.Order.Plan.CourseName,
                    c.CourseStartDate,
                    c.CourseEndDate,
                    c.CourseState,
                    c.IsQuest,
                });

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

    }
}
