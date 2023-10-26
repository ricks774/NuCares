using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class EnumList
    {
        public enum GenderType
        {
            男,
            女,
        }

        public enum CityName
        {
            基隆市,
            台北市,
            新北市,
            桃園市,
            新竹市,
            新竹縣,
            苗栗縣,
            台中市,
            彰化縣,
            南投縣,
            雲林縣,
            嘉義市,
            嘉義縣,
            台南市,
            高雄市,
            屏東縣,
            宜蘭縣,
            花蓮縣,
            台東縣,
            澎湖縣,
            金門縣,
            連江縣,
        }

        public enum CourseState
        {
            未開始,
            進行中,
            已結束
        }
    }
}