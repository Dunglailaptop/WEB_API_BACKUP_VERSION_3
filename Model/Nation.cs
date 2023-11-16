using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Nation
{
    public int Idnation { get; set; }

    public string? Namenation { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
