using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;

namespace MultiCriteriaDecision.Analysis
{
    public class DecisionItem : DecisionItemBase, IDecisionItem
    {
        List<Relation> m_Relations = null;

        public DecisionItem(IDecision project, ItemType type, string name) : base(type, name)
        {
        }

        private DecisionItem(ItemType type, string name) : base(type, name)
        {
        }

        public List<Relation> Relations { get => m_Relations; }

        public IDecisionItem Add(ItemType type, string name)
        {
            DecisionItem tmp_item = new DecisionItem(type, name);
            tmp_item.SetParent(this);
            m_Childs.Add(tmp_item);
            return tmp_item;
        }
        public void Remove(IDecisionItem item)
        {
            m_Childs.Remove(item);
        }
    }
}
