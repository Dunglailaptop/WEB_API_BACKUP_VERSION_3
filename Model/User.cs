using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class User
{
    public long Idusers { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? Birthday { get; set; }

    public int? Idrole { get; set; }

    public string? Avatar { get; set; }
    public int? gender {get;set;}
    public string? address {get;set;}

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Role? IdroleNavigation { get; set; }

    public virtual ICollection<Problem> Problems { get; set; } = new List<Problem>();

    public virtual ICollection<Userofcinema> Userofcinemas { get; set; } = new List<Userofcinema>();

    public virtual ICollection<Videouser> Videousers { get; set; } = new List<Videouser>();
}
