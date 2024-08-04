using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.ApiAuth
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public string Correo { get; set; }

        public string Contrasenna { get; set; }

        public string Role { get; set; }

        public string CodigoValidacion { get; set; }

        public DateTime ExpiracionCodigo { get; set; }
    }
}
