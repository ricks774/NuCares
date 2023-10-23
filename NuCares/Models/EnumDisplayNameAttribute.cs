using System;

namespace NuCares.Models
{
    internal class EnumDisplayNameAttribute : Attribute
    {
        private string v;

        public EnumDisplayNameAttribute(string v)
        {
            this.v = v;
        }
    }
}