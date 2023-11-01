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
    [OpenApiTag("Home", Description = "營養師平台首頁")]
    public class NuHomeController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "首頁 - 取得最高評價營養師"

        /// <summary>
        /// 首頁 - 取得最高評價營養師
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("home/topNu")]
        public IHttpActionResult GetTopNu()
        {
            var random = new Random();
            var nutritionistsData = db.Nutritionists
             .Where(n => n.IsPublic && n.Plans.Any())
             .Take(20)
             .ToList();

            var topNutritionists = nutritionistsData
             .OrderByDescending(n => n.Plans.Average(p => p.Comments.Average(c => (double?)c.Rate) ?? 0))
             .ThenBy(n => random.NextDouble())
             .AsEnumerable()
             .Select(n => new
             {
                 NutritionistId = n.Id,
                 n.Title,
                 n.PortraitImage,
                 Expertis = n.Expertise.Split(',').ToArray()
             });
            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "取得評分最高營養師資料成功",
                Data = topNutritionists
            };
            return Ok(result);
        }
        #endregion  "首頁 - 取得最高評價營養師"
    }
}
