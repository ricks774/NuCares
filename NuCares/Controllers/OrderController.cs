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
    [OpenApiTag("Order", Description = "訂單")]
    public class OrderController : ApiController
    {
        private readonly NuCaresDBContext db = new NuCaresDBContext();

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
            var result = new
            {
                StatusCode = 200,
                Status = "Success",
                Message = "訂單新增成功",
                Date = new
                {
                    newOrder
                }
            };
            return Ok(result);
        }
        #endregion "創建訂單（未付款）"
    }
}