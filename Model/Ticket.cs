using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Ticket
{
    public long Idticket { get; set; }

    public int? Idchair { get; set; }

    public int? Idinterest { get; set; }

    public int? Pricechair { get; set; }

    public long? Idbill { get; set; }

    public virtual Bill? IdbillNavigation { get; set; }

    public virtual Chair? IdchairNavigation { get; set; }

    public virtual Cinemainterest? IdinterestNavigation { get; set; }
}
