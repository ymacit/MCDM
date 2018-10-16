using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public class Decision
    {
        List<DecisionItem> m_Clusters = null;
        List<Relation> m_relations = null;
        public Decision()
        {
            m_relations = new List<Relation>();
            m_Clusters = new List<DecisionItem>();
        }
        public string Name { get; set; }
        public List<Cluster> Clusters { get; }
        public List<Relation> ClusterRelations { get; }
    }
}
