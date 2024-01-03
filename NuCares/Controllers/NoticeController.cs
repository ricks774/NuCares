using NuCares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NuCares.Security;
using NuCares.helper;
using NSwag.Annotations;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace NuCares.Controllers
{
    [OpenApiTag("Notifications", Description = "通知")]
    public class NoticeController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "讀取單一通知"

        /// <summary>
        /// 讀取單一通知
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("notice/{noticeId}")]
        [JwtAuthFilter]
        public IHttpActionResult GetNotice(int noticeId)
        {
            #region "JwtToken驗證"

            // 取出請求內容，解密 JwtToken 取出資料
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int userId = (int)userToken["Id"];
            string userName = userToken["UserName"].ToString();

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

            #endregion "JwtToken驗證"

            var noticeData = db.Notification.FirstOrDefault(n => n.UserId == userId && n.Id == noticeId);

            if (noticeData != null)
            {
                noticeData.IsRead = true;

                try
                {
                    db.SaveChanges();
                    var result = new
                    {
                        StatusCode = 200,
                        Status = "Success",
                        Message = "通知已讀成功",
                        ChannelId = userId,
                    };
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }

            return Content(HttpStatusCode.BadRequest, new
            {
                StatusCode = 400,
                Status = "Error",
                Message = "沒有新通知"
            });
        }

        #endregion "讀取單一通知"

        #region "取得全部通知"

        /// <summary>
        /// 取得所有通知訊息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("notice/all")]
        [JwtAuthFilter]
        public IHttpActionResult GetAllNotice()
        {
            #region "JwtToken驗證"

            // 取出請求內容，解密 JwtToken 取出資料
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int userId = (int)userToken["Id"];
            string userName = userToken["UserName"].ToString();

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

            #endregion "JwtToken驗證"

            var noticeData = db.Notification.Where(n => n.UserId == userId).ToList();

            // 創建一個 List 來存儲通知
            var noticeList = new List<object>();

            if (noticeData.Any())
            {
                foreach (var notice in noticeData)
                {
                    if (notice.NoticeMessage.Contains("已完成生活問卷"))
                    {
                        // 取得課程資料
                        int courseId = int.Parse(notice.NoticeType);    // 取得課程Id
                        var couresData = db.Courses.FirstOrDefault(c => c.Id == courseId);

                        #region "判斷問卷&課程是否存在"

                        if (couresData == null)
                        {
                            return Content(HttpStatusCode.BadRequest, new
                            {
                                StatusCode = 400,
                                Status = "Error",
                                Message = "找不到這筆課程"
                            });
                        }

                        #endregion "判斷問卷&課程是否存在"

                        #region "回傳問卷格式"

                        var surveysNotice = new
                        {
                            NoticeId = notice.Id,
                            CourseId = courseId,
                            CourseName = couresData.Order.Plan.CourseName,
                            Message = notice.NoticeMessage,
                            Title = couresData.Order.Plan.Nutritionist.Title,
                            UserName = couresData.Order.User.UserName,
                            Date = notice.CreateTime.ToString("yyyy/MM/dd HH:mm"),
                            IsRead = notice.IsRead
                        };

                        #endregion "回傳問卷格式"

                        // 將通知加入 List
                        noticeList.Add(surveysNotice);
                    }
                    else if (notice.NoticeMessage.Contains("已購課"))
                    {
                        // 取得訂單資料
                        int orderId = int.Parse(notice.NoticeType);    // 取得訂單Id
                        var orderData = db.Orders.FirstOrDefault(o => o.Id == orderId);

                        #region "判斷訂單是否存在"

                        if (orderData == null)
                        {
                            return Content(HttpStatusCode.BadRequest, new
                            {
                                StatusCode = 400,
                                Status = "Error",
                                Message = "找不到這筆訂單"
                            });
                        }

                        #endregion "判斷訂單是否存在"

                        #region "回傳購課格式"

                        var orderNotice = new
                        {
                            NoticeId = notice.Id,
                            CourseName = orderData.Plan.CourseName,
                            Message = notice.NoticeMessage,
                            Title = orderData.Plan.Nutritionist.Title,
                            UserName = orderData.User.UserName,
                            Date = notice.CreateTime.ToString("yyyy/MM/dd HH:mm"),
                            IsRead = notice.IsRead
                        };

                        #endregion "回傳購課格式"

                        // 將通知加入 List
                        noticeList.Add(orderNotice);
                    }
                    else if (notice.NoticeMessage.Contains("已評價"))
                    {
                        // 取得課程資料
                        int courseId = int.Parse(notice.NoticeType);    // 取得課程Id
                        var couresData = db.Courses.FirstOrDefault(c => c.Id == courseId);

                        #region "判斷課程是否存在"

                        if (couresData == null)
                        {
                            return Content(HttpStatusCode.BadRequest, new
                            {
                                StatusCode = 400,
                                Status = "Error",
                                Message = "找不到這筆課程"
                            });
                        }

                        #endregion "判斷課程是否存在"

                        #region "回傳評價格式"

                        var commentsNotice = new
                        {
                            NoticeId = notice.Id,
                            NutritionistId = couresData.Order.Plan.NutritionistId,
                            CourseName = couresData.Order.Plan.CourseName,
                            Message = notice.NoticeMessage,
                            Title = couresData.Order.Plan.Nutritionist.Title,
                            UserName = couresData.Order.User.UserName,
                            Date = notice.CreateTime.ToString("yyyy/MM/dd HH:mm"),
                            IsRead = notice.IsRead
                        };

                        #endregion "回傳評價格式"

                        // 將通知加入 List
                        noticeList.Add(commentsNotice);
                    }
                    else if (notice.NoticeMessage.Contains("開始課程"))
                    {
                        // 取得課程資料
                        int courseId = int.Parse(notice.NoticeType);    // 取得課程Id
                        var couresData = db.Courses.FirstOrDefault(c => c.Id == courseId);

                        #region "判斷課程是否存在"

                        if (couresData == null)
                        {
                            return Content(HttpStatusCode.BadRequest, new
                            {
                                StatusCode = 400,
                                Status = "Error",
                                Message = "找不到這筆課程"
                            });
                        }

                        #endregion "判斷課程是否存在"

                        #region "回傳開始課程格式"

                        var startNotice = new
                        {
                            NoticeId = notice.Id,
                            CourseId = courseId,
                            CourseName = couresData.Order.Plan.CourseName,
                            Message = notice.NoticeMessage,
                            Title = couresData.Order.Plan.Nutritionist.Title,
                            UserName = couresData.Order.User.UserName,
                            Date = notice.CreateTime.ToString("yyyy/MM/dd HH:mm"),
                            IsRead = notice.IsRead
                        };

                        #endregion "回傳開始課程格式"

                        // 將通知加入 List
                        noticeList.Add(startNotice);
                    }
                }

                // 將通知列表按照日期降序排序
                noticeList = noticeList.OrderByDescending(n => ((dynamic)n).Date).ToList();

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "取得通知成功",
                    ChannelId = userId,
                    Data = noticeList
                };
                return Ok(result);
            }

            return Content(HttpStatusCode.BadRequest, new
            {
                StatusCode = 400,
                Status = "Error",
                Message = "沒有新通知"
            });
        }

        #endregion "取得全部通知"

        #region "取得全域未讀通知"

        /// <summary>
        /// 取得所有未讀通知
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("notice/new")]
        [JwtAuthFilter]
        public IHttpActionResult GetNotReadNotice()
        {
            #region "JwtToken驗證"

            // 取出請求內容，解密 JwtToken 取出資料
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int userId = (int)userToken["Id"];
            string userName = userToken["UserName"].ToString();

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

            #endregion "JwtToken驗證"

            var noticeData = db.Notification.Where(n => n.UserId == userId).ToList();
            bool isAllRead = noticeData.All(n => n.GlobalIsRead);

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "取得未讀通知成功",
                IsAllRead = isAllRead,
                ChannelId = userId,
            };
            return Ok(result);
        }

        #endregion "取得全域未讀通知"

        #region "全域通知已讀"

        /// <summary>
        /// 已讀全域通知
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("notice/new")]
        [JwtAuthFilter]
        public IHttpActionResult ReadAllGlobalNotice()
        {
            #region "JwtToken驗證"

            // 取出請求內容，解密 JwtToken 取出資料
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int userId = (int)userToken["Id"];
            string userName = userToken["UserName"].ToString();

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

            #endregion "JwtToken驗證"

            var noticeData = db.Notification.Where(n => n.UserId == userId).ToList();

            if (noticeData != null)
            {
                for (int i = 0; i < noticeData.Count; i++)
                {
                    noticeData[i].GlobalIsRead = true;
                }

                try
                {
                    db.SaveChanges();
                    var result = new
                    {
                        StatusCode = 200,
                        Status = "Success",
                        Message = "全域通知已讀成功",
                        ChannelId = userId,
                    };
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }

            return Content(HttpStatusCode.BadRequest, new
            {
                StatusCode = 400,
                Status = "Error",
                Message = "沒有新通知"
            });
        }

        #endregion "全域通知已讀"

        #region "所有通知已讀"

        /// <summary>
        /// 所有通知已讀
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("notice/readall")]
        [JwtAuthFilter]
        public IHttpActionResult ReadAllNotice()
        {
            #region "JwtToken驗證"

            // 取出請求內容，解密 JwtToken 取出資料
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int userId = (int)userToken["Id"];
            string userName = userToken["UserName"].ToString();

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

            #endregion "JwtToken驗證"

            var noticeData = db.Notification.Where(n => n.UserId == userId).ToList();

            if (noticeData != null)
            {
                for (int i = 0; i < noticeData.Count; i++)
                {
                    noticeData[i].IsRead = true;
                }

                try
                {
                    db.SaveChanges();
                    var result = new
                    {
                        StatusCode = 200,
                        Status = "Success",
                        Message = "所有通知已讀成功",
                        ChannelId = userId,
                    };
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }

            return Content(HttpStatusCode.BadRequest, new
            {
                StatusCode = 400,
                Status = "Error",
                Message = "沒有新通知"
            });
        }

        #endregion "所有通知已讀"
    }
}