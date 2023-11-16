using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Account
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long? Idusers { get; set; }

    public int? points {get;set;}

    public virtual User? IdusersNavigation { get; set; }
}
