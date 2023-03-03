using System;
using System.Collections.Generic;

namespace FootBallWebLaba1.Models;

public partial class Stadium
{
    public int StadiumId { get; set; }

    public string StadiumLocation { get; set; }

    public int StadiumCapacity { get; set; }

    public DateTime StadiumEstablismentDate { get; set; }

    public int ClubId { get; set; }

    public virtual Club Club { get; set; }

    public virtual ICollection<Match> Matches { get; } = new List<Match>();
}
