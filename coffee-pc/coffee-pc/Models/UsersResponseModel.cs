using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_pc.Models
{
    public class UsersResponseModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutDate { get; set; }
        public BindableCollection<string> Roles { get; set; }
        
    }


}
