using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Bill
{
    public long Idbill { get; set; }

    public long? Idmovie { get; set; }

    public int? Idvoucher { get; set; }

    public long? Iduser { get; set; }

    public int? Idinterest { get; set; }

    public long? Idcinema { get; set; }

    public int? Quantityticket { get; set; }

    public int? Vat { get; set; }

    public int? Totalamount { get; set; }

    public DateTime? Datebill { get; set; }

    public string? Note { get; set; }

    public int? Statusbill { get; set; }

    public virtual Cinemainterest? IdinterestNavigation { get; set; }

    public virtual User? IduserNavigation { get; set; }

    public virtual Voucher? IdvoucherNavigation { get; set; }

    public virtual ICollection<Listfoodbill> Listfoodbills { get; set; } = new List<Listfoodbill>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
