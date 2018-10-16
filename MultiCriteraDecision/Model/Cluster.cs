using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public class Cluster : DecisionItem
    {
        public Cluster(DecisionItem parent, string name) : base(parent, name)
        {
        }

        public Cluster(ItemType type, string name) : base(type, name)
        {
        }
    }
}
