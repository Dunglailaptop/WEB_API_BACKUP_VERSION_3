using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Videouser
{
    public long Idvideo { get; set; }

    public string? Videofile { get; set; }

    public long? Iduser { get; set; }

    public string? Titlevideo { get; set; }

    public string? Describes { get; set; }

    public DateTime? Dateup { get; set; }

    public string? Imageview { get; set; }

    public int? Types { get; set; }

    public virtual User? IduserNavigation { get; set; }
}
