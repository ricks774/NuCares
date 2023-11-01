using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NuCares.Models;
using NuCares.Security;

namespace NuCares.Controllers
{
    public class SurveyController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "新增問卷API"

        [HttpPost]
        [Route("course/{courseId}/survey")]
        [JwtAuthFilter]
        public IHttpActionResult AddSurvey(ViewAddSurvey viewAddSurvey, int courseId)
        {
            #region "JwtToken驗證"

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

            #endregion "JwtToken驗證"

            #region "查看課程是否存在"

            var courseCheck = db.Courses.Find(courseId);
            if (courseCheck == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "找不到這筆課程"
                });
            }

            #endregion "查看課程是否存在"

            bool surveyData = db.Surveys.Any(s => s.CourseId == courseId);
            if (surveyData)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "新增失敗，問卷已經填寫"
                });
            }

            if (viewAddSurvey != null)
            {
                // 創建一個新的 Survey 物件，紀錄傳入的數值
                var newSurvey = new Survey
                {
                    CourseId = courseId
                };
                // 用迴圈將所有題目寫入
                for (int i = 1; i <= 25; i++)
                {
                    string question = $"Question{i}";
                    string getVaule = (string)viewAddSurvey.GetType().GetProperty(question).GetValue(viewAddSurvey);
                    newSurvey.GetType().GetProperty(question).SetValue(newSurvey, getVaule);
                }
                newSurvey.CreateTime = DateTime.Today;

                db.Surveys.Add(newSurvey);
                db.SaveChanges();

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "新增問卷成功",
                };
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "請填寫正確的資料"
                });
            }
        }

        #endregion "新增問卷API"

        #region "取得課程問卷"

        /// <summary>
        /// 取得課程問卷
        /// </summary>
        /// <param name="courseId">課程 Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("course/{courseId}/survey")]
        [JwtAuthFilter]
        public IHttpActionResult GetCourseSurvey(int courseId)
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

            var surveyData = db.Surveys.FirstOrDefault(s => s.CourseId == courseId && s.Course.IsQuest);

            if (surveyData == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { SurveyData = "此課程尚未建立問卷" }
                });
            }
            if (surveyData.Course.Order.Plan.Nutritionist.UserId != id)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 403,
                    Status = "Error",
                    Message = new { SurveyData = "您無權限" }
                });
            }
            var questionsAndAnswers = new Dictionary<string, string[]>();
            for (int i = 1; i <= 25; i++)
            {
                string question = $"Question{i}";
                string answer = (string)surveyData.GetType().GetProperty(question).GetValue(surveyData);
                questionsAndAnswers[question] = new string[] { answer };
            }
            var birthDate = surveyData.Course.Order.User.Birthday;
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (today < birthDate.AddYears(age))
            {
                age--;
            }

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "課程問卷資料",
                Data = new
                {
                    UserName = surveyData.Course.Order.UserName,
                    Age = age,
                    Gender = surveyData.Course.Order.User.Gender.ToString(),
                    Answers = questionsAndAnswers
                }
            };
            return Ok(result);
        }

        #endregion "取得課程問卷"
    }
}