using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TagDTO
    {
        public int TagId { get; set; }
        public string NameTag { get; set; }
        public string SlugTag { get; set; }
        public int Frequency { get; set; }
    }
}
