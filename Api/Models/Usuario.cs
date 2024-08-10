using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Correo { get; set; }

    public string Contrasena { get; set; }

    public string CodigoValidacion { get; set; }

    public DateTime? FechaExpiracionCodigo { get; set; }
}
