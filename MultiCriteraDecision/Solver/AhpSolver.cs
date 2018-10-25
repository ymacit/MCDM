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
    public class AhpSolver
    {
        protected IDecision m_Decision = null;
        protected Matrix m_UnweightedMatrix = null;
        protected Matrix m_WeightedMatrix = null;
        protected Matrix m_ResultMatrix = null;
        Dictionary<IDecisionItem, List<CompareItem>> m_AllComparisonItems = null;
        List<CompareItem> m_FlatPairWiseComparison = null;
        Dictionary<IComparisonPerspective, PairwiseMatrixSet> m_Perspectives = null;
        Dictionary<string, IDecisionItem> m_DecisionItemDictionary = null;
        List<IDecisionItem> m_MatrixRows = null;
        List<IDecisionItem> m_MatrixColumns = null;
        public AhpSolver(IDecision decision)
        {
            m_Decision = decision;
        }

        public Matrix Solve()
        {
            FillFlatList();
            FlatComparisons();
            GeneratePairwiseMatrix();
            GenerateUnweightedMatrix();
            GenerateWeightedMatrix();
            return m_ResultMatrix;
        }

        internal void GenerateWeightedMatrix()
        {
            PropagatePerpectiveWeigt(m_Decision.RootPerspective);
            PropagateMatrixWeigt();
            PrintMatrix(m_ResultMatrix, "WeightedW");
            GenerateResultMatrix();
            PrintMatrix(m_ResultMatrix, "Result");
        }

        private void GenerateResultMatrix()
        {
            m_ResultMatrix = new Matrix(m_WeightedMatrix.RowCount, 1);
            for (int i = 0; i < m_WeightedMatrix.RowCount; i++)
            {
                for (int j = 0; j < m_WeightedMatrix.ColumnCount; j++)
                {
                    m_ResultMatrix[i, 0] += m_WeightedMatrix[i, j];
                }
            }

            for (int i = 0; i < m_WeightedMatrix.RowCount; i++)
            {
                 m_ResultMatrix[i, 0] = Math.Round(m_ResultMatrix[i, 0],5);
            }
        }

        private void PropagateMatrixWeigt()
        {
            //clone unweighted matrix
            m_WeightedMatrix = m_UnweightedMatrix.Duplicate();

            List<IComparisonPerspective> tmp_LastNodes = m_Perspectives.Keys.ToList();
            tmp_LastNodes = tmp_LastNodes.Where(node => node.SubPerspectives.Count == 0).ToList();
            PairwiseMatrixSet tmp_ParentSet = null;
            int tmp_SourceVector = -1;
            int tmp_TargetVector = -1;
            double tmp_Value = 0;
            foreach (IComparisonPerspective perspective in tmp_LastNodes)
            {
                tmp_ParentSet = m_Perspectives[perspective.Parent];
                if(tmp_ParentSet!=null)
                {
                    tmp_SourceVector = tmp_ParentSet.VectorList[perspective.Pivot];
                    tmp_TargetVector = m_MatrixColumns.IndexOf(perspective.Pivot);

                    if (tmp_SourceVector != -1 && tmp_TargetVector != -1)
                    {
                        tmp_Value = tmp_ParentSet.WeightMatrix[tmp_SourceVector, 0];
                        for (int i = 0; i < m_WeightedMatrix.RowCount; i++)
                        {
                            m_WeightedMatrix[i, tmp_TargetVector] *= tmp_Value;
                        }
                    }
                }
            }
        }
        private void PropagatePerpectiveWeigt(IComparisonPerspective perspective )
        {
            PairwiseMatrixSet tmp_MatrixSet = null;
            PairwiseMatrixSet tmp_SubMatrixSet = null;
            int tmp_vector = -1;
            if (perspective.SubPerspectives.Count>0)
            {
                tmp_MatrixSet= m_Perspectives[perspective];
                if (tmp_MatrixSet == null || tmp_MatrixSet.WeightMatrix.RowCount!= perspective.SubPerspectives.Count)
                    throw new Exception("Comparison Matrix set is not found or different column count for " + perspective.Pivot.Name);

                //clone and change row<->column position for weight matrix;
                //Matrix tmp_WeightMatrix = tmp_MatrixSet.WeightMatrix.Duplicate();
                //tmp_WeightMatrix.ChangeDimesion(tmp_WeightMatrix.ColumnCount, tmp_WeightMatrix.RowCount);
                System.Diagnostics.Debug.WriteLine("With Child " + perspective.Pivot.Name);
                //Apply child 
                foreach (IComparisonPerspective subPerspective in perspective.SubPerspectives)
                {
                    tmp_vector = tmp_MatrixSet.VectorList[subPerspective.Pivot];
                    if (tmp_vector != -1)
                    {
                        tmp_SubMatrixSet = m_Perspectives[subPerspective];
                        for (int i = 0; i < tmp_SubMatrixSet.WeightMatrix.RowCount; i++)
                        {
                            //Weight by multiply parent weight Value
                            tmp_SubMatrixSet.WeightMatrix[i, 0] = tmp_SubMatrixSet.WeightMatrix[i, 0] * tmp_MatrixSet.WeightMatrix[tmp_vector, 0];
                        }                        
                        //
                        PropagatePerpectiveWeigt(subPerspective);
                    }
                    else
                        throw new Exception("Vector index is not found for " + subPerspective.Pivot.Name + " in " + perspective.Pivot.Name);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No   Child " + perspective.Pivot.Name);
            }
        }

        internal void GenerateUnweightedMatrix()
        {
            m_UnweightedMatrix = new Matrix(m_MatrixRows.Count, m_MatrixColumns.Count);
            m_ResultMatrix = new Matrix(m_MatrixRows.Count, 1);
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
                    Matrix.CopyAtoB(tmp_set.WeightMatrix, 0, 0, m_UnweightedMatrix, 0, tmp_colIndex, tmp_set.WeightMatrix.RowCount, tmp_set.WeightMatrix.ColumnCount);
                }
            }
            PrintMatrix(m_UnweightedMatrix, "Unweighted");
        }

        internal void FlatComparisons()
        {
            m_FlatPairWiseComparison = new List<CompareItem>();
            List<CompareItem> tmp_sublist = null;
            foreach (KeyValuePair<IDecisionItem, List<CompareItem>> pair in m_AllComparisonItems)
            {
                //tmp_sublist = m_Decision.Judgments
                //    .SelectMany(judgment => judgment.Comparisons
                //    .SelectMany(comparison => comparison.Pairwises
                //   .Where(pairwise => pairwise.Relation.Source.ID == pair.Key.ID)
                //   .Select(pwi => new CompareItem() { ComparePivot = comparison.Perspective.Pivot, CompareSource = pwi.Relation.Source, CompareTarget = pwi.Relation.Target, CompareJudge = judgment.Judge, Ratio = pwi.Ratio }))).ToList();

                tmp_sublist = (from j in m_Decision.Judgments
                              from c in j.Comparisons
                              from p in c.Pairwises
                              where p.Relation.Source.ID == pair.Key.ID
                              select new CompareItem() { ComparePivot = c.Perspective.Pivot, CompareSource = p.Relation.Source, CompareTarget = p.Relation.Target, CompareJudge = j.Judge, Ratio = p.Ratio }).ToList();

                //check comparisionlist
                foreach (CompareItem compareItem in tmp_sublist)
                {
                    m_FlatPairWiseComparison.Add(compareItem);
                }
            }
        }

        internal void FlatComparisons2()
        {
            m_FlatPairWiseComparison = new List<CompareItem>();

            foreach (IJudgment judgment in m_Decision.Judgments)
            {
                foreach (IComparison comparison in judgment.Comparisons)
                {
                    foreach (IPairwiseComparison pairwise in comparison.Pairwises)
                    {
                        m_FlatPairWiseComparison.Add(new CompareItem() { ComparePivot = comparison.Perspective.Pivot,  CompareJudge = judgment.Judge, CompareSource = pairwise.Relation.Source, CompareTarget = pairwise.Relation.Target, Ratio = pairwise.Ratio });
                    }
                }
            }            
        }

        internal void GeneratePairwiseMatrix()
        {
            m_Perspectives = new Dictionary<IComparisonPerspective, PairwiseMatrixSet>();
            m_MatrixColumns = new List<IDecisionItem>();
            m_MatrixRows = new List<IDecisionItem>();
            GeneratePairwiseMatrix(m_Decision.RootPerspective);
        }
        private void GeneratePairwiseMatrix(IComparisonPerspective perspective )
        {
            m_Perspectives.Add(perspective, GeneratePairwiseMatrices(perspective));
            if (perspective.SubPerspectives.Count > 0)
            {
                foreach (IComparisonPerspective item in perspective.SubPerspectives)
                {
                    GeneratePairwiseMatrix(item);
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
        private PairwiseMatrixSet GeneratePairwiseMatrices(IComparisonPerspective connectorItem )
        {
            List<CompareItem> tmp_PairwiseList = m_FlatPairWiseComparison.Where(comparison => comparison.ComparePivot.ID == connectorItem.Pivot.ID).ToList();
            Dictionary<IDecisionItem, int> tmp_VectorList = new Dictionary<IDecisionItem,int>();
            int tmp_counter = 0;
            foreach (CompareItem item in tmp_PairwiseList)
            {
                if (!tmp_VectorList.Keys.Contains(item.CompareSource))
                {
                    tmp_VectorList.Add(item.CompareSource, tmp_counter);
                    tmp_counter++;
                }
                if (!tmp_VectorList.Keys.Contains(item.CompareTarget))
                {
                    tmp_VectorList.Add(item.CompareTarget, tmp_counter);
                    tmp_counter++;
                }
            }
            int tmp_count = tmp_VectorList.Count;
            Matrix tmp_RawMatrix = new Matrix(tmp_count, tmp_count);
            Matrix tmp_RowWorkerMatrix = new Matrix(tmp_count,1);

            int tmp_row = 0;
            int tmp_column = 0;
            CompareItem tmp_item;
            for (int i = 0; i < tmp_PairwiseList.Count; i++)
            {
                tmp_item = tmp_PairwiseList[i];
                tmp_row = tmp_VectorList[tmp_item.CompareSource];
                tmp_column = tmp_VectorList[tmp_item.CompareTarget];
                tmp_RawMatrix[tmp_row, tmp_column] = tmp_item.Ratio;
                tmp_RawMatrix[tmp_column, tmp_row] = (1f / tmp_item.Ratio);
            }
            for (int i = 0; i < tmp_VectorList.Count; i++)
            {
                tmp_RawMatrix[i, i] = 1;
            }
            PrintMatrix(tmp_RawMatrix, connectorItem.Pivot.Name + " Raw" );
            tmp_RowWorkerMatrix = GetWeightMatrix(tmp_RawMatrix);
            PrintMatrix(tmp_RowWorkerMatrix, connectorItem.Pivot.Name + " Weight");
            double tmp_ConsistencyRatio = Consistency.Check(tmp_RawMatrix, tmp_RowWorkerMatrix);
            PairwiseMatrixSet tmp_PairwiseMatrix = new PairwiseMatrixSet() { ConsistencyRatio = tmp_ConsistencyRatio, RawMatrix = tmp_RawMatrix, WeightMatrix = tmp_RowWorkerMatrix, VectorList= tmp_VectorList, LastProcessedPerspective =connectorItem };
            return tmp_PairwiseMatrix;
        }

        private Matrix GetWeightMatrix(Matrix ComparisonMatrix)
        {
            int tmp_RowCount = ComparisonMatrix.RowCount;
            int tmp_ColumnCount = ComparisonMatrix.ColumnCount;
            Matrix tmp_NormalizeMatrix = new Matrix(tmp_RowCount, tmp_ColumnCount);
            Matrix tmp_ColumnWorkerMatrix = new Matrix(1, tmp_ColumnCount);
            Matrix tmp_RowWorkerMatrix = new Matrix(tmp_RowCount, 1);
            double tmp_CellSum = 0f;

            //sum column values
            for (int j = 0; j < tmp_ColumnCount; j++)
            {
                tmp_CellSum = 0;
                for (int i = 0; i < tmp_RowCount; i++)
                {
                    tmp_CellSum += ComparisonMatrix[i, j];
                }
                tmp_ColumnWorkerMatrix[0, j] = tmp_CellSum;
            }
            //normalize column values
            for (int j = 0; j < tmp_ColumnCount; j++)
            {
                for (int i = 0; i < tmp_RowCount; i++)
                {
                    tmp_NormalizeMatrix[i, j] = ComparisonMatrix[i, j] / tmp_ColumnWorkerMatrix[0, j];
                }
            }
            PrintMatrix(tmp_NormalizeMatrix, "Normalize" ) ;

            //sum row values and calculate avarage
            for (int i = 0; i < tmp_RowCount; i++)
            {
                tmp_CellSum = 0;
                for (int j = 0; j < tmp_ColumnCount; j++)
                {
                    tmp_CellSum += tmp_NormalizeMatrix[i, j];
                }
                tmp_RowWorkerMatrix[i, 0] = (tmp_CellSum / (tmp_ColumnCount)); //avarage
            }
            //check sum for column values
            for (int j = 0; j < tmp_ColumnCount; j++)
            {
                tmp_CellSum = 0;
                for (int i = 0; i < tmp_RowCount; i++)
                {
                    tmp_CellSum += tmp_NormalizeMatrix[i, j];
                }
                if (Math.Round(tmp_CellSum, 5) != 1)
                    throw new ArithmeticException("Sum of Column value is different from 1");
                tmp_ColumnWorkerMatrix[0, j] = tmp_CellSum;
            }
            return tmp_RowWorkerMatrix;
        }
        private void PrintMatrix(Matrix matrix, string message )
        {
            //print values
            System.Diagnostics.Debug.WriteLine("******* " + message +  " Matrix Values**********");
            System.Diagnostics.Debug.Write(0.ToString().PadLeft(7) + "\t");
            for (int j = 0; j < matrix.ColumnCount; j++)
            {
                System.Diagnostics.Debug.Write(j.ToString().PadLeft(7) + "\t");
            }
            System.Diagnostics.Debug.WriteLine("");
            for (int i = 0; i < matrix.RowCount; i++)
            {
                System.Diagnostics.Debug.Write(i.ToString().PadLeft(7) + "\t");
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    System.Diagnostics.Debug.Write(matrix[i, j].ToString("F5").PadLeft(7) + "\t");
                }
                System.Diagnostics.Debug.WriteLine("");
            }

        }
        internal virtual  Matrix GetResult()
        {
            return null;
        }

        public Matrix UnweightedMatrix { get => m_UnweightedMatrix; }
        public Matrix WeightedMatrix { get => m_WeightedMatrix;  }
        public Matrix ResultMatrix { get => m_ResultMatrix; }

        internal void CreateUnweightedMatrix()
        {
            int tmp_itemcount = m_AllComparisonItems.Count;
            Matrix tmp_Unweighted = new Matrix(tmp_itemcount, tmp_itemcount);
            //foreach (var item in collection)
            //{

            //}
        }
        internal void FillFlatList()
        {
            m_AllComparisonItems = new Dictionary<IDecisionItem, List<CompareItem>>();
            m_DecisionItemDictionary = new Dictionary<string, IDecisionItem>();
            ReadChildItems(m_Decision, (IReadOnlyList<IDecisionItem>)m_Decision.Clusters);
        }
        private  int ReadChildItems(IDecision decision, IReadOnlyList<IDecisionItem> itemList)
        {
            int tmp_itemCount = 0;
            if (itemList.Count > 0)
            {
                foreach (IDecisionItem childitem in itemList)
                {
                    m_DecisionItemDictionary.Add(childitem.ID.ToString(), childitem);
                    this.m_AllComparisonItems.Add(childitem,new List<CompareItem>());
                    tmp_itemCount += ReadChildItems(decision, childitem.Childs);
                }
            }
            return tmp_itemCount;
        }
    }
    internal class PairwiseMatrixSet
    {
        internal Dictionary<IDecisionItem, int> VectorList;
        internal Matrix RawMatrix;
        internal Matrix WeightMatrix;
        internal Double ConsistencyRatio;
        internal IComparisonPerspective LastProcessedPerspective;
    }
}
