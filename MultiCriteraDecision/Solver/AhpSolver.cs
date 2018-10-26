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
        protected Matrix m_WeightedMatrix = null;
        protected Matrix m_ResultMatrix = null;
        Dictionary<IDecisionItem, List<CompareItem>> m_AllComparisonItems = null;
        List<CompareItem> m_FlatPairWiseComparison = null;
        Dictionary<IComparisonPerspective, PairwiseMatrixSet> m_Perspectives = null;
        List<IDecisionItem> m_MatrixRows = null;
        List<IDecisionItem> m_MatrixColumns = null;
        private const int m_MaxStringLengt = 15;
        public AhpSolver(IDecision decision)
        {
            m_Decision = decision;
        }

        public Matrix Solve()
        {
            FillFlatList();
            FlatComparisons();
            GeneratePairwiseMatrix();
            PrintTree(m_Decision.RootPerspective, "", 1, true);
            //GenerateUnweightedMatrix();
            GenerateWeightedMatrix();
            PrintTree(m_Decision.RootPerspective, "", 1, true);
            PrintMatrixWithName(m_WeightedMatrix, "Weighted with Name");
            PrintMatrixWithName(m_ResultMatrix, "Result with Name");
            return m_ResultMatrix;
        }

        internal void GenerateWeightedMatrix()
        {
            //PropagatePerpectiveWeigt_2(m_Decision.RootPerspective);
            PropagatePerpectiveWeigt(m_Decision.RootPerspective, null);
            //PropagateMatrixWeigt();
            GenerateJoinedMatrix();
            GenerateResultMatrix();
            PrintMatrix(m_WeightedMatrix, "WeightedW");
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
        private void PropagatePerpectiveWeigt(IComparisonPerspective perspective, PairwiseMatrixSet parentSet)
        {
            PairwiseMatrixSet tmp_CurrentMatrixSet = null;
            int tmp_vector = -1;

            tmp_CurrentMatrixSet = m_Perspectives[perspective];
            if (tmp_CurrentMatrixSet == null)
                throw new Exception("Comparison Matrix set is not found " + perspective.Pivot.Name);

            if (parentSet != null)
            {
                tmp_vector = parentSet.VectorList[perspective.Pivot];
                if (tmp_vector != -1)
                {
                    for (int i = 0; i < tmp_CurrentMatrixSet.WeightMatrix.RowCount; i++)
                    {
                        //Weight by multiply parent weight Value
                        tmp_CurrentMatrixSet.WeightMatrix[i, 0] = tmp_CurrentMatrixSet.WeightMatrix[i, 0] * parentSet.WeightMatrix[tmp_vector, 0];
                    }
                }
                else
                    throw new Exception("Vector index is not found for " + perspective.Pivot.Name);
            }

            if (perspective.SubPerspectives.Count > 0)
            {
                foreach (IComparisonPerspective subPerspective in perspective.SubPerspectives)
                {
                    PropagatePerpectiveWeigt(subPerspective, tmp_CurrentMatrixSet);
                }
            }
        }

        internal void GenerateJoinedMatrix()
        {
            m_WeightedMatrix = new Matrix(m_MatrixRows.Count, m_MatrixColumns.Count);
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
                    Matrix.CopyAtoB(tmp_set.WeightMatrix, 0, 0, m_WeightedMatrix, 0, tmp_colIndex, tmp_set.WeightMatrix.RowCount, tmp_set.WeightMatrix.ColumnCount);
                }
            }
            PrintMatrix(m_WeightedMatrix, "Weighted");
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
            PairwiseMatrixSet tmp_PairwiseMatrix = new PairwiseMatrixSet() { ConsistencyRatio = tmp_ConsistencyRatio, RawMatrix = tmp_RawMatrix, WeightMatrix = tmp_RowWorkerMatrix, VectorList= tmp_VectorList };
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

        public void PrintTree(IComparisonPerspective perspective, string indent, double rate, bool last)
        {
            System.Diagnostics.Debug.Write(indent);
            if (last)
            {
                System.Diagnostics.Debug.Write("\\-");
                indent += "  ";
            }
            else
            {
                System.Diagnostics.Debug.Write("|-");
                indent += "| ";
            }
            System.Diagnostics.Debug.WriteLine(perspective.Pivot.Name + " : " + rate.ToString("F5"));

            PairwiseMatrixSet tmp_matrixSet = m_Perspectives[perspective];
            if (perspective.SubPerspectives.Count > 0)
            {
                int tmp_childCounter = 0;
                foreach (IComparisonPerspective item in perspective.SubPerspectives)
                {
                    int tmp_index = tmp_matrixSet.VectorList[item.Pivot];
                    double tmp_value = tmp_matrixSet.WeightMatrix[tmp_index, 0];
                    tmp_childCounter++;
                    PrintTree(item, indent, tmp_value, perspective.SubPerspectives.Count==tmp_childCounter);
                }
            }
            else
            {
                foreach (KeyValuePair<IDecisionItem, int> pair in tmp_matrixSet.VectorList)
                {
                    System.Diagnostics.Debug.WriteLine(indent + "+-" + pair.Key.Name + " : " + tmp_matrixSet.WeightMatrix[pair.Value, 0].ToString("F5"));
                }
            }
        }

        private void PrintMatrix(Matrix matrix, string message )
        {
            //Dictionary<int, IDecisionItem> tmp_dictionary=dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
            //print values
            System.Diagnostics.Debug.WriteLine("******* " + message +  " Matrix Values**********");
            System.Diagnostics.Debug.Write("".PadLeft(10) + "\t");
            for (int j = 0; j < matrix.ColumnCount; j++)
            {
                System.Diagnostics.Debug.Write(j.ToString().PadLeft(10) + "\t");
            }
            System.Diagnostics.Debug.WriteLine("");
            for (int i = 0; i < matrix.RowCount; i++)
            {
                System.Diagnostics.Debug.Write(i.ToString().PadLeft(10) + "\t");
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    System.Diagnostics.Debug.Write(matrix[i, j].ToString("F5").PadLeft(10) + "\t");
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }
        private void PrintMatrixWithName(Matrix matrix, string message)
        {
            //Dictionary<int, IDecisionItem> tmp_dictionary=dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
            //print values
            System.Diagnostics.Debug.WriteLine("******* " + message + " Matrix Values**********");
            System.Diagnostics.Debug.Write("".ToString().PadLeft(m_MaxStringLengt) + "\t");
            for (int j = 0; j < matrix.ColumnCount; j++)
            {
                System.Diagnostics.Debug.Write(GetNameStrictLengt(m_MatrixColumns[j].Name).PadLeft(m_MaxStringLengt) + "\t");
            }
            System.Diagnostics.Debug.WriteLine("");
            for (int i = 0; i < matrix.RowCount; i++)
            {

                System.Diagnostics.Debug.Write(GetNameStrictLengt(m_MatrixRows[i].Name).PadLeft(m_MaxStringLengt) + "\t");
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    System.Diagnostics.Debug.Write(matrix[i, j].ToString("F5").PadLeft(m_MaxStringLengt) + "\t");
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }
        private string GetNameStrictLengt(string name)
        {
            int tmp_maxlengt = m_MaxStringLengt;
            int tmp_curlengt = -1;
            tmp_curlengt = name.Length;
            if (tmp_curlengt > tmp_maxlengt)
                tmp_curlengt = tmp_maxlengt;             
            return name.Substring(0, tmp_curlengt);
        }


        internal virtual  Matrix GetResult()
        {
            return null;
        }
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
            ReadChildItems(m_Decision, (IReadOnlyList<IDecisionItem>)m_Decision.Clusters);
        }
        private  int ReadChildItems(IDecision decision, IReadOnlyList<IDecisionItemBase> itemList)
        {
            int tmp_itemCount = 0;
            if (itemList.Count > 0)
            {
                foreach (IDecisionItem childitem in itemList)
                {
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
    }
}
