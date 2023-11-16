using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Cinema
{
    public long Idcinema { get; set; }

    public string? Namecinema { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Picture { get; set; }

    public string? Describes { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Userofcinema> Userofcinemas { get; set; } = new List<Userofcinema>();
}
