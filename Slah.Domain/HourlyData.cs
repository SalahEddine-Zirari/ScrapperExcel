using Slah.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salah.Domain
{
    public class HourlyData
    {

        public DateTime HourOfDay { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }

    }
}
