using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public interface IJudgment
    {
        IDecision ParentProject { get; }
        IJudge Judge { get; set; }
        IList<IComparison> Comparisons { get; }
        DateTime ComparisonDate { get; set; }
    }
}
