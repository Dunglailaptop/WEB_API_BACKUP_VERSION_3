using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Role
{
    public int Idrole { get; set; }

    public string? IdName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
