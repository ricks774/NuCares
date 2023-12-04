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
            return imagePath != null ? $@"https://nucares.top/upload/images/{imagePath}" : null;
        }
    }
}