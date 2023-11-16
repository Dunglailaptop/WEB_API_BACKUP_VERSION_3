using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Comment
{
    public string? Idcomments { get; set; }

    public long? Iduser { get; set; }

    public long? Idvideo { get; set; }

    public virtual User? IduserNavigation { get; set; }

    public virtual Videouser? IdvideoNavigation { get; set; }
}
