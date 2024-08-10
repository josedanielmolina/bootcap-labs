using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class ChangePasswordDTO
    {
        public string Correo { get; set; }
        public string Codigo { get; set; }
        public string Contrasena { get; set; }
    }
}
