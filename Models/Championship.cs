using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootBallWebLaba1.Models;

public partial class Championship
{
    public int ChampionshipId { get; set; }
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Країна")]
    public string ChampionshipCountry { get; set; }
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва")]
    public string ChampionshipName { get; set; }
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Кількість команд")]
    public int ChampionshipClubQuantity { get; set; }

    public virtual ICollection<Match> Matches { get; } = new List<Match>();
}
