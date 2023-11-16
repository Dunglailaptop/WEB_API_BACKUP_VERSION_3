using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Room
{
    public long Idroom { get; set; }

    public int? Allchair { get; set; }

    public string Nameroom { get; set; } = null!;

    public int? Chairinrow { get; set; }

    public int? Statusroom { get; set; }

    public long? Idcinema { get; set; }

    public virtual ICollection<Chair> Chairs { get; set; } = new List<Chair>();

    public virtual ICollection<Cinemainterest> Cinemainterests { get; set; } = new List<Cinemainterest>();

    public virtual Cinema? IdcinemaNavigation { get; set; }

    public virtual ICollection<Listcategorychair> Listcategorychairs { get; set; } = new List<Listcategorychair>();
}
