using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Cinemainterest
{
    public int Idinterest { get; set; }

    public long? Idroom { get; set; }

    public long? Idmovie { get; set; }

    public DateTime? Dateshow { get; set; }

    public DateTime? Times { get; set; } // bắt dầu
    public DateTime? TimeEnd { get; set; } // kết thúc

    public long? Idcinema {get;set;}

    public int statusinterest {get;set;}

    public int  resetTime {get;set;}

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Movie? IdmovieNavigation { get; set; }

    public virtual Room? IdroomNavigation { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
