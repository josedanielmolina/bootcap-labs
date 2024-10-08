﻿using System;
using System.Collections.Generic;

namespace Api.Empresa.Models;

public partial class Areasempresa
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
