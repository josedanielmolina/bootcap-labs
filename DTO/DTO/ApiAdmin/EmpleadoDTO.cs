using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.ApiAdmin
{
    public class EmpleadoDTO
    {
        public int Id { get; set; }

        public string Nombres { get; set; }

        public string Correo { get; set; }

        public string Cargo { get; set; }

        public string CodigoRh { get; set; }
    }
}
