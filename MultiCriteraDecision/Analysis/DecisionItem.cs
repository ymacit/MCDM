using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;

namespace MultiCriteriaDecision.Analysis
{
    public class DecisionItem : ItemBase, IDecisionItem
    {
        List<Relation> m_Relations = null;
        IDecisionItem m_parent = null;
        IDecision m_project = null;
        protected List<IDecisionItem> m_Childs = null;
        ItemType m_type = ItemType.None;
        public DecisionItem(IDecision project, ItemType type, string name) : this(type, name)
        {
            m_project = project;
        }

        private DecisionItem(ItemType type, string name) : base( name)
        {
            m_Childs = new List<IDecisionItem>();
            m_type = type;
        }


        public IDecision ParentProject { get => m_project; }

        public IDecisionItem ParentItem { get => m_parent; }

        public IReadOnlyList<IDecisionItem> Childs { get => m_Childs; }

        public ItemType ItemType { get => m_type; }

        public List<Relation> Relations { get => m_Relations; }

        public IDecisionItem Add(ItemType type, string name)
        {
            DecisionItem tmp_item = new DecisionItem(type, name);
            tmp_item.m_parent=this;
            m_Childs.Add(tmp_item);
            return tmp_item;
        }
        public void Remove(IDecisionItem item)
        {
            m_Childs.Remove(item);
        }

        public override string ToString()
        {
            string tmp_result = this.m_type.ToString().PadRight(20) + " | " + base.Name;

            if (m_Childs.Count > 0)
            {
                tmp_result += "\n" + new string('-', 30) + "\n";
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
