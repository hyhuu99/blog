using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace blog.Models
{
    public class TagVM
    {

            public int TagId { get; set; }

            public string Name { get; set; }

            public string SlugTag { get; set; }

            public int Frequency { get; set; }
         
    }
}