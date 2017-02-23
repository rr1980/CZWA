using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public int SelectedUserId { get; set; } = 2;
        public int[] SelectedRoleId { get; set; } = new int[] { 2 };
    }
}
