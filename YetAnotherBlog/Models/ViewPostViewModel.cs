﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherBlog.Models
{
    public class ViewPostViewModel
    {
        public PostModel Post { get; set; }
        public ResponseModel Responses { get; set; }
        public ResponseModel UserResponse { get; set; }
    }
}
