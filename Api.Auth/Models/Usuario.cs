using System;
using System.Collections.Generic;

namespace ApiAuth.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Correo { get; set; }

    public string Contrasenna { get; set; }

    public string Role { get; set; }

    public string CodigoValidacion { get; set; }

    public DateTime ExpiracionCodigo { get; set; }

    public string CodigoRh { get; set; }

    public bool Status { get; set; }
}
