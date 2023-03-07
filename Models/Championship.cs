using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootBallWebLaba1.Models;

public partial class Championship
{
    public int ChampionshipId { get; set; }
    [Display(Name = "Країна")]
    public string ChampionshipCountry { get; set; }
    [Display(Name = "Назва")]
    public string ChampionshipName { get; set; }
    [Display(Name = "Кількість команд")]
    public int ChampionshipClubQuantity { get; set; }

    public virtual ICollection<Match> Matches { get; } = new List<Match>();
}
