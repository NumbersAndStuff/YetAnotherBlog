using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherBlog.Models
{
    public class File
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class FilesViewModel
    {
        public List<File> Files { get; set; }
    }
}
