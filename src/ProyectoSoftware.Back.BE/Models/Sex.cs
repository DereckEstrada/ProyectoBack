using System;
using System.Collections.Generic;

namespace ProyectoSoftware.Back.BE.Models;

public partial class Sex
{
    public int SexId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
