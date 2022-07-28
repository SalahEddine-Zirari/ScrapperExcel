using Salah.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slah.Domain
{

    public class HourlyPriceAndVolume 
    {
        public DateTime Date { get; set; }
        public IEnumerable<HourlyData> HData { get; set; }
        
    }

   

}
