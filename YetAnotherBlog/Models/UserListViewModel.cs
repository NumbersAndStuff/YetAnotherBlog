using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherBlog.Models
{
    public class User
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class UserListViewModel
    {
        public List<User> Users { get; set; }
    }
}
