using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Model;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Helper;

namespace MultiCriteriaDecision.Analysis
{
    public class Decision : IDecision
    {
        List<IDecisionItem> m_Clusters = null;

        IComparisonPerspective m_RootPerspective = null;
        List<IJudgment> m_Judgments = null;

        string m_name = null;
        public Decision(string name)
        {
            m_name = name;
            m_Clusters = new List<IDecisionItem>();
            m_Judgments = new List<IJudgment>();
        }
        public string Name { get =>m_name; set=>m_name=value; }

        public IList<IDecisionItem> Clusters { get=>m_Clusters; }

        

        public IDecisionItem AddCluster(ItemType type, string name)
        {
            DecisionItem tmp_item = new DecisionItem(this, type, name);
            m_Clusters.Add(tmp_item);
            return tmp_item;
        }

        public IComparisonPerspective RootPerspective { get => m_RootPerspective; set => m_RootPerspective = value; }

        public IList<IJudgment> Judgments { get => m_Judgments; }

        public override string ToString()
        {
            string tmp_result = new string('-', 30) + "\n";
            tmp_result += this.GetType().Name.PadRight(20) + " | " + m_name +"\n";

            foreach (IDecisionItem item in m_Clusters)
            {
                tmp_result += item.ToString() +"\n";
            }

            return tmp_result;
        }

    }
}
