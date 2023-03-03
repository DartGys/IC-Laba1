using System;
using System.Collections.Generic;

namespace FootBallWebLaba1.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string PlayerName { get; set; }

    public int PlayerNumber { get; set; }

    public string PlayerPosition { get; set; }

    public decimal PlayerSalary { get; set; }

    public DateTime PlayerBirthDate { get; set; }

    public int ClubId { get; set; }

    public virtual Club PlayerNavigation { get; set; }
}
