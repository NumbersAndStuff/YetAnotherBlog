using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherBlog.Models
{
    public class UploadViewModel
    {
        public IEnumerable<IFormFile> File { get; set; }
    }
}
