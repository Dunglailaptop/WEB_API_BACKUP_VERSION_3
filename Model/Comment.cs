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

public partial class CommentVideo {
     public int idcomment {get;set;}
    public long? Iduser {get;set;}
    public long? Idvideo {get;set;}

    public string message {get;set;}

    public int heart {get;set;}

    public int likes {get;set;}
}
public partial class likeandcomment {
     public int idliekandcomment {get;set;} 
     public int likes {get;set;}
     public int comments {get;set;}
     public int idvideo  {get;set;}
     public long Idusers {get;set;}
}