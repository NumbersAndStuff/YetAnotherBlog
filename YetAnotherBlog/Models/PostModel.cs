using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherBlog.Models
{
    public class PostModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Post { get; set; }
        public string Tags { get; set; }
        public bool AllowResponses { get; set; }
        public int ResponseCount { get; set; }
        public DateTime TimePosted { get; set; }

        public bool Edited { get; set; }
        public DateTime LastEdited { get; set; }
    }
}
