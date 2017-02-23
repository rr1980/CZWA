using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.Web.TagHelpers
{
    public static class TagHelperTools
    {
        public static string FirstLetterToLower(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("null or empty", data);
            }
            return Char.ToLowerInvariant(data[0]) + data.Substring(1);
        }

        public static string GetID(string id = "")
        {
            return id + Guid.NewGuid();
        }
    }
}
