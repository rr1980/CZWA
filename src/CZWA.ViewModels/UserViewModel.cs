using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Common;
using System.ComponentModel.DataAnnotations;

namespace CZWA.ViewModels
{
    public class UserViewModel : IEntity 
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Name " + ErrorMessage.REQUIRED)]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Name " + ErrorMessage.MINMAXLENGTH)]
        public string Name { get; set; }
        public string Vorname { get; set; }
        [Required(ErrorMessage = "Username " + ErrorMessage.REQUIRED)]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Username " + ErrorMessage.MINMAXLENGTH)]
        public string Username { get; set; }

        public string ShowName { get; set; }

        public string Password { get; set; }

        public IEnumerable<int> Roles { get; set; }
    }
}
