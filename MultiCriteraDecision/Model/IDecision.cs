using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IDecision
    {
        string Name { get; set; }
        IDecisionItem AddCluster(ItemType type, string name);
        IList<IDecisionItem> Clusters { get; }
        IComparisonPerspective RootPerspective { get; set; }
        IList<IJudgment> Judgments { get; }
    }
}
