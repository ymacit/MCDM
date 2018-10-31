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
    public abstract class SolverBase
    {
        protected IDecision m_Decision = null;
        protected DecisionResult m_DecisionResult = null;
        protected Matrix m_WeightedMatrix = null;
        internal List<CompareItem> m_FlatPairWiseComparison = null;
        internal Dictionary<IComparisonPerspective, PairwiseMatrixSet> m_Perspectives = null;
        protected List<IDecisionItem> m_MatrixRows = null;
        protected List<IDecisionItem> m_MatrixColumns = null;
        protected const int m_MaxStringLengt = 15;

        public SolverBase(IDecision decision)
        {
            m_Decision = decision;
            m_DecisionResult = new DecisionResult("Result of " + m_Decision.Name);
        }

        public Matrix WeightedMatrix { get => m_WeightedMatrix; }

        public IDecisionResult Solve()
        {
            PrepareFlatComparisons();
            GeneratePairwiseMatrix();
            //PrintTree(m_Decision.RootPerspective, "", 1, true);
            PropagatePerpectiveWeigt(m_Decision.RootPerspective, null);
            GenerateJoinedMatrix();
            PrintMatrixWithName(m_WeightedMatrix, "Weighted with Name");
            GenerateResult();
            PrintTree(m_Decision.RootPerspective, "", 1, true);
            PrintResultList();
            return m_DecisionResult;
        }

        protected abstract void GenerateJoinedMatrix();

        protected abstract void GenerateResult();

        protected void GeneratePairwiseMatrix()
        {
            m_Perspectives = new Dictionary<IComparisonPerspective, PairwiseMatrixSet>();
            GeneratePairwiseMatrix(m_Decision.RootPerspective);
        }
        private void GeneratePairwiseMatrix(IComparisonPerspective perspective)
        {
            m_Perspectives.Add(perspective, GeneratePairwiseMatrices(perspective));
            if (perspective.SubPerspectives.Count > 0)
            {
                foreach (IComparisonPerspective item in perspective.SubPerspectives)
                {
                    GeneratePairwiseMatrix(item);
                }
            }
        }
        private PairwiseMatrixSet GeneratePairwiseMatrices(IComparisonPerspective connectorItem)
        {
            List<CompareItem> tmp_PairwiseList = m_FlatPairWiseComparison.Where(comparison => comparison.ComparePivot.ID == connectorItem.Pivot.ID).ToList();
            Dictionary<IDecisionItem, int> tmp_VectorList = new Dictionary<IDecisionItem, int>();
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
            Matrix tmp_RowWorkerMatrix = new Matrix(tmp_count, 1);

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
            PrintMatrix(tmp_RawMatrix, connectorItem.Pivot.Name + " Raw");
            tmp_RowWorkerMatrix = GetWeightMatrix(tmp_RawMatrix);
            PrintMatrix(tmp_RowWorkerMatrix, connectorItem.Pivot.Name + " Weight");
            float tmp_ConsistencyRatio = Consistency.Check(tmp_RawMatrix, tmp_RowWorkerMatrix);
            m_DecisionResult.AddConsistency(connectorItem, (float)tmp_ConsistencyRatio);
            PairwiseMatrixSet tmp_PairwiseMatrix = new PairwiseMatrixSet() { ConsistencyRatio = tmp_ConsistencyRatio, RawMatrix = tmp_RawMatrix, WeightMatrix = tmp_RowWorkerMatrix, VectorList = tmp_VectorList };
            return tmp_PairwiseMatrix;
        }

        protected void PrepareFlatComparisons()
        {
            Dictionary<IDecisionItem, List<CompareItem>> tmp_AllComparisonItems =  new Dictionary<IDecisionItem, List<CompareItem>>();
            ReadChildItems((IReadOnlyList<IDecisionItem>)m_Decision.Clusters, tmp_AllComparisonItems);
            
            m_FlatPairWiseComparison = new List<CompareItem>();
            List<CompareItem> tmp_sublist = null;
            foreach (KeyValuePair<IDecisionItem, List<CompareItem>> pair in tmp_AllComparisonItems)
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
        private void ReadChildItems(IReadOnlyList<IDecisionItem> sourceList, Dictionary<IDecisionItem, List<CompareItem>> target)
        {
            foreach (IDecisionItem childitem in sourceList)
            {
                target.Add(childitem, new List<CompareItem>());
                ReadChildItems(childitem.Childs, target);
            }
        }

        internal void PropagatePerpectiveWeigt(IComparisonPerspective perspective, PairwiseMatrixSet parentSet)
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

        protected Matrix GetWeightMatrix(Matrix ComparisonMatrix)
        {
            int tmp_RowCount = ComparisonMatrix.RowCount;
            int tmp_ColumnCount = ComparisonMatrix.ColumnCount;
            Matrix tmp_NormalizeMatrix = ColumnNormalization(ComparisonMatrix);
            Matrix tmp_RowWorkerMatrix = new Matrix(tmp_RowCount, 1);
            double tmp_CellSum = 0f;
           
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
            return tmp_RowWorkerMatrix;
        }

        protected Matrix ColumnNormalization(Matrix rawMatrix)
        {
            int tmp_RowCount = rawMatrix.RowCount;
            int tmp_ColumnCount = rawMatrix.ColumnCount;
            Matrix tmp_NormalizeMatrix = new Matrix(tmp_RowCount, tmp_ColumnCount);
            Matrix tmp_ColumnWorkerMatrix = new Matrix(1, tmp_ColumnCount);
            double tmp_CellSum = 0f;

            //sum column values
            for (int j = 0; j < tmp_ColumnCount; j++)
            {
                tmp_CellSum = 0;
                for (int i = 0; i < tmp_RowCount; i++)
                {
                    tmp_CellSum += rawMatrix[i, j];
                }
                tmp_ColumnWorkerMatrix[0, j] = tmp_CellSum;
            }
            //normalize column values
            for (int j = 0; j < tmp_ColumnCount; j++)
            {
                for (int i = 0; i < tmp_RowCount; i++)
                {
                    tmp_NormalizeMatrix[i, j] = rawMatrix[i, j] / tmp_ColumnWorkerMatrix[0, j];
                }
            }
            //check sum for column values
            PrintMatrix(tmp_NormalizeMatrix, "Normalize");
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
            return tmp_NormalizeMatrix;
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
                    PrintTree(item, indent, tmp_value, perspective.SubPerspectives.Count == tmp_childCounter);
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

        protected void PrintMatrix(Matrix matrix, string message)
        {
            //Dictionary<int, IDecisionItem> tmp_dictionary=dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
            //print values
            System.Diagnostics.Debug.WriteLine("******* " + message + " Matrix Values**********");
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
        protected void PrintMatrixWithName(Matrix matrix, string message)
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
        protected string GetNameStrictLengt(string name)
        {
            int tmp_maxlengt = m_MaxStringLengt;
            int tmp_curlengt = -1;
            tmp_curlengt = name.Length;
            if (tmp_curlengt > tmp_maxlengt)
                tmp_curlengt = tmp_maxlengt;
            return name.Substring(0, tmp_curlengt);
        }

        protected void PrintResultList()
        {
            //Result list is prepared in GenerateResultMatrix function
            //Nothing to do
            System.Diagnostics.Debug.WriteLine("*******  Result with Name *********");
            System.Diagnostics.Debug.Write("".ToString().PadLeft(m_MaxStringLengt) + "\t");
            System.Diagnostics.Debug.WriteLine("Alternatives");
            foreach (KeyValuePair<IDecisionItem, float> pair in m_DecisionResult.Alternatives)
            {
                System.Diagnostics.Debug.Write(GetNameStrictLengt(pair.Key.Name).PadLeft(m_MaxStringLengt) + "\t");
                System.Diagnostics.Debug.WriteLine(pair.Value.ToString("F5").PadLeft(m_MaxStringLengt) + "\t");
            }
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
