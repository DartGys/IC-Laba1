using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootBallWebLaba1.Models;

public partial class Player
{
    public int PlayerId { get; set; }
    [Display(Name = "Імя")]
    public string PlayerName { get; set; }
    [Display(Name = "Номер")]
    public int PlayerNumber { get; set; }
    [Display(Name = "Позиція")]
    public string PlayerPosition { get; set; }
    [Display(Name = "Зарплата")]
    public decimal PlayerSalary { get; set; }
    [Display(Name = "Рік народження")]
    public DateTime PlayerBirthDate { get; set; }

    public int ClubId { get; set; }
    [Display(Name = "Команда")]
    public virtual Club Club { get; set; }

    public virtual ICollection<ScoredGoal> ScoredGoals { get; } = new List<ScoredGoal>();
}
