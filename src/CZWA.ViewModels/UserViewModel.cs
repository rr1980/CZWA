using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Common;

namespace CZWA.ViewModels
{
    public class UserViewModel : IEntity 
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Username { get; set; }

        public IEnumerable<int> Roles { get; set; }
    }
}
