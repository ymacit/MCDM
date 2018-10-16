using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public class DecisionItem : IDecisionItem
    {
        string m_Name = string.Empty;
        IDecisionItem m_parent = null;
        List<IDecisionItem> m_Childs = null;
        ItemType m_type = ItemType.None;
        public DecisionItem(DecisionItem parent, string name) : this(parent.ItemType, name)
        {
            m_parent = parent;
        }

        public DecisionItem(ItemType type, string name)
        {
            m_Childs = new List<IDecisionItem>();
            m_type = type;
            m_Name = name;
        }

        public string Name { get => m_Name; set => m_Name=value; }        

        public IDecisionItem Parent { get => m_parent; }

        public IReadOnlyList<IDecisionItem> Childs { get => m_Childs; }

        public ItemType ItemType { get => m_type; }

        public IDecisionItem Add(ItemType type, string name)
        {
            DecisionItem tmp_item = new DecisionItem(type, name);
            tmp_item.m_parent = this;
            m_Childs.Add(tmp_item);
            return tmp_item;
        }
        public void Remove(IDecisionItem item)
        {
            m_Childs.Remove(item);
        }

        public void Print()
        {
            System.Diagnostics.Debug.WriteLine(this.m_type.ToString().PadRight(20) + " | " + m_Name);
            if(m_Childs.Count>0)
            {
                System.Diagnostics.Debug.WriteLine(new string('-', 30));
                foreach (IDecisionItem item in m_Childs)
                {
                    ((DecisionItem)item).Print();
                }
                if(m_parent!=null && m_parent.ItemType!=m_type)
                    System.Diagnostics.Debug.WriteLine(new string('-', 30));
            }
        }
    }
}
