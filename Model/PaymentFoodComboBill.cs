using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class FoodCombillPayment
{
    public int id { get; set; }
    public int  IdFoodcombo { get; set; }
 
    public int idFoodlistcombo { get; set; }
    
    public int numbers {get;set;}

    public DateTime datetimes {get;set;}

    public int total_price {get;set;}

    public long? iduser {get;set;}

    public long? idcinemas {get;set;}

    public int statusbillfoodcombo {get;set;}
    
}