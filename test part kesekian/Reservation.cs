using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_part_kesekian
{
    public class Reservation
    {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string NomorHP { get; set; }
            public DateTime ReservationTime { get; set; }
            public int JumlahOrang { get; set; }
            public string TableNumber { get; set; }
            public string Status { get; set; }
        
    }
}
