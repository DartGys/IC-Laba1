using System;
using System.Collections.Generic;

namespace WebLabaICTry2.Models;

public partial class Club
{
    public int ClubId { get; set; }

    public string ClubName { get; set; } = null!;

    public string ClubOrigin { get; set; } = null!;

    public int ClubPlayerQuantity { get; set; }

    public string ClubCoachName { get; set; } = null!;

    public DateTime ClubEstablishmentDate { get; set; }

    public virtual ICollection<Match> MatchGuestClubs { get; } = new List<Match>();

    public virtual ICollection<Match> MatchHostClubs { get; } = new List<Match>();

    public virtual Player? Player { get; set; }

    public virtual ICollection<Stadium> Stadia { get; } = new List<Stadium>();
}
