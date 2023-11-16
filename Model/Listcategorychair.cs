using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Listcategorychair
{
    public int Idlistcategorychair { get; set; }

    public long Idroom { get; set; }

    public int Idcategory { get; set; }

    public virtual Categorychair IdcategoryNavigation { get; set; } = null!;

    public virtual Room IdroomNavigation { get; set; } = null!;
}
