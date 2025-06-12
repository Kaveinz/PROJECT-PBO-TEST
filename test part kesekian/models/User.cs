using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_part_kesekian.models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string nama_lengkap { get; set; }
        public string email { get; set; }
        public string nomor_hp { get; set; }
        public DateTime created_at { get; set; }
        public string status { get; set; }
    }
}
