using System;
using System.Collections.Generic;

namespace ApiAdmin.Models;

public partial class Empleado
{
    public int Id { get; set; }

    public string Nombres { get; set; }

    public string Correo { get; set; }

    public string Cargo { get; set; }

    public string CodigoRh { get; set; }

    public int AreaEmpresaId { get; set; }

    public virtual Areasempresa AreaEmpresa { get; set; }
}
