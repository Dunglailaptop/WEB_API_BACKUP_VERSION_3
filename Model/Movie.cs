using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Movie
{
    public long Idmovie { get; set; }

    public string? Namemovie { get; set; }

    public string? Author { get; set; }

    public DateTime? Yearbirthday { get; set; }

    public int? Idcategorymovie { get; set; }

    public int? Idnation { get; set; }

    public int? Timeall { get; set; }

    public string? Describes { get; set; }

    public string? Poster { get; set; }

    public long? Idvideo { get; set; }

    public int? Statusshow { get; set; }

    public virtual ICollection<Cinemainterest> Cinemainterests { get; set; } = new List<Cinemainterest>();

    public virtual CategoryMovie? IdcategorymovieNavigation { get; set; }

    public virtual Nation? IdnationNavigation { get; set; }
}
