using Newtonsoft.Json;
using NSwag.Annotations;
using NuCares.Models;
using NuCares.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NuCares.Controllers
{
    [OpenApiTag("Order", Description = "訂單")]
    public class OrderController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

        #region "取得方案"
        /// <summary>
        /// 取得方案
        /// </summary>
        /// <param name="planId">課程方案 ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("plan/{planId}")]
        [JwtAuthFilter]
        public IHttpActionResult GetPlan(int planId)
        {
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int userId = (int)userToken["Id"];

            //檢查會員
            bool checkUser = db.Users.Any(n => n.Id == userId);
            if (!checkUser)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 401,
                    Status = "Error",
                    Message = new { Auth = "請重新登入" }
                });
            }

            //檢查課程方案
            var planData = db.Plans.FirstOrDefault(p => p.Id == planId && !p.IsDelete);
            if (planData == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { Plan = "查無此課程方案" }
                });
            }

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "取得課程方案成功",
                Data = new
                {
                    planData.Nutritionist.Title,
                    planData.CourseName,
                    planData.CourseWeek,
                    planData.CoursePrice
                }
            };
            return Ok(result);
        }
        #endregion "取得方案"

        #region  "創建訂單（未付款）"
        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="viewOrder">新增訂單</param>
        /// <param name="planId">課程方案 ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/{planId}")]
        [JwtAuthFilter]
        public IHttpActionResult CreateOrder([FromBody] ViewOrder viewOrder, int planId)
        {
            var userToken = JwtAuthFilter.GetToken(Request.Headers.Authorization.Parameter);
            int userId = (int)userToken["Id"];

            //檢查會員
            bool checkUser = db.Users.Any(n => n.Id == userId);
            if (!checkUser)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 401,
                    Status = "Error",
                    Message = new { Auth = "請重新登入" }
                });
            }

            //檢查課程方案
            var plan = db.Plans.FirstOrDefault(p => p.Id == planId && !p.IsDelete);
            if (plan == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    StatusCode = 400,
                    Status = "Error",
                    Message = new { Plan = "查無此課程方案" }
                });
            }
            if (!ModelState.IsValid || viewOrder == null)
            {
                var errors = ModelState.Keys
                .Where(key => ModelState[key].Errors.Any())
                .Select(key =>
                {
                    var propertyName = key.Split('.').Last();
                    var errorMessage = ModelState[key].Errors.First().ErrorMessage;
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
            Random random = new Random();
            string orderNumber = DateTime.Now.ToString("yyyyMMdd") + random.Next(100, 1000).ToString();
            var newOrder = new Order
            {
                PlanId = planId,
                UserId = userId,
                OrderNumber = orderNumber,
                ContactTime = viewOrder.ContactTime,
                PaymentMethod = viewOrder.PaymentMethod,
                Invoice = viewOrder.Invoice,
                UserName = viewOrder.UserName,
                UserEmail = viewOrder.UserEmail,
                UserPhone = viewOrder.UserPhone,
                UserLineId = viewOrder.UserLineId,
            };
            db.Orders.Add(newOrder);
            var newCourse = new Course
            {
                OrderId = newOrder.Id
            };
            db.Courses.Add(newCourse);
            db.SaveChanges();
            // 整理金流串接資料
            // 加密用金鑰
            string hashKey = ConfigurationManager.AppSettings["HashKey"];
            string hashIV = ConfigurationManager.AppSettings["HashIV"];

            // 金流接收必填資料
            string merchantID = ConfigurationManager.AppSettings["MerchantID"];
            string tradeInfo = "";
            string tradeSha = "";
            string version = "2.0"; // 參考文件串接程式版本

            // tradeInfo 內容，導回的網址都需為 https 
            string respondType = "JSON"; // 回傳格式
            string timeStamp = ((int)(newOrder.CreateDate - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
            string merchantOrderNo = timeStamp + "_" + newOrder.Id; // 底線後方為訂單ID，解密比對用，不可重覆(規則參考文件)
            string amt = newOrder.Plan.CoursePrice.ToString();
            string itemDesc = newOrder.Plan.CourseName + " / " + newOrder.Plan.Nutritionist.Title + " 營養師";
            string tradeLimit = "600"; // 交易限制秒數
            string notifyURL = @"https://" + Request.RequestUri.Host + "/order/paymentResult";// NotifyURL 填後端接收藍新付款結果的 API 位置，如 : /api/users/getpaymentdata
            string returnURL = "";  // 前端可用 Status: SUCCESS 來判斷付款成功，網址夾帶可拿來取得活動內容
            string email = newOrder.UserEmail; // 通知付款完成用
            string loginType = "0"; // 0不須登入藍新金流會員

            // 將 model 轉換為List<KeyValuePair<string, string>>
            List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>() {
            new KeyValuePair<string, string>("MerchantID", merchantID),
            new KeyValuePair<string, string>("RespondType", respondType),
            new KeyValuePair<string, string>("TimeStamp", timeStamp),
            new KeyValuePair<string, string>("Version", version),
            new KeyValuePair<string, string>("MerchantOrderNo", merchantOrderNo),
            new KeyValuePair<string, string>("Amt", amt),
            new KeyValuePair<string, string>("ItemDesc", itemDesc),
            new KeyValuePair<string, string>("TradeLimit", tradeLimit),
            new KeyValuePair<string, string>("NotifyURL", notifyURL),
            new KeyValuePair<string, string>("ReturnURL", returnURL),
            new KeyValuePair<string, string>("Email", email),
            new KeyValuePair<string, string>("LoginType", loginType),
            new KeyValuePair<string, string>("CREDIT", "1")
        };

            // 將 List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
            var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
            // AES 加密
            tradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, hashKey, hashIV);
            // SHA256 加密
            tradeSha = CryptoUtil.EncryptSHA256($"HashKey={hashKey}&{tradeInfo}&HashIV={hashIV}");

            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "訂單新增成功",
                Data = new
                {
                    newOrder.Id,
                    newOrder.OrderNumber,
                    newOrder.ContactTime,
                    CourseName = newOrder.Plan.CourseName,
                    Nutritionist = newOrder.Plan.Nutritionist.Title,
                    CourseWeek = newOrder.Plan.CourseWeek,
                    CoursePrice = newOrder.Plan.CoursePrice,
                    newOrder.UserId,
                    newOrder.UserName,
                    newOrder.UserEmail,
                    newOrder.UserPhone,
                    newOrder.UserLineId,
                    newOrder.PaymentMethod,
                    newOrder.Invoice,
                    newOrder.IsPayment,
                    CourseId = newCourse.Id,
                    MerchantID = merchantID,
                    TradeInfo = tradeInfo,
                    TradeSha = tradeSha,
                    Version = version
                }
            };
            return Ok(result);
        }
        #endregion "創建訂單（未付款）"

        #region "藍新回傳付款結果"
        /// <summary>
        /// 藍新回傳付款結果
        /// </summary>
        /// <param name="data">藍新回傳</param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/paymentResult")]
        public HttpResponseMessage GetPaymentData(NewebPayReturn data)
        {
            // 加密用金鑰
            string hashKey = ConfigurationManager.AppSettings["HashKey"];
            string hashIV = ConfigurationManager.AppSettings["HashIV"];
            // AES 解密
            string decryptTradeInfo = CryptoUtil.DecryptAESHex(data.TradeInfo, hashKey, hashIV);
            PaymentResult result = JsonConvert.DeserializeObject<PaymentResult>(decryptTradeInfo);
            // 取出交易記錄資料庫的訂單ID
            string[] orderNo = result.Result.MerchantOrderNo.Split('_');
            int logId = Convert.ToInt32(orderNo[1]);

            // 付款失敗跳離執行
            var response = Request.CreateResponse(HttpStatusCode.OK);
            if (!data.Status.Equals("SUCCESS"))
            {
                return response;
            }

            // 用取得的"訂單ID"修改資料庫此筆訂單的付款狀態為 true
            var orderData = db.Orders.FirstOrDefault(o => o.Id == logId);
            orderData.IsPayment = true;
            db.SaveChanges();

            return response;
        }
        #endregion "藍新回傳付款結果"

    }
}