using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;
using MultiCriteriaDecision.Analysis;
using MultiCriteriaDecision.Solver;
using MultiCriteriaDecision.Helper;

namespace DecisionUnitTest
{
    [TestClass]
    public class AHPUnitTest
    {
        [TestMethod]
        public void MatrixVector()
        {
           // double[,] tmp_array = new double[3, 3] { {0.3000, 0.2857, 0.3750}, { 0.6000, 0.5714, 0.5000 }, {0.1000, 0.1429, 0.1250} };
            double[,] tmp_array = new double[3, 3] { { 1.0, 0.5, 3.0 }, { 2.0, 1.0, 4.0 }, { 0.3333, 0.25, 1.0 } };
            Matrix tmp_matrix = new Matrix(tmp_array);
            double[,] tmp_nAvarage = new double[3, 1] { { 0.3202 }, { 0.5571 }, { 0.1226 } };
            Matrix tmp_nAvgmatrix = new Matrix(tmp_nAvarage);
            //Matrix tmp_result = tmp_matrix * tmp_nAvgmatrix;
            double tmp_resultValue = tmp_matrix.GetMaxEigenvalueByWeight(tmp_nAvgmatrix);

            Assert.IsNull(tmp_matrix, "success");
        }
        [TestMethod]
        public void Test_SchoolSelection1()
        {
            IDecision tmp_project = new Decision("decision1");
            //Goal
            IDecisionItem tmp_ClusterGoal = tmp_project.AddCluster(ItemType.Goal, "Goal");
            IDecisionItem tmp_GoalNode = tmp_ClusterGoal.Add(ItemType.Goal, "Satisfaction with School");
            //Criteria
            IDecisionItem tmp_ClusterCriteria = tmp_project.AddCluster(ItemType.Criteria, "Criteria");
            IDecisionItem tmp_CriteriaLearning = tmp_ClusterCriteria.Add(ItemType.Criteria, "Learning");                    //1
            IDecisionItem tmp_CriteriaFriends = tmp_ClusterCriteria.Add(ItemType.Criteria, "Friends");                      //2
            IDecisionItem tmp_CriteriaSchoolLife = tmp_ClusterCriteria.Add(ItemType.Criteria, "School Life");               //3
            IDecisionItem tmp_CriteriaVocationalTraining = tmp_ClusterCriteria.Add(ItemType.Criteria, "Vocational Training");//4
            IDecisionItem tmp_CriteriaCollegePrep = tmp_ClusterCriteria.Add(ItemType.Criteria, "College Prep");             //5
            IDecisionItem tmp_CriteriaMusicClasses = tmp_ClusterCriteria.Add(ItemType.Criteria, "Music Classes");           //6    
            //Alternative
            IDecisionItem tmp_ClusterAlternative = tmp_project.AddCluster(ItemType.Alternative, "Alternative");
            IDecisionItem tmp_AlternativeSchoolA = tmp_ClusterAlternative.Add(ItemType.Alternative, "School A");
            IDecisionItem tmp_AlternativeSchoolB = tmp_ClusterAlternative.Add(ItemType.Alternative, "School B");
            IDecisionItem tmp_AlternativeSchoolC = tmp_ClusterAlternative.Add(ItemType.Alternative, "School C");

            //criteria perpective
            IComparisonPerspective tmp_connectorItem_c0 = new ComparisonPerspective(tmp_GoalNode, null) ;
            IRelation tmp_Relation_Cr12 = new Relation( tmp_project ) { Source = tmp_CriteriaLearning, Target = tmp_CriteriaFriends };
            IRelation tmp_Relation_Cr13 = new Relation( tmp_project ) { Source = tmp_CriteriaLearning, Target = tmp_CriteriaSchoolLife };
            IRelation tmp_Relation_Cr14 = new Relation( tmp_project ) { Source = tmp_CriteriaLearning, Target = tmp_CriteriaVocationalTraining };
            IRelation tmp_Relation_Cr15 = new Relation( tmp_project ) { Source = tmp_CriteriaLearning, Target = tmp_CriteriaCollegePrep };
            IRelation tmp_Relation_Cr16 = new Relation( tmp_project ) { Source = tmp_CriteriaLearning, Target = tmp_CriteriaMusicClasses };
            IRelation tmp_Relation_Cr23 = new Relation( tmp_project ) { Source = tmp_CriteriaFriends, Target = tmp_CriteriaSchoolLife };
            IRelation tmp_Relation_Cr24 = new Relation( tmp_project ) { Source = tmp_CriteriaFriends, Target = tmp_CriteriaVocationalTraining };
            IRelation tmp_Relation_Cr25 = new Relation( tmp_project ) { Source = tmp_CriteriaFriends, Target = tmp_CriteriaCollegePrep };
            IRelation tmp_Relation_Cr26 = new Relation( tmp_project ) { Source = tmp_CriteriaFriends, Target = tmp_CriteriaMusicClasses };
            IRelation tmp_Relation_Cr34 = new Relation( tmp_project ) { Source = tmp_CriteriaSchoolLife, Target = tmp_CriteriaVocationalTraining };
            IRelation tmp_Relation_Cr35 = new Relation( tmp_project ) { Source = tmp_CriteriaSchoolLife, Target = tmp_CriteriaCollegePrep };
            IRelation tmp_Relation_Cr36 = new Relation( tmp_project ) { Source = tmp_CriteriaSchoolLife, Target = tmp_CriteriaMusicClasses };
            IRelation tmp_Relation_Cr45 = new Relation( tmp_project ) { Source = tmp_CriteriaVocationalTraining, Target = tmp_CriteriaCollegePrep };
            IRelation tmp_Relation_Cr46 = new Relation( tmp_project ) { Source = tmp_CriteriaVocationalTraining, Target = tmp_CriteriaMusicClasses };
            IRelation tmp_Relation_Cr56 = new Relation( tmp_project ) { Source = tmp_CriteriaCollegePrep, Target = tmp_CriteriaMusicClasses };
            ((List<IRelation>)tmp_connectorItem_c0.Relations).AddRange(new List<IRelation>() { tmp_Relation_Cr12, tmp_Relation_Cr13, tmp_Relation_Cr14, tmp_Relation_Cr15, tmp_Relation_Cr16, tmp_Relation_Cr23, tmp_Relation_Cr24, tmp_Relation_Cr25, tmp_Relation_Cr26, tmp_Relation_Cr34, tmp_Relation_Cr35, tmp_Relation_Cr36, tmp_Relation_Cr45, tmp_Relation_Cr46, tmp_Relation_Cr56});
            tmp_project.RootPerspective=tmp_connectorItem_c0;
            
            //Judgment definition
            IJudge tmp_judge1 = new Judge() { Name = "Name1" };
            IJudgment tmp_Judgment1 = new Judgment(tmp_judge1, tmp_project) { ComparisonDate = System.DateTime.Now };
            tmp_project.Judgments.Add(tmp_Judgment1);

            //criteria judgment
            IComparison tmp_comparison_c0 = new Comparison(tmp_connectorItem_c0, tmp_Judgment1);
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr12) { Ratio = 4f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr13) { Ratio = 3f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr14) { Ratio = 1f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr15) { Ratio = 3f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr16) { Ratio = 4f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr23) { Ratio = 7f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr24) { Ratio = 3f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr25) { Ratio = 1f/5 });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr26) { Ratio = 1f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr34) { Ratio = 1f/5 });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr35) { Ratio = 1f/5 });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr36) { Ratio = 1f/6 });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr45) { Ratio = 1f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr46) { Ratio = 1f/3 });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr56) { Ratio = 3f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c0);

            //Criteria and Alternative Relations
            IRelation tmp_Relation_C_A12 = new Relation( tmp_project ) { Source = tmp_AlternativeSchoolA, Target = tmp_AlternativeSchoolB };
            IRelation tmp_Relation_C_A13 = new Relation( tmp_project ) { Source = tmp_AlternativeSchoolA, Target = tmp_AlternativeSchoolC };
            IRelation tmp_Relation_C_A23 = new Relation( tmp_project ) { Source = tmp_AlternativeSchoolB, Target = tmp_AlternativeSchoolC };


            //Learning perpective
            IComparisonPerspective tmp_connectorItem_c1 = new ComparisonPerspective(tmp_CriteriaLearning, tmp_connectorItem_c0);
            ((List<IRelation>)tmp_connectorItem_c1.Relations).AddRange(new List<IRelation>() { tmp_Relation_C_A12, tmp_Relation_C_A13, tmp_Relation_C_A23 });
            tmp_connectorItem_c0.SubPerspectives.Add(tmp_connectorItem_c1);

            //Learning Judgment
            IComparison tmp_comparison_c1 = new Comparison(tmp_connectorItem_c1, tmp_Judgment1);
            tmp_comparison_c1.Pairwises.Add(new PairwiseComparison(tmp_comparison_c1, tmp_Relation_C_A12) { Ratio = 1f/3 });
            tmp_comparison_c1.Pairwises.Add(new PairwiseComparison(tmp_comparison_c1, tmp_Relation_C_A13) { Ratio = 1f/2 });
            tmp_comparison_c1.Pairwises.Add(new PairwiseComparison(tmp_comparison_c1, tmp_Relation_C_A23) { Ratio = 3f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c1);


            //Friends perpective
            IComparisonPerspective tmp_connectorItem_c2 = new ComparisonPerspective(tmp_CriteriaFriends, tmp_connectorItem_c0);
            ((List<IRelation>)tmp_connectorItem_c2.Relations).AddRange(new List<IRelation>() { tmp_Relation_C_A12, tmp_Relation_C_A13, tmp_Relation_C_A23 });
            tmp_connectorItem_c0.SubPerspectives.Add(tmp_connectorItem_c2);

            //Friends Judgment
            IComparison tmp_comparison_c2 = new Comparison(tmp_connectorItem_c2, tmp_Judgment1);
            tmp_comparison_c2.Pairwises.Add(new PairwiseComparison(tmp_comparison_c2, tmp_Relation_C_A12) { Ratio = 1f });
            tmp_comparison_c2.Pairwises.Add(new PairwiseComparison(tmp_comparison_c2, tmp_Relation_C_A13) { Ratio = 1f });
            tmp_comparison_c2.Pairwises.Add(new PairwiseComparison(tmp_comparison_c2, tmp_Relation_C_A23) { Ratio = 1f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c2);

            //School Life perpective
            IComparisonPerspective tmp_connectorItem_c3 = new ComparisonPerspective(tmp_CriteriaSchoolLife, tmp_connectorItem_c0);
            ((List<IRelation>)tmp_connectorItem_c3.Relations).AddRange(new List<IRelation>() { tmp_Relation_C_A12, tmp_Relation_C_A13, tmp_Relation_C_A23 });
            tmp_connectorItem_c0.SubPerspectives.Add(tmp_connectorItem_c3);

            //School Life Judgment
            IComparison tmp_comparison_c3 = new Comparison(tmp_connectorItem_c3, tmp_Judgment1);
            tmp_comparison_c3.Pairwises.Add(new PairwiseComparison(tmp_comparison_c3, tmp_Relation_C_A12) { Ratio = 5f });
            tmp_comparison_c3.Pairwises.Add(new PairwiseComparison(tmp_comparison_c3, tmp_Relation_C_A13) { Ratio = 1f });
            tmp_comparison_c3.Pairwises.Add(new PairwiseComparison(tmp_comparison_c3, tmp_Relation_C_A23) { Ratio = 1f/5 });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c3);

            //Vocational Training perpective
            IComparisonPerspective tmp_connectorItem_c4 = new ComparisonPerspective(tmp_CriteriaVocationalTraining, tmp_connectorItem_c0);
            ((List<IRelation>)tmp_connectorItem_c4.Relations).AddRange(new List<IRelation>() { tmp_Relation_C_A12, tmp_Relation_C_A13, tmp_Relation_C_A23 });
            tmp_connectorItem_c0.SubPerspectives.Add(tmp_connectorItem_c4);

            //Vocational Training Judgment
            IComparison tmp_comparison_c4 = new Comparison(tmp_connectorItem_c4, tmp_Judgment1);
            tmp_comparison_c4.Pairwises.Add(new PairwiseComparison(tmp_comparison_c4, tmp_Relation_C_A12) { Ratio = 9f });
            tmp_comparison_c4.Pairwises.Add(new PairwiseComparison(tmp_comparison_c4, tmp_Relation_C_A13) { Ratio = 7f });
            tmp_comparison_c4.Pairwises.Add(new PairwiseComparison(tmp_comparison_c4, tmp_Relation_C_A23) { Ratio = 1f/5 });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c4);

            //College Prep perpective
            IComparisonPerspective tmp_connectorItem_c5 = new ComparisonPerspective(tmp_CriteriaCollegePrep, tmp_connectorItem_c0);
            ((List<IRelation>)tmp_connectorItem_c5.Relations).AddRange(new List<IRelation>() { tmp_Relation_C_A12, tmp_Relation_C_A13, tmp_Relation_C_A23 });
            tmp_connectorItem_c0.SubPerspectives.Add(tmp_connectorItem_c5);

            //College Prep Judgment
            IComparison tmp_comparison_c5 = new Comparison(tmp_connectorItem_c5, tmp_Judgment1);
            tmp_comparison_c5.Pairwises.Add(new PairwiseComparison(tmp_comparison_c5, tmp_Relation_C_A12) { Ratio = 1f/2 });
            tmp_comparison_c5.Pairwises.Add(new PairwiseComparison(tmp_comparison_c5, tmp_Relation_C_A13) { Ratio = 1f });
            tmp_comparison_c5.Pairwises.Add(new PairwiseComparison(tmp_comparison_c5, tmp_Relation_C_A23) { Ratio = 2f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c5);

            //Music Classes perpective
            IComparisonPerspective tmp_connectorItem_c6 = new ComparisonPerspective(tmp_CriteriaMusicClasses, tmp_connectorItem_c0);
            ((List<IRelation>)tmp_connectorItem_c6.Relations).AddRange(new List<IRelation>() { tmp_Relation_C_A12, tmp_Relation_C_A13, tmp_Relation_C_A23 });
            tmp_connectorItem_c0.SubPerspectives.Add(tmp_connectorItem_c6);

            //Music Classes Judgment
            IComparison tmp_comparison_c6 = new Comparison(tmp_connectorItem_c6, tmp_Judgment1);
            tmp_comparison_c6.Pairwises.Add(new PairwiseComparison(tmp_comparison_c6, tmp_Relation_C_A12) { Ratio = 6f });
            tmp_comparison_c6.Pairwises.Add(new PairwiseComparison(tmp_comparison_c6, tmp_Relation_C_A13) { Ratio = 4f });
            tmp_comparison_c6.Pairwises.Add(new PairwiseComparison(tmp_comparison_c6, tmp_Relation_C_A23) { Ratio = 1f/3 });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c6);


            System.Diagnostics.Debug.WriteLine(tmp_project.ToString());

            AhpSolver tmp_solver = new AhpSolver(tmp_project);
            Matrix resultMatrix =tmp_solver.Solve();

            Assert.IsNotNull(resultMatrix, "success");

        }
    }
}
