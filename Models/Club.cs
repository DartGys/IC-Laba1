using System;
using System.Collections.Generic;

namespace FootBallWebLaba1.Models;

public partial class Club
{
    public int ClubId { get; set; }

    public string ClubName { get; set; }

    public string ClubOrigin { get; set; }

    public int ClubPlayerQuantity { get; set; }

    public string ClubCoachName { get; set; }

    public DateTime ClubEstablishmentDate { get; set; }

    public virtual ICollection<Match> MatchGuestClubs { get; } = new List<Match>();

    public virtual ICollection<Match> MatchHostClubs { get; } = new List<Match>();

    public virtual Player Player { get; set; }

    public virtual ICollection<Stadium> Stadium { get; } = new List<Stadium>();
}
