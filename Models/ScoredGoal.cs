using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootBallWebLaba1.Models;

public partial class ScoredGoal
{
    public int ScoredGoalId { get; set; }
    [Display(Name = "Хвилина гола")]
    public int ScoredMinute { get; set; }

    public int PlayerId { get; set; }

    public int MatchId { get; set; }
    [Display(Name = "Матч")]
    public virtual Match Match { get; set; }
    [Display(Name = "Гравець")]
    public virtual Player Player { get; set; }
}
