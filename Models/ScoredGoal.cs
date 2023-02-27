﻿using System;
using System.Collections.Generic;

namespace WebLabaICTry2.Models;

public partial class ScoredGoal
{
    public int ScoredGoalId { get; set; }

    public int ScoredMinute { get; set; }

    public int PlayerId { get; set; }

    public int MatchId { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}