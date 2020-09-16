﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GOAP.Cost
{
    public interface IGOAPCostComparer : IComparer<IGOAPCost>
    {
        IGOAPCost ZeroCost { get; }
        IGOAPCost BadCost { get; }
    }
}
