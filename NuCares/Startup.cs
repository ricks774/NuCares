using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using NSwag;
using NSwag.AspNet.Owin;
using NSwag.Generation.Processors.Security;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;

[assembly: OwinStartup(typeof(NuCares.Startup))]

namespace NuCares
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 如需如何設定應用程式的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=316888
            var config = new HttpConfiguration();

            // 針對 JSON 資料使用 camel (JSON 回應會改 camel，但 Swagger 提示不會)
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //設定 CORS，AllowAll 表示允許所有來源
            app.UseCors(CorsOptions.AllowAll);
            // 配置 SignalR Hub
            app.MapSignalR(new HubConfiguration
            {
                EnableDetailedErrors = true, // 可選：啟用詳細錯誤信息
                EnableJavaScriptProxies = true, // 可選：啟用 JavaScript 代理
                EnableJSONP = true  // 可選：允許跨來源的瀏覽器通訊
            });
            //app.MapSignalR("/signalr", new HubConfiguration
            //{
            //    EnableDetailedErrors = true, // 可選：啟用詳細錯誤信息
            //    EnableJavaScriptProxies = true // 可選：啟用 JavaScript 代理
            //});

            //var corsOptions = new CorsOptions
            //{
            //    PolicyProvider = new CorsPolicyProvider
            //    {
            //        PolicyResolver = context =>
            //        {
            //            var policy = new CorsPolicy();
            //            policy.Origins.Add("*"); // 新增您的跨域網址
            //            policy.Headers.Add("*"); // 允許任何標頭
            //            policy.Methods.Add("*"); // 允許任何方法
            //            policy.SupportsCredentials = true; // 允許攜帶認證資訊
            //            return Task.FromResult(policy);
            //        }
            //    }
            //};

            app.UseSwaggerUi3(typeof(Startup).Assembly, settings =>
            {
                // 針對 WebAPI，指定路由包含 Action 名稱
                settings.GeneratorSettings.DefaultUrlTemplate =
                    "api/{controller}/{action}/{id?}";
                // 加入客製化調整邏輯名稱版本等
                settings.PostProcess = document =>
                {
                    document.Info.Title = "NuCares";
                };
                // 加入 Authorization JWT token 定義
                settings.GeneratorSettings.DocumentProcessors.Add(new SecurityDefinitionAppender("Bearer", new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Type into the textbox: Bearer {your JWT token}.",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Scheme = "Bearer" // 不填寫會影響 Filter 判斷錯誤
                }));
                // REF: https://github.com/RicoSuter/NSwag/issues/1304 (每支 API 單獨呈現認證 UI 圖示)
                settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));
            });
            app.UseWebApi(config);
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();
        }
    }
}