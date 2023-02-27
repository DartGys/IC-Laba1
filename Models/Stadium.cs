using System;
using System.Collections.Generic;

namespace WebLabaICTry2.Models;

public partial class Stadium
{
    public int StadiumId { get; set; }

    public string StadiumLocation { get; set; } = null!;

    public int StadiumCapacity { get; set; }

    public DateTime StadiumEstablismentDate { get; set; }

    public int ClubId { get; set; }

    public virtual Club Club { get; set; } = null!;

    public virtual ICollection<Match> Matches { get; } = new List<Match>();
}
