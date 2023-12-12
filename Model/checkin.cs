using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class checkin
{
   public int idcheckin {get;set;}
   public DateTime timestart {get;set;}
   public DateTime timeend {get;set;}
   public long Idusers {get;set;} 
   public int checksession {get;set;}

   public long Idcinema {get;set;}
}
