using System;
using System.Collections.Generic;

namespace Api.RabbitMQ.Consumer.Models;

public partial class Transferencia
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int CuentaRemitente { get; set; }

    public int CuentaDestino { get; set; }

    public decimal Monto { get; set; }
}
