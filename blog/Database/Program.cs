using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var db = new MiniBlog())
            {
                MiniBlog MBlog = new MiniBlog();
                User us = new User();
                us.Email = "admin@gmail.com";
                us.Password = "a0c1d878ea1ad8761dbc12e3f33836e0";
                us.CreateDate = DateTime.Now;
                us.DisplayName = "Khánh đẹp trai";
                us.Role = 1;
                db.User.Add(us);
                db.SaveChanges();
            }
            Console.WriteLine("complete");
            Console.ReadKey();
        }
    }
}
