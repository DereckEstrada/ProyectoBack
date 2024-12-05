using System;
using System.Collections.Generic;

namespace ProyectoSoftware.Back.BE.Models;

public partial class Rol
{
    public int RolId { get; set; }

    public string DescriptionRol { get; set; } = null!;

    public DateTime DateCreate { get; set; }

    public DateTime? DateModificate { get; set; }

    public int? UserCreate { get; set; }

    public int? UserModificate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
