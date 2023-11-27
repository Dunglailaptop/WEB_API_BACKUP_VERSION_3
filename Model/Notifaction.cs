using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class Notifaction
{
   public int  idnotifaction  {get;set;}
    public string  messages  {get;set;}
     public DateTime  datecreate {get;set;} 
      public long? iduser {get;set;}
}
