using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherBlog.Models
{
    public class ResponseModel
    {
        [Key]
        public Guid Id {get; set;}

        // Stores the ID of the blog post this is responding to
        public Guid ResponseTo { get; set; }

        // The body of the response
        public string Contents { get; set; }

        // ID of the user who posted this response
        public Guid PostedBy { get; set; }

        // The post date of the response, will be used to order responses properly
        public Guid DatePosted { get; set; }

        // Is this response hidden (eg for moderation?)
        public bool Hidden { get; set; }
    }
}
