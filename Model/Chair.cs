using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Chair
{
    public int Idchair { get; set; }

    public int? NumberChair { get; set; }

    public int? Roww { get; set; }

    public long? Idroom { get; set; }

    public int? StatusChair { get; set; }

    public int? Idcategorychair { get; set; }

    public string? RowChar { get; set; }

    public virtual Categorychair? IdcategorychairNavigation { get; set; }

    public virtual Room? IdroomNavigation { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
