using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Userofcinema
{
    public int Iduserofcinema { get; set; }

    public long? Iduser { get; set; }

    public long? Idcinema { get; set; }

    public virtual Cinema? IdcinemaNavigation { get; set; }

    public virtual User? IduserNavigation { get; set; }
}
