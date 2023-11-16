using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Listfoodbill
{
    public int Idlistfood { get; set; }

    public long? Idbill { get; set; }

    public int? Idfood { get; set; }

    public virtual Bill? IdbillNavigation { get; set; }

    public virtual Food? IdfoodNavigation { get; set; }
}
