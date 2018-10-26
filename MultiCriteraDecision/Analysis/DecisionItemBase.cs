using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;

namespace MultiCriteriaDecision.Analysis
{
    public class DecisionItemBase : ItemBase, IDecisionItemBase
    {
        IDecisionItemBase m_parent = null;
        IDecision m_project = null;
        protected List<IDecisionItemBase> m_Childs = null;
        ItemType m_type = ItemType.None;

        public DecisionItemBase(IDecision project, ItemType type, string name) : this(type, name)
        {
            m_project = project;
        }

        protected DecisionItemBase(ItemType type, string name) :base(name)
        {
            m_Childs = new List<IDecisionItemBase>();
            m_type = type;
        }

        public IDecision ParentProject { get => m_project; }

        public IDecisionItemBase ParentItem { get => m_parent; }

        public IReadOnlyList<IDecisionItemBase> Childs { get => m_Childs; }

        public ItemType ItemType { get => m_type; }

        internal void SetParent(DecisionItemBase parent)
        {
            m_parent = parent;
        }
        public override string ToString()
        {
            string tmp_result = this.m_type.ToString().PadRight(20) + " | " + base.Name ;

            if (m_Childs.Count > 0)
            {
                tmp_result += "\n"+ new string('-', 30) + "\n";
                foreach (IDecisionItem item in m_Childs)
                {
                    tmp_result += item.ToString() + "\n";
                }
                if (m_parent != null && m_parent.ItemType != m_type)
                    tmp_result += new string('-', 30) + "\n";
            }
            return tmp_result;
        }
    }
}
