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
    public class AhpSolver : SolverBase
    {
        public AhpSolver(IDecision decision):base(decision)
        {
        }

        protected override void GenerateResult()
        {
            double tmp_rowsum = 0;
            for (int i = 0; i < m_MatrixRows.Count; i++)
            {
                tmp_rowsum = 0;
                for (int j = 0; j < m_WeightedMatrix.ColumnCount; j++)
                {
                    tmp_rowsum += m_WeightedMatrix[i, j];
                }
                m_DecisionResult.AddAlternative(m_MatrixRows[i], (float)Math.Round(tmp_rowsum, 5));
            }
        }

        protected override void GenerateJoinedMatrix()
        {
            m_MatrixColumns = new List<IDecisionItem>();
            m_MatrixRows = new List<IDecisionItem>();
            PrepareDimensions(m_Decision.RootPerspective);

            m_WeightedMatrix = new Matrix(m_MatrixRows.Count, m_MatrixColumns.Count);
            //int tmp_rowIndex = -1;
            int tmp_colIndex = -1;
            PairwiseMatrixSet tmp_set;
            Dictionary<IComparisonPerspective, PairwiseMatrixSet> tmp_Leaf = m_Perspectives.Where(pairItem => pairItem.Key.SubPerspectives.Count == 0).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (KeyValuePair<IComparisonPerspective, PairwiseMatrixSet> pairItem in tmp_Leaf)
            {
                tmp_colIndex = m_MatrixColumns.IndexOf(pairItem.Key.Pivot);
                if (tmp_colIndex > -1)
                {
                    tmp_set = pairItem.Value;
                    Matrix.CopyAtoB(tmp_set.WeightMatrix, 0, 0, m_WeightedMatrix, 0, tmp_colIndex, tmp_set.WeightMatrix.RowCount, tmp_set.WeightMatrix.ColumnCount);
                }
            }
        }

        private void PrepareDimensions(IComparisonPerspective perspective)
        {
            if (perspective.SubPerspectives.Count > 0)
            {
                foreach (IComparisonPerspective item in perspective.SubPerspectives)
                {
                    PrepareDimensions(item);
                }
            }
            else
            {
                if (!m_MatrixColumns.Contains(perspective.Pivot))
                    m_MatrixColumns.Add(perspective.Pivot);

                foreach (IRelation relationItem in perspective.Relations)
                {
                    if (!m_MatrixRows.Contains(relationItem.Source))
                        m_MatrixRows.Add(relationItem.Source);
                    if (!m_MatrixRows.Contains(relationItem.Target))
                        m_MatrixRows.Add(relationItem.Target);
                }
            }
        }

    }

}
