using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuCares.helper
{
    public class ImageUrl
    {
        public static string GetImageUrl(string imagePath)
        {
            return imagePath != null ? $@"https://localhost:44354/upload/images/{imagePath}" : null;
        }
    }
}