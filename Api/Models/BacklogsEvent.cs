using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Backlogsevent
{
    public int Id { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? CompleteAt { get; set; }

    public int EventType { get; set; }

    public string Json { get; set; }
}
