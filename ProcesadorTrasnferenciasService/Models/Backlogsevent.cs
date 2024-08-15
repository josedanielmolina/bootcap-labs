using System;
using System.Collections.Generic;

namespace Api.RabbitMQ.Consumer.Models;

public partial class Backlogsevent
{
    public int Id { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? CompleteAt { get; set; }

    public uint EventType { get; set; }

    public string Json { get; set; }
}
