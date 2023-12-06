using NuCares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NuCares.Security;
using NuCares.helper;

namespace NuCares.Controllers
{
    public class NoticeController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "完成問卷通知"

        [HttpGet]
        [Route("notice")]
        [JwtAuthFilter]
        public IHttpActionResult SurveyNotice()
        {
            #region "JwtToken驗證"

            // 取出請求內容，解密 JwtToken 取出資料
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)userToken["Id"];
            string userName = userToken["UserName"].ToString();

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

            #endregion "JwtToken驗證"

            var noticeData = db.Notification.FirstOrDefault(n => n.UserId == id);

            if (noticeData != null)
            {
                // 取得課程資料
                int courseId = int.Parse(noticeData.NoticeType);    // 取得課程Id
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

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "新增問卷成功",
                    Data = new
                    {
                        NoticeId = noticeData.Id,
                        CourseId = courseId,
                        CourseNane = couresData.Order.Plan.CourseName,
                        Message = noticeData.NoticeMessage,
                        Title = couresData.Order.Plan.Nutritionist.Title,
                        UserName = userName,
                        //Date = noticeData.CreateTime.ToString("yyyy/MM/dd HH:mm"),
                        IsRead = noticeData.IsRead
                    }
                };
            }

            return Content(HttpStatusCode.BadRequest, new
            {
                StatusCode = 400,
                Status = "Error",
                Message = "沒有通知",
                id,
                noticeData.UserId,
                noticeData
            });
        }

        #endregion "完成問卷通知"
    }
}