using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Food
{
    public int Idfood { get; set; }

    public string? Namefood { get; set; }

    public int? Quantityfood { get; set; }

    public string? Picture { get; set; }

    public int? Pricefood { get; set; }

    public int? Idcategoryfood { get; set; }

    public DateTime datecreate {get;set;}

    public virtual ICollection<Listfoodbill> Listfoodbills { get; set; } = new List<Listfoodbill>();
}
