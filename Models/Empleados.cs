﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetCorePractica.Model
{
    public class Empleados
    {
        public int idHospital { get; set; }
        public string apellido { get; set; }
        public string trabajo { get; set; }
        public int salario { get; set; }
    }
}
