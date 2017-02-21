﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Username { get; set; }
        public IEnumerable<UserRoleViewModel> Roles { get; set; }
    }
}
