using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class CategoryMovie
{
    public int Idcategorymovie { get; set; }

    public string? Namecategorymovie { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
