using System;
using System.Collections.Generic;

namespace ProyectoSoftware.Back.BE.Models;

public partial class User
{
    public int UserId { get; set; }

    public string NameUser { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Active { get; set; }

    public int? RolId { get; set; }

    public DateTime DateCreate { get; set; }

    public DateTime? DateModificate { get; set; }

    public int? UserCreate { get; set; }

    public int? UserModificate { get; set; }

    public int Attempts { get; set; }

    public string? RecoveredToken { get; set; }

    public bool Blocked { get; set; }

    public DateTime? DateLastAttempts { get; set; }

    public virtual ICollection<Person> People { get; set; } = new List<Person>();

    public virtual Rol? Rol { get; set; }
}
