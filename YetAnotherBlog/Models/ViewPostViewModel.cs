using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherBlog.Models
{
    public class ViewPostViewModel
    {
        public Posts Post { get; set; }
        public List<ResponseModel> Responses { get; set; }
        public ResponseModel UserResponse { get; set; }
    }
}
