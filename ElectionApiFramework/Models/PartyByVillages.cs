using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionApiFramework.Models
{
    public class PartyByVillages
    {
        public string VillageName { get; set; }
        public int IndependentCandidate { get; set; }
        public int MCAP { get; set; }
        public int PDM { get; set; }
        public int MULP { get; set; }
        public int Total { get; set; }
    }
}
