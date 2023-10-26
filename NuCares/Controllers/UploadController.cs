using NSwag.Annotations;
using NuCares.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace NuCares.Controllers
{
    [OpenApiTag("Upload", Description = "上傳圖片")]
    public class UploadController : ApiController
    {
        #region "上傳圖片"
        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("upload/image")]
        [JwtAuthFilter]
        public async Task<IHttpActionResult> UploadProfile()
        {
            // 檢查請求是否包含 multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // 檢查資料夾是否存在，若無則建立
            string root = HttpContext.Current.Server.MapPath("~/upload/images");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            try
            {
                // 讀取 MIME 資料
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                // 取得檔案副檔名，單檔用.FirstOrDefault()直接取出，多檔需用迴圈
                string fileNameData = provider.Contents.FirstOrDefault().Headers.ContentDisposition.FileName.Trim('\"');
                string fileType = Path.GetExtension(fileNameData).ToLower(); // 取得檔案副檔名並轉為小寫，例如 ".jpg"

                // 驗證檔案大小
                long maxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB (調整為你需要的大小)
                var fileSize = provider.Contents.FirstOrDefault().Headers.ContentLength;
                if (fileSize > maxFileSizeInBytes)
                {
                    return Content(HttpStatusCode.BadRequest, new { StatusCode = 400, Status = "Error", Message = "上傳檔案太大" });
                }

                // 驗證檔案副檔名
                string[] allowedFileExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedFileExtensions.Contains(fileType))
                {
                    return Content(HttpStatusCode.BadRequest, new { StatusCode = 400, Status = "Error", Message = "只能夠上傳字尾為.gif,.jpg,.bmp,.png的檔案" });
                }

                // 定義檔案名稱
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileType;

                // 儲存圖片，單檔用.FirstOrDefault()直接取出，多檔需用迴圈
                var fileBytes = await provider.Contents.FirstOrDefault().ReadAsByteArrayAsync();
                var outputPath = Path.Combine(root, fileName);
                using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    await output.WriteAsync(fileBytes, 0, fileBytes.Length);
                }

                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Status = "Success",
                    Data = new
                    {
                        ImageUrl = fileName
                    }
                });
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, new { StatusCode = 400, Status = "Error", Message = e });
            }
        }
        #endregion "上傳圖片"
    }
}
