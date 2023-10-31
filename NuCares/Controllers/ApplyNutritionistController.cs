using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NSwag.Annotations;
using NuCares.Models;
using NuCares.Security;

namespace NuCares.Controllers
{
    [OpenApiTag("Stdent", Description = "學員")]
    public class ApplyNutritionistController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "申請成為營養師"

        /// <summary>
        /// 申請成為營養師
        /// </summary>
        /// <param name="viewApplyNutritionist"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/applyNutritionist")]
        [JwtAuthFilter]
        public IHttpActionResult ApplyNu(ViewApplyNutritionist viewApplyNutritionist)
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

            var userData = db.Users.SingleOrDefault(u => u.Id == id);

            // 判斷輸入的格式是否正確
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = "申請失敗，欄位輸入錯誤"
                });
            }

            // 將性別轉成數字
            int gender = viewApplyNutritionist.Gender == "male" ? 0 : 1;

            // 將取得的資料更新到資料庫
            userData.UserName = !string.IsNullOrEmpty(viewApplyNutritionist.UserName) ? viewApplyNutritionist.UserName : userData.UserName;
            userData.Gender = !string.IsNullOrEmpty(viewApplyNutritionist.Gender) ? (EnumList.GenderType)gender : userData.Gender;
            userData.CertificateImage = viewApplyNutritionist.CertificateImage;

            try
            {
                db.SaveChanges();

                // 創建一個新的 Nutritionist 物件，給予預設值
                var newNu = new Nutritionist
                {
                    UserId = id,
                    IsPublic = false,
                    Title = userData.UserName,
                    City = "台北市",
                    Expertise = "體重控制,上班族營養,孕期營養,樂齡營養與保健"
                };

                db.Nutritionists.Add(newNu);
                db.SaveChanges();

                var result = new
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "申請成功，等待管理員審核"
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        #endregion "申請成為營養師"
    }
}