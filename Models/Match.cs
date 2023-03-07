using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootBallWebLaba1.Models;

public partial class Match
{
    public int MatchId { get; set; }
    [Display(Name = "Дата")]
    public DateTime MatchDate { get; set; }
    [Display(Name = "Тривалість")]
    public short MatchDuration { get; set; }

    public int StaidumId { get; set; }

    public int HostClubId { get; set; }

    public int GuestClubId { get; set; }

    public int ChampionshipId { get; set; }
    [Display(Name = "Чемапіонат")]
    public virtual Championship Championship { get; set; }
    [Display(Name = "Команда гостей")]
    public virtual Club GuestClub { get; set; }
    [Display(Name = "Команда хозяїв")]
    public virtual Club HostClub { get; set; }

    public virtual ICollection<ScoredGoal> ScoredGoals { get; } = new List<ScoredGoal>();
    [Display(Name = "Стадіон")]
    public virtual Stadium Stadium { get; set; }
}
