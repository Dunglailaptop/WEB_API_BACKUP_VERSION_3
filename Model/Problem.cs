using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Problem
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Describes { get; set; }

    public string? Picture { get; set; }

    public int? Idcategory { get; set; }

    public long? Idusers { get; set; }

    public virtual User? IdusersNavigation { get; set; }
}
