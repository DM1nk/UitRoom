using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UitRoom
{
    [Serializable]
    public class room
    {
        public string idroom { get; set; }
        public string stat { get; set; }
        public string day { get; set; }
        public override string ToString()
        {
            return idroom + "." + stat + "." + day + "." ;
        }
    }
}
