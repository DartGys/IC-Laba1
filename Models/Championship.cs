using System;
using System.Collections.Generic;

namespace WebLabaICTry2.Models;

public partial class Championship
{
    public int ChampionshipId { get; set; }

    public string ChampionshipCountry { get; set; } = null!;

    public string ChampionshipName { get; set; } = null!;

    public int ChampionshipClubQuantity { get; set; }
    public int MatchId { get; set; }
    public virtual Match Match { get; set; } = null!;    
}
