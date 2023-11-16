using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Voucher
{
    public int Idvoucher { get; set; }

    public string? Namevoucher { get; set; }

    public int? Price { get; set; }

    public int? Percent { get; set; }

    public string? Note { get; set; }

    public string? Poster { get; set; }
    public int? statuss {get;set;}

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
