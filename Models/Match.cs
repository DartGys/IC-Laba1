using System;
using System.Collections.Generic;

namespace FootBallWebLaba1.Models;

public partial class Match
{
    public int MatchId { get; set; }

    public DateTime MatchDate { get; set; }

    public short MatchDuration { get; set; }

    public int StaidumId { get; set; }

    public int HostClubId { get; set; }

    public int GuestClubId { get; set; }

    public int ChampionshipId { get; set; }

    public virtual Championship Championship { get; set; }

    public virtual Club GuestClub { get; set; }

    public virtual Club HostClub { get; set; }

    public virtual Stadium Stadium { get; set; }
}
