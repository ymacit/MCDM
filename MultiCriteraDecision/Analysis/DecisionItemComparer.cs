using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;

namespace MultiCriteriaDecision.Analysis
{
    public class DecisionItemComparer : IComparer<IDecisionItem>
    {
        public int Compare(IDecisionItem x, IDecisionItem y)
        {
            if (x != null && y != null)
            {
                int typecompare = x.ItemType.CompareTo(y.ItemType);

                if (typecompare != 0)
                {
                    return typecompare;
                }
                else
                {
                    return x.Name.CompareTo(y.Name);
                }
            }
            else if (x != null && y == null)
            {
                return 1;
            }
            else if (x == null && y != null)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
