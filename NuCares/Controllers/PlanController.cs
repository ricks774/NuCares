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
    public class PlanController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region  "新增課程方案API"
        /// <summary>
        /// 新增營養師課程方案
        /// </summary>
        /// <param name="viewPlan">新增方案</param>
        /// <returns></returns>
        [HttpPost]
        [Route("nu/plan")]
        [JwtAuthFilter]
        public IHttpActionResult CreatePlan([FromBody] ViewPlan viewPlan)
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
                Rank = viewPlan.Rank,
                CourseName = viewPlan.CourseName,
                CourseWeek = viewPlan.CourseWeek,
                CoursePrice = viewPlan.CoursePrice,
                Detail = viewPlan.Detail,
                Tag = viewPlan.Tag
            };
            db.Plans.Add(newPlan);
            db.SaveChanges();
            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "營養師課程方案成功",
                Data = newPlan
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

            var nuPlan = db.Plans
            .Where(plan => plan.Nutritionist.UserId == id && !plan.IsDelete)
            .OrderBy(plan => (int)plan.Rank)
            .ThenBy(plan => plan.CreateDate)
            .ToList();

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "營養師課程方案取得成功",
                Data = nuPlan
            };
            return Ok(result);
        }

        #endregion "取得課程方案"

        #region "修改課程方案"
        /// <summary>
        /// 更新營養師課程方案
        /// </summary>
        /// <param name="viewEditPlan">新增方案</param>
        /// <param name="planId">方案 Id</param>
        /// <returns></returns>
        [HttpPut]
        [Route("nu/plan/{planId}")]
        [JwtAuthFilter]
        public IHttpActionResult EditPlan([FromBody] ViewEditPlan viewEditPlan, int planId)
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
            var plan = db.Plans.FirstOrDefault(p => p.Id == planId && p.Nutritionist.UserId == id && !p.IsDelete);
            if (plan == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { Plan = "查無此課程方案" }
                });
            }

            //更新資料
            if (viewEditPlan.Rank.HasValue)
            {
                plan.Rank = viewEditPlan.Rank.Value;
            }
            if (viewEditPlan.CourseWeek != 0)
            {
                plan.CourseWeek = viewEditPlan.CourseWeek;
            }

            if (viewEditPlan.CoursePrice != 0)
            {
                plan.CoursePrice = viewEditPlan.CoursePrice;
            }

            var propertiesToCopy = new List<string>
            {
                "CourseName",
                "Detail",
                "Tag"
            };

            foreach (var property in propertiesToCopy)
            {
                var propertyValue = viewEditPlan.GetType().GetProperty(property).GetValue(viewEditPlan, null);
                if (propertyValue != null)
                {
                    plan.GetType().GetProperty(property).SetValue(plan, propertyValue);
                }
            }

            db.SaveChanges();
            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "營養師課程方案更新成功",
                Data = plan
            };

            return Ok(result);
        }

        #endregion "修改課程方案"


        #region "刪除課程方案"
        /// <summary>
        /// 刪除營養師課程方案
        /// </summary>
        /// <param name="planId">方案 Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("nu/plan/{planId}")]
        [JwtAuthFilter]
        public IHttpActionResult DelPlan(int planId)
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

            var plan = db.Plans.FirstOrDefault(p => p.Id == planId && p.Nutritionist.UserId == id && !p.IsDelete);
            if (plan == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { Plan = "查無此課程方案" }
                });
            }

            plan.IsDelete = true;
            db.SaveChanges();
            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "營養師課程方案刪除成功",
                Data = plan
            };

            return Ok(result);

        }

        #endregion"刪除課程方案"
    }
}
