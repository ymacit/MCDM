using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MultiCriteriaDecision.Helper;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;
using MultiCriteriaDecision.Analysis;

namespace MultiCriteriaDecision.Solver
{
    public  class AnpSolver : SolverBase
    {
        protected Matrix m_LimitedMatrix = null;
        protected const int m_MatrixPower= 513;
        public AnpSolver(IDecision decision):base(decision)
        {
        }        

        internal Matrix LimitedMatrix { get => m_LimitedMatrix; set => m_LimitedMatrix = value; }


        private void PrepareDimensions(IComparisonPerspective perspective)
        {
            foreach (KeyValuePair<IComparisonPerspective, PairwiseMatrixSet> pair in m_Perspectives)
            {
                    if (!m_MatrixRows.Contains(pair.Key.Pivot))
                        m_MatrixRows.Add(pair.Key.Pivot);
                foreach (IDecisionItem item in pair.Value.VectorList.Keys)
                {
                    if (!m_MatrixRows.Contains(item))
                        m_MatrixRows.Add(item);
                }
            }

            //if (perspective.SubPerspectives.Count > 0)
            //{
            //    foreach (IComparisonPerspective item in perspective.SubPerspectives)
            //    {
            //        PrepareDimensions(item);
            //    }
            //}
            //else
            //{
            //    if (!m_MatrixRows.Contains(perspective.Pivot))
            //        m_MatrixRows.Add(perspective.Pivot);

            //    foreach (IRelation relationItem in perspective.Relations)
            //    {
            //        if (!m_MatrixRows.Contains(relationItem.Source))
            //            m_MatrixRows.Add(relationItem.Source);
            //        if (!m_MatrixRows.Contains(relationItem.Target))
            //            m_MatrixRows.Add(relationItem.Target);
            //    }
            //}
        }

        protected override void GenerateJoinedMatrix()
        {
            m_MatrixRows = new List<IDecisionItem>();
            m_MatrixColumns = new List<IDecisionItem>();
            PrepareDimensions(m_Decision.RootPerspective);
            //matrix must be nxn dimension for ANP
            DecisionItemComparer dc = new DecisionItemComparer();
            m_MatrixRows.Sort(dc);
            m_MatrixColumns.AddRange(m_MatrixRows);
            m_MatrixColumns.Sort(dc);

            m_WeightedMatrix = new Matrix(m_MatrixRows.Count, m_MatrixColumns.Count);
            int tmp_rowIndex = -1;
            int tmp_colIndex = -1;
            PairwiseMatrixSet tmp_set;
            //Dictionary<IComparisonPerspective, PairwiseMatrixSet> tmp_Leaf = m_Perspectives.Where(pairItem => pairItem.Key.SubPerspectives.Count == 0).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (KeyValuePair<IComparisonPerspective, PairwiseMatrixSet> pairItem in m_Perspectives)
            {
                tmp_colIndex = m_MatrixColumns.IndexOf(pairItem.Key.Pivot);
                tmp_set = pairItem.Value;
                foreach (KeyValuePair<IDecisionItem, int> pairVector in tmp_set.VectorList)
                {
                    tmp_rowIndex = m_MatrixRows.IndexOf(pairVector.Key);
                    m_WeightedMatrix[tmp_rowIndex, tmp_colIndex] = tmp_set.WeightMatrix[pairVector.Value, 0];
                }
            }
            for (int i = 0; i < m_MatrixRows.Count; i++)
            {
                if(m_MatrixRows[i].ItemType== ItemType.Alternative)
                    m_WeightedMatrix[i, i] = 1;
            }
        }

        protected override void GenerateResult()
        {
            m_WeightedMatrix = ColumnNormalization(m_WeightedMatrix);
            m_LimitedMatrix = Matrix.Power(m_WeightedMatrix, m_MatrixPower);
            PrintMatrixWithName(m_LimitedMatrix, "anp limited");

            int tmp_colIndex = m_MatrixColumns.FindIndex(decision => decision.ItemType == ItemType.Goal);
            List<IDecisionItem> tmp_Alternatives = m_MatrixRows.Where(decision => decision.ItemType == ItemType.Alternative).ToList();
            int tmp_rowIndex = -1;
            foreach (IDecisionItem item in tmp_Alternatives)
            {
                tmp_rowIndex = m_MatrixRows.IndexOf(item);
                m_DecisionResult.AddAlternative(item, (float)m_LimitedMatrix[tmp_rowIndex, tmp_colIndex]);
            }
        }
    }
}
