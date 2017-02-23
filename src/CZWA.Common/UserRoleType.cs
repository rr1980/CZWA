using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CZWA.Common
{
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum UserRoleType
    {
        Admin,
        Default
    }
}
