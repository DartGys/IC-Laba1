using System;
using System.Collections.Generic;

namespace WebLabaICTry2.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string PlayerName { get; set; } = null!;

    public int PlayerNumber { get; set; }

    public string PlayerPosition { get; set; } = null!;

    public decimal PlayerSalary { get; set; }

    public DateTime PlayerBirthDate { get; set; }

    public int ClubId { get; set; }

    public virtual Club PlayerNavigation { get; set; } = null!;
}
