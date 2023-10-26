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
    public class PlanController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region  "新增API"
        /// <summary>
        /// 新增營養師課程方案
        /// </summary>
        /// <param name="planView">新增方案</param>
        /// <returns></returns>
        [HttpPost]
        [Route("nu/plan")]
        [JwtAuthFilter]
        public IHttpActionResult CreatePlan([FromBody] PlanView planView)
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

            if (!ModelState.IsValid)
            {
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


            var nu = db.Nutritionists.FirstOrDefault(n => n.UserId == id);
            var newPlan = new Plan
            {
                NutritionistId = nu.Id,
                Rank = planView.Rank,
                CourseName = planView.CourseName,
                CourseWeek = planView.CourseWeek,
                CoursePrice = planView.CoursePrice,
                Detail = planView.Detail,
                Tag = planView.Tag
            };
            db.Plans.Add(newPlan);
            db.SaveChanges();
            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "營養師課程方案成功",
                Date = new { newPlan }
            };
            return Ok(result);
        }
        #endregion "新增API"

        #region "取得課程方案"
        /// <summary>
        /// 取得營養師課程方案
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("nu/plan")]
        [JwtAuthFilter]
        public IHttpActionResult GelPlans()
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
            var nu = db.Nutritionists.FirstOrDefault(n => n.UserId == id);
            var nuPlan = db.Plans
            .Where(plan => plan.NutritionistId == nu.Id)
            .OrderByDescending(plan => plan.Rank)
            .ThenBy(plan => plan.CreateDate)
            .ToList();

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "營養師課程方案取得成功",
                Date = new { nuPlan }
            };
            return Ok(result);
        }

        #endregion "取得課程方案"
    }
}
