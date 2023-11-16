using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Categorychair
{
    public int Idcategorychair { get; set; }

    public string? Namecategorychair { get; set; }

    public string? Colorchair { get; set; }

    public int? Price { get; set; }

    public long? Idroom {get;set;}

    public virtual ICollection<Chair> Chairs { get; set; } = new List<Chair>();

    public virtual ICollection<Listcategorychair> Listcategorychairs { get; set; } = new List<Listcategorychair>();
}
