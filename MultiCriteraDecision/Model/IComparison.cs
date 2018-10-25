using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public interface IComparison
    {
        IComparisonPerspective Perspective { get; }
        IJudgment Judgment { get; }
        IList<IPairwiseComparison> Pairwises { get; }
    }
}
