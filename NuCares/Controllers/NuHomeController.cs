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
        /// <returns></returns>
        [HttpGet]
        [Route("nutritionists")]
        public IHttpActionResult GetAllNu(int page = 1)
        {
            int userId = 0;

            #region "JwtToken驗證"

            if (Request.Headers.Authorization != null && !string.IsNullOrEmpty(Request.Headers.Authorization.Parameter))
            {
                // 取出請求內容，解密 JwtToken 取出資料
                var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
                userId = (int)userToken["Id"];

                bool checkUser = db.Users.Any(n => n.Id == userId);
                if (!checkUser)
                {
                    return Content(HttpStatusCode.Unauthorized, new
                    {
                        StatusCode = 401,
                        Status = "Error",
                        Message = "請重新登入"
                    });
                }
            }
            else
            {
                // 如果沒有 JWT Token，進行相應的處理
                userId = 0;
            }

            #endregion "JwtToken驗證"

            int pageSize = 5; // 每頁顯示的記錄數
            var totalRecords = db.Nutritionists.Where(n => n.IsPublic).Count(); // 計算符合條件的記錄總數
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize); // 計算總頁數

            if (userId == 0)
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
                    n.Id,
                    n.Title,
                    PortraitImage = ImageUrl.GetImageUrl(n.PortraitImage),
                    Expertise = n.Expertise.Split(',').ToArray(),
                    Favorite = false,
                    n.AboutMe,
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
                    Data = nuData,
                    Pagination = new
                    {
                        Current_page = page,
                        Total_pages = totalPages
                    }
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
                        n.Id,
                        n.Title,
                        PortraitImage = ImageUrl.GetImageUrl(n.PortraitImage),
                        Expertise = n.Expertise.Split(',').ToArray(),
                        Favorite = n.FavoriteLists.Where(f => f.UserId == userId).Any(),
                        n.AboutMe,
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
                    Data = nuData,
                    Pagination = new
                    {
                        Current_page = page,
                        Total_pages = totalPages
                    }
                };
                return Ok(result);
            }
        }

        #endregion "首頁 - 取得所有營養師"

        #region "首頁 - 取得所有營養師Id"

        /// <summary>
        /// 取得所有營養師Id資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("nutritionistsid")]
        public IHttpActionResult GetNuId()
        {
            // 取的所有公開的營養師
            var nudata = db.Nutritionists.Where(n => n.IsPublic);

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "取得營養師資料Id成功",
                Data = nudata.Select(n => new
                {
                    n.Id
                })
            };
            return Ok(result);
        }

        #endregion "首頁 - 取得所有營養師Id"

        #region "首頁 - 取得單一營養師"

        /// <summary>
        /// 取得單一營養師
        /// </summary>
        /// <param name="nutritionistId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("nutritionist/{nutritionistId}")]
        public IHttpActionResult GetSingleNu(int nutritionistId, int userid = 0)
        {
            try
            {
                // 取得營養師資料
                var nuData = (
                from n in db.Nutritionists
                join u in db.Users on n.UserId equals u.Id
                where n.Id == nutritionistId
                select new { Nutritionist = n, User = u });   // select出2張表的所有欄位

                // 取得是否為收藏的營養師
                bool isFavorite = db.FavoriteLists.Where(f => f.UserId == userid && f.NutritionistId == nutritionistId).Any();

                // 取得評價資料
                var commentsData = (
                    from p in db.Plans
                    join o in db.Orders on p.Id equals o.PlanId
                    join c in db.Courses on o.Id equals c.OrderId
                    join cm in db.Comments on c.Id equals cm.CourseId
                    join u in db.Users on o.UserId equals u.Id
                    where p.NutritionistId == nutritionistId
                    select new { User = u, Comment = cm })   // select出2張表的所有欄位
                    .OrderByDescending(cm => cm.Comment.CreateDate); // 根據CreateDate升序排序

                // 計算評價的總平均
                var rateAvg = Math.Round(commentsData.Select(r => r.Comment.Rate).DefaultIfEmpty().Average(), 1);

                // 要輸出的資料
                var newNuData = nuData
                    .AsEnumerable()
                    .Select(nd => new
                    {
                        nd.Nutritionist.Title,
                        PortraitImage = ImageUrl.GetImageUrl(nd.Nutritionist.PortraitImage),
                        Expertise = nd.Nutritionist.Expertise.Split(',').ToArray(),
                        Favorite = isFavorite,
                        Gender = nd.User.Gender == 0 ? "男" : "女",
                        nd.Nutritionist.City,
                        nd.Nutritionist.Education,
                        nd.Nutritionist.Experience,
                        nd.Nutritionist.AboutMe,
                        nd.Nutritionist.CourseIntro,
                        Plan = nd.Nutritionist.Plans.Where(p => !p.IsDelete).Select(p => new
                        {
                            p.Id,
                            p.Rank,
                            p.CourseName,
                            p.CourseWeek,
                            p.CoursePrice,
                            p.Tag,
                            p.Detail
                        }).OrderBy(p => p.Rank),
                        Comment = commentsData.AsEnumerable().Select(c => new
                        {
                            c.User.UserName,
                            c.Comment.Content,
                            c.Comment.Rate,
                            CreateDate = c.Comment.CreateDate.ToString("yyyy/MM/dd")
                        }),
                        RateAVG = rateAvg
                    }).FirstOrDefault();

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "取得所有營養師",
                    Data = newNuData
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion "首頁 - 取得單一營養師"
    }
}