using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetCorePractica.Models
{
    public class EmpHospInfo
    {
        public List<string> Apellidos { get; set; }
        public decimal SumaSalarial { get; set; }
        public decimal MediaSalarial { get; set; }
        public int Personas { get; set; }
    }
}
