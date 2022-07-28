using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slah.Domain
{
    public class PeriodicPrice
    {
        public Period Period { get; set; }
        public decimal? Price { get; set; }
        public string Name { get; set; } = null!;
    }
}
