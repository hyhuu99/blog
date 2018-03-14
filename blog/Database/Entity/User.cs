using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    [Table("User")]
    public class User 
    {
        public User()
        {

        }
        [Key]
        public int ID { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public byte[] ImageUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public int Role { get; set; }
      

    }
}
