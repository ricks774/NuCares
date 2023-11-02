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
                    PortraitImage = ImageUrl.GetImageUrl(n.PortraitImage),
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

        #endregion "首頁 - 取得最高評價營養師"

        #region "首頁 - 取得所有營養師"

        /// <summary>
        /// 取得所有營養師資訊
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("nutritionists")]
        public IHttpActionResult GetAllNu(int page = 1, int userid = 0)
        {
            int pageSize = 10; // 每頁顯示的記錄數
            var totalRecords = db.Nutritionists.Where(n => n.IsPublic).Count(); // 計算符合條件的記錄總數
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize); // 計算總頁數

            if (userid == 0)
            {
                // 未登入時
                var nuData = db.Nutritionists
                .Where(n => n.IsPublic)
                .OrderBy(n => n.Id) // 主要排序條件
                .Skip(((int)page - 1) * pageSize) // 跳過前面的記錄
                .Take(pageSize) // 每頁顯示的記錄數
                .AsEnumerable() // 使查詢先執行,再在記憶體中處理
                .Select(n => new
                {
                    n.Title,
                    PortraitImage = ImageUrl.GetImageUrl(n.PortraitImage),
                    Expertise = n.Expertise.Split(',').ToArray(),
                    Favorite = false,
                    Course = n.Plans.Select(p => new
                    {
                        p.Rank,
                        p.CourseName,
                        p.CourseWeek,
                        p.CoursePrice,
                        p.Tag
                    }).OrderBy(p => p.Rank).Take(2)
                });

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "取得所有營養師",
                    Data = nuData
                };
                return Ok(result);
            }
            else
            {
                // 登入時
                var nuData = db.FavoriteLists
                    .Where(f => f.UserId == userid)
                    .Skip(((int)page - 1) * pageSize) // 跳過前面的記錄
                    .Take(pageSize) // 每頁顯示的記錄數
                    .AsEnumerable() // 使查詢先執行,再在記憶體中處理
                    .Select(f => new
                    {
                        f.Nutritionist.Title,
                        PortraitImage = ImageUrl.GetImageUrl(f.Nutritionist.PortraitImage),
                        Expertise = f.Nutritionist.Expertise.Split(',').ToArray(),
                        f.NutritionistId,
                        Course = f.Nutritionist.Plans.Select(p => new
                        {
                            p.Rank,
                            p.CourseName,
                            p.CourseWeek,
                            p.CoursePrice,
                            p.Tag
                        }).OrderBy(p => p.Rank).Take(2)
                    });

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "取得所有營養師",
                    Data = "YA",
                    userid
                };
                return Ok(result);
            }
        }

        #endregion "首頁 - 取得所有營養師"
    }
}