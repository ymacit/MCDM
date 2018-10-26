using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IDecision :IBase
    {
        IList<IDecisionItem> Clusters { get; }
        IDecisionItem AddCluster(ItemType type, string name);
        IComparisonPerspective RootPerspective { get; set; }
        IList<IJudgment> Judgments { get; }
    }
}
