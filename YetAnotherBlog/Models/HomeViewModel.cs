using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherBlog.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Posts> Posts { get; set; }
    }
}
