using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminLTE9.Models
{
    public class PassAirFlight
    {
        public Passenger Passenger { get; set; }
        public Flight Flight { get; set; }
        public Airplane Airplane { get; set; }
    }
}