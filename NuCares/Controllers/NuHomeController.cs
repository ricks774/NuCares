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
                    Course = n.Plans.Where(p => !p.IsDelete).Select(p => new
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
                        Favorite = n.FavoriteLists.Where(f => f.UserId == userid).Any(),
                        Course = n.Plans.Where(p => !p.IsDelete).Select(p => new
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
        }

        #endregion "首頁 - 取得所有營養師"

        #region "首頁 - 取得單一營養師"

        [HttpGet]
        [Route("nutritionist")]
        public IHttpActionResult GetNu(int nutritionistid, int userid = 0)
        {
            var nuData = (
                from n in db.Nutritionists
                join u in db.Users on n.UserId equals u.Id
                //join o in db.Orders on u.Id equals o.UserId
                //join cs in db.Courses on o.Id equals cs.OrderId
                where n.Id == nutritionistid
                select new { Nutritionist = n, User = u });   // select出2張表的所有欄位

            var commentsData = (
                from p in db.Plans
                join o in db.Orders on p.Id equals o.PlanId
                join c in db.Courses on o.Id equals c.OrderId
                join cm in db.Comments on c.Id equals cm.CourseId
                join u in db.Users on o.UserId equals u.Id
                where p.NutritionistId == nutritionistid
                select new { User = u, Comment = cm });   // select出2張表的所有欄位

            // 未登入時
            if (userid == 0)
            {
                //// 取得營養師已被購買的 課程id
                //var coursesData = db.Plans.Where(p => p.NutritionistId == nutritionistid)
                //.SelectMany(p => p.Orders)
                //.SelectMany(o => o.Courses)
                //.Select(c => c.Id);

                //// 透過 課程id 取得該營養師課程的評價
                //var commentsData = db.Comments.Where(c => coursesData.Contains(c.CourseId)).AsEnumerable();

                //// 取得 Orders id
                //var orderIds = db.Courses.Where(c => coursesData.Contains(c.Id)).Select(c => c.OrderId);
                //// 取得 Users id
                //var userIds = db.Orders.Where(o => orderIds.Contains(o.Id)).Select(o => o.UserId);
                //// 取得 Users UserName
                //var userNameData = db.Users.Where(u => userIds.Contains(u.Id)).Select(u => u.UserName);

                var newNuData = nuData
                    .AsEnumerable()
                    .Select(nd => new
                    {
                        nd.Nutritionist.Title,
                        PortraitImage = ImageUrl.GetImageUrl(nd.Nutritionist.PortraitImage),
                        Expertise = nd.Nutritionist.Expertise.Split(',').ToArray(),
                        Favorite = nd.User.FavoriteLists.Where(f => f.UserId == userid).Any(),
                        Gender = nd.User.Gender == 0 ? "男" : "女",
                        nd.Nutritionist.City,
                        nd.Nutritionist.Education,
                        nd.Nutritionist.Experience,
                        nd.Nutritionist.AboutMe,
                        nd.Nutritionist.CourseIntro,
                        Course = nd.Nutritionist.Plans.Where(p => !p.IsDelete).Select(p => new
                        {
                            p.Rank,
                            p.CourseName,
                            p.CourseWeek,
                            p.CoursePrice,
                            p.Tag
                        }).OrderBy(p => p.Rank),
                        Comment = commentsData.AsEnumerable().Select(c => new
                        {
                            c.User.UserName,
                            c.Comment.Content,
                            c.Comment.Rate,
                            CreateDate = c.Comment.CreateDate.ToString("yyyy/MM/dd")
                        })
                    });

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "取得所有營養師",
                    Data = newNuData
                };
                return Ok(result);
            }
            else
            {
                // 登入時
                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "取得所有營養師",
                    Data = ""
                };
                return Ok("BAD");
            }
        }

        #endregion "首頁 - 取得單一營養師"
    }
}