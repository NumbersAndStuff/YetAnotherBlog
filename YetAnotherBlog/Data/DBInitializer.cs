using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherBlog.Models;

namespace YetAnotherBlog.Data
{
    public class DBInitializer
    {
        public static void Seed (IApplicationBuilder applicationBuilder)
        {
            ApplicationDbContext context = (ApplicationDbContext)(applicationBuilder.ApplicationServices.GetService(typeof(ApplicationDbContext)));

            if (!context.Users.Any())
            {
                ApplicationUser admin = new ApplicationUser();

            }
        }
    }
}
