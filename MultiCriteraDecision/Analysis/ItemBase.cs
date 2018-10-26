using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Analysis
{
    public class ItemBase
    {
        string m_Name = string.Empty;
        Guid m_id = Guid.Empty;

        public ItemBase( string name)
        {
            m_id = Guid.NewGuid();
            m_Name = name;
        }
        public string Name { get => m_Name; set => m_Name = value; }

        public Guid ID { get => m_id; }
    }
}
