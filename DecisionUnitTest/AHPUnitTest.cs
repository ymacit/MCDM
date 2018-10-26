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
            IJudge tmp_judge1 = new Judge("Name1");
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

        [TestMethod]
        public void Test_CarSelection1()
        {
            IDecision tmp_project = new Decision("decision2");
            //Goal
            IDecisionItem tmp_Cluster_Goal = tmp_project.AddCluster(ItemType.Goal, "Goal");
            IDecisionItem tmp_Goal_Node = tmp_Cluster_Goal.Add(ItemType.Goal, "Select a new car");
            //Criteria
            IDecisionItem tmp_Cluster_Criteria = tmp_project.AddCluster(ItemType.Criteria, "Criteria");
            IDecisionItem tmp_Criteria_Style = tmp_Cluster_Criteria.Add(ItemType.Criteria, "Style");
            IDecisionItem tmp_Criteria_Reliability = tmp_Cluster_Criteria.Add(ItemType.Criteria, "Reliability");                    
            IDecisionItem tmp_Criteria_FuelEconomy = tmp_Cluster_Criteria.Add(ItemType.Criteria, "FuelEconomy");                    

            //Alternative
            IDecisionItem tmp_Cluster_Alternative = tmp_project.AddCluster(ItemType.Alternative, "Alternative");
            IDecisionItem tmp_Alternative_Civic = tmp_Cluster_Alternative.Add(ItemType.Alternative, "Civic");
            IDecisionItem tmp_Alternative_Saturn = tmp_Cluster_Alternative.Add(ItemType.Alternative, "Saturn");
            IDecisionItem tmp_Alternative_Escort = tmp_Cluster_Alternative.Add(ItemType.Alternative, "Escort");
            IDecisionItem tmp_Alternative_Clio = tmp_Cluster_Alternative.Add(ItemType.Alternative, "Clio");

            //criteria perpective
            IComparisonPerspective tmp_connectorItem_c0 = new ComparisonPerspective(tmp_Goal_Node, null);
            IRelation tmp_Relation_Cr12 = new Relation(tmp_project) { Source = tmp_Criteria_Style, Target = tmp_Criteria_Reliability };
            IRelation tmp_Relation_Cr13 = new Relation(tmp_project) { Source = tmp_Criteria_Style, Target = tmp_Criteria_FuelEconomy };
            IRelation tmp_Relation_Cr23 = new Relation(tmp_project) { Source = tmp_Criteria_Reliability, Target = tmp_Criteria_FuelEconomy };
            ((List<IRelation>)tmp_connectorItem_c0.Relations).AddRange(new List<IRelation>() { tmp_Relation_Cr12, tmp_Relation_Cr13, tmp_Relation_Cr23 });
            tmp_project.RootPerspective = tmp_connectorItem_c0;

            //Judgment definition
            IJudge tmp_judge1 = new Judge("Name1");
            IJudgment tmp_Judgment1 = new Judgment(tmp_judge1, tmp_project) { ComparisonDate = System.DateTime.Now };
            tmp_project.Judgments.Add(tmp_Judgment1);

            //criteria judgment
            IComparison tmp_comparison_c0 = new Comparison(tmp_connectorItem_c0, tmp_Judgment1);
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr12) { Ratio = 1f/2 });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr13) { Ratio = 3f });
            tmp_comparison_c0.Pairwises.Add(new PairwiseComparison(tmp_comparison_c0, tmp_Relation_Cr23) { Ratio = 4f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c0);

            //Criteria and Alternative Relations
            IRelation tmp_Relation_C_A12 = new Relation(tmp_project) { Source = tmp_Alternative_Civic, Target = tmp_Alternative_Saturn };
            IRelation tmp_Relation_C_A13 = new Relation(tmp_project) { Source = tmp_Alternative_Civic, Target = tmp_Alternative_Escort };
            IRelation tmp_Relation_C_A14 = new Relation(tmp_project) { Source = tmp_Alternative_Civic, Target = tmp_Alternative_Clio };
            IRelation tmp_Relation_C_A23 = new Relation(tmp_project) { Source = tmp_Alternative_Saturn, Target = tmp_Alternative_Escort };
            IRelation tmp_Relation_C_A24 = new Relation(tmp_project) { Source = tmp_Alternative_Saturn, Target = tmp_Alternative_Clio };
            IRelation tmp_Relation_C_A34 = new Relation(tmp_project) { Source = tmp_Alternative_Escort, Target = tmp_Alternative_Clio };


            //Style perpective
            IComparisonPerspective tmp_connectorItem_c1 = new ComparisonPerspective(tmp_Criteria_Style, tmp_connectorItem_c0);
            ((List<IRelation>)tmp_connectorItem_c1.Relations).AddRange(new List<IRelation>() { tmp_Relation_C_A12, tmp_Relation_C_A13, tmp_Relation_C_A14, tmp_Relation_C_A23, tmp_Relation_C_A24, tmp_Relation_C_A34 });
            tmp_connectorItem_c0.SubPerspectives.Add(tmp_connectorItem_c1);

            //Style Judgment
            IComparison tmp_comparison_c1 = new Comparison(tmp_connectorItem_c1, tmp_Judgment1);
            tmp_comparison_c1.Pairwises.Add(new PairwiseComparison(tmp_comparison_c1, tmp_Relation_C_A12) { Ratio = 1f/4 });
            tmp_comparison_c1.Pairwises.Add(new PairwiseComparison(tmp_comparison_c1, tmp_Relation_C_A13) { Ratio = 4f});
            tmp_comparison_c1.Pairwises.Add(new PairwiseComparison(tmp_comparison_c1, tmp_Relation_C_A14) { Ratio = 1f/6 });
            tmp_comparison_c1.Pairwises.Add(new PairwiseComparison(tmp_comparison_c1, tmp_Relation_C_A23) { Ratio = 4f });
            tmp_comparison_c1.Pairwises.Add(new PairwiseComparison(tmp_comparison_c1, tmp_Relation_C_A24) { Ratio = 1f/4 });
            tmp_comparison_c1.Pairwises.Add(new PairwiseComparison(tmp_comparison_c1, tmp_Relation_C_A34) { Ratio = 1f/5 });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c1);


            //Reliability perpective
            IComparisonPerspective tmp_connectorItem_c2 = new ComparisonPerspective(tmp_Criteria_Reliability, tmp_connectorItem_c0);
            ((List<IRelation>)tmp_connectorItem_c2.Relations).AddRange(new List<IRelation>() { tmp_Relation_C_A12, tmp_Relation_C_A13, tmp_Relation_C_A14, tmp_Relation_C_A23, tmp_Relation_C_A24, tmp_Relation_C_A34 });
            tmp_connectorItem_c0.SubPerspectives.Add(tmp_connectorItem_c2);

            //Reliability Judgment
            IComparison tmp_comparison_c2 = new Comparison(tmp_connectorItem_c2, tmp_Judgment1);
            tmp_comparison_c2.Pairwises.Add(new PairwiseComparison(tmp_comparison_c2, tmp_Relation_C_A12) { Ratio = 2f });
            tmp_comparison_c2.Pairwises.Add(new PairwiseComparison(tmp_comparison_c2, tmp_Relation_C_A13) { Ratio = 5f });
            tmp_comparison_c2.Pairwises.Add(new PairwiseComparison(tmp_comparison_c2, tmp_Relation_C_A14) { Ratio = 1f });
            tmp_comparison_c2.Pairwises.Add(new PairwiseComparison(tmp_comparison_c2, tmp_Relation_C_A23) { Ratio = 3f });
            tmp_comparison_c2.Pairwises.Add(new PairwiseComparison(tmp_comparison_c2, tmp_Relation_C_A24) { Ratio = 2f});
            tmp_comparison_c2.Pairwises.Add(new PairwiseComparison(tmp_comparison_c2, tmp_Relation_C_A34) { Ratio = 1f / 4 });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c2);

            //FuelEconomy perpective
            IComparisonPerspective tmp_connectorItem_c3 = new ComparisonPerspective(tmp_Criteria_FuelEconomy, tmp_connectorItem_c0);
            ((List<IRelation>)tmp_connectorItem_c3.Relations).AddRange(new List<IRelation>() { tmp_Relation_C_A12, tmp_Relation_C_A13, tmp_Relation_C_A14, tmp_Relation_C_A23, tmp_Relation_C_A24, tmp_Relation_C_A34 });
            tmp_connectorItem_c0.SubPerspectives.Add(tmp_connectorItem_c3);

            //FuelEconomy Judgment
            IComparison tmp_comparison_c3 = new Comparison(tmp_connectorItem_c3, tmp_Judgment1);
            tmp_comparison_c3.Pairwises.Add(new PairwiseComparison(tmp_comparison_c3, tmp_Relation_C_A12) { Ratio = 34f /27 });
            tmp_comparison_c3.Pairwises.Add(new PairwiseComparison(tmp_comparison_c3, tmp_Relation_C_A13) { Ratio = 34f /24 });
            tmp_comparison_c3.Pairwises.Add(new PairwiseComparison(tmp_comparison_c3, tmp_Relation_C_A14) { Ratio = 34f /28 });
            tmp_comparison_c3.Pairwises.Add(new PairwiseComparison(tmp_comparison_c3, tmp_Relation_C_A23) { Ratio = 27f /24 });
            tmp_comparison_c3.Pairwises.Add(new PairwiseComparison(tmp_comparison_c3, tmp_Relation_C_A24) { Ratio = 27f /24 });
            tmp_comparison_c3.Pairwises.Add(new PairwiseComparison(tmp_comparison_c3, tmp_Relation_C_A34) { Ratio = 24f /28 });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_c3);

            System.Diagnostics.Debug.WriteLine(tmp_project.ToString());

            AhpSolver tmp_solver = new AhpSolver(tmp_project);
            Matrix resultMatrix = tmp_solver.Solve();

            Assert.IsNotNull(resultMatrix, "success");
        }

        [TestMethod]
        public void Test_CarSelection2()
        {
            //http://en.wikipedia.org/wiki/Analytic_hierarchy_process_%E2%80%94_Car_example
            //https://rpubs.com/gluc/ahp

            IDecision tmp_project = new Decision("Buying a family car");
            //Goal
            IDecisionItem tmp_Cluster_Goal = tmp_project.AddCluster(ItemType.Goal, "Goal");
            IDecisionItem tmp_Goal_Node = tmp_Cluster_Goal.Add(ItemType.Goal, "Choose the best car for the Jones family");
            //Criteria
            //cost, safety, style, and capacity 
            IDecisionItem tmp_Cluster_Criteria = tmp_project.AddCluster(ItemType.Criteria, "Criteria");
            IDecisionItem tmp_Criteria_Cost = tmp_Cluster_Criteria.Add(ItemType.Criteria, "Cost");
            IDecisionItem tmp_Criteria_Safety = tmp_Cluster_Criteria.Add(ItemType.Criteria, "Safety");
            IDecisionItem tmp_Criteria_Style = tmp_Cluster_Criteria.Add(ItemType.Criteria, "Style");
            IDecisionItem tmp_Criteria_Capacity = tmp_Cluster_Criteria.Add(ItemType.Criteria, "Capacity");

            //Cost - Sub Criteria
            //purchase price, fuel costs, maintenance costs, and resale value
            IDecisionItem tmp_SubCriteria_PurchasePrice = tmp_Criteria_Cost.Add(ItemType.SubCriteria, "Purchase Price");
            IDecisionItem tmp_SubCriteria_FuelCost = tmp_Criteria_Cost.Add(ItemType.SubCriteria, "Fuel Cost");
            IDecisionItem tmp_SubCriteria_MaintenanceCost = tmp_Criteria_Cost.Add(ItemType.SubCriteria, "Maintenance Cost");
            IDecisionItem tmp_SubCriteria_ResaleValue = tmp_Criteria_Cost.Add(ItemType.SubCriteria, "Resale Value");

            //Capacity - Sub Criteria
            //cargo capacity and passenger capacity
            IDecisionItem tmp_SubCriteria_CargoCapacity = tmp_Criteria_Capacity.Add(ItemType.SubCriteria, "Cargo Capacity");
            IDecisionItem tmp_SubCriteria_PassengerCapacity = tmp_Criteria_Capacity.Add(ItemType.SubCriteria, "Passenger Capacity");

            //Alternative
            //Accord Sedan, Accord Hybrid Sedan, Pilot SUV, CR-V SUV, Element SUV, and Odyssey Minivan. 
            IDecisionItem tmp_Cluster_Alternative = tmp_project.AddCluster(ItemType.Alternative, "Alternative");
            IDecisionItem tmp_Alternative_AccordSedan = tmp_Cluster_Alternative.Add(ItemType.Alternative, "Accord Sedan");
            IDecisionItem tmp_Alternative_HybridSedan = tmp_Cluster_Alternative.Add(ItemType.Alternative, "Accord Hybrid Sedan");
            IDecisionItem tmp_Alternative_PilotSUV = tmp_Cluster_Alternative.Add(ItemType.Alternative, "Pilot SUV");
            IDecisionItem tmp_Alternative_CrvSUV = tmp_Cluster_Alternative.Add(ItemType.Alternative, "CR-V SUV");
            IDecisionItem tmp_Alternative_ElementSUV = tmp_Cluster_Alternative.Add(ItemType.Alternative, "Element SUV");
            IDecisionItem tmp_Alternative_OdysseyMinivan = tmp_Cluster_Alternative.Add(ItemType.Alternative, "Odyssey Minivan");

            //criteria perpective
            IComparisonPerspective tmp_CriteriaPerpective = new ComparisonPerspective(tmp_Goal_Node, null);
            IRelation tmp_Relation_Cr12 = new Relation(tmp_project) { Source = tmp_Criteria_Cost, Target = tmp_Criteria_Safety };
            IRelation tmp_Relation_Cr13 = new Relation(tmp_project) { Source = tmp_Criteria_Cost, Target = tmp_Criteria_Style };
            IRelation tmp_Relation_Cr14 = new Relation(tmp_project) { Source = tmp_Criteria_Cost, Target = tmp_Criteria_Capacity };
            IRelation tmp_Relation_Cr23 = new Relation(tmp_project) { Source = tmp_Criteria_Safety, Target = tmp_Criteria_Style };
            IRelation tmp_Relation_Cr24 = new Relation(tmp_project) { Source = tmp_Criteria_Safety, Target = tmp_Criteria_Capacity };
            IRelation tmp_Relation_Cr34 = new Relation(tmp_project) { Source = tmp_Criteria_Style, Target = tmp_Criteria_Capacity };
            ((List<IRelation>)tmp_CriteriaPerpective.Relations).AddRange(new List<IRelation>() { tmp_Relation_Cr12, tmp_Relation_Cr13, tmp_Relation_Cr14, tmp_Relation_Cr23, tmp_Relation_Cr24, tmp_Relation_Cr34 });
            tmp_project.RootPerspective = tmp_CriteriaPerpective;

            //Judgment definition
            IJudge tmp_judge1 = new Judge("Name1");
            IJudgment tmp_Judgment1 = new Judgment(tmp_judge1, tmp_project) { ComparisonDate = System.DateTime.Now };
            tmp_project.Judgments.Add(tmp_Judgment1);

            //Goal criteria judgment
            IComparison tmp_CriteriaComparison = new Comparison(tmp_CriteriaPerpective, tmp_Judgment1);
            tmp_CriteriaComparison.Pairwises.Add(new PairwiseComparison(tmp_CriteriaComparison, tmp_Relation_Cr12) { Ratio = 3f });
            tmp_CriteriaComparison.Pairwises.Add(new PairwiseComparison(tmp_CriteriaComparison, tmp_Relation_Cr13) { Ratio = 7f });
            tmp_CriteriaComparison.Pairwises.Add(new PairwiseComparison(tmp_CriteriaComparison, tmp_Relation_Cr14) { Ratio = 3f });
            tmp_CriteriaComparison.Pairwises.Add(new PairwiseComparison(tmp_CriteriaComparison, tmp_Relation_Cr23) { Ratio = 9f });
            tmp_CriteriaComparison.Pairwises.Add(new PairwiseComparison(tmp_CriteriaComparison, tmp_Relation_Cr24) { Ratio = 1f });
            tmp_CriteriaComparison.Pairwises.Add(new PairwiseComparison(tmp_CriteriaComparison, tmp_Relation_Cr34) { Ratio = 1f/7 });
            tmp_Judgment1.Comparisons.Add(tmp_CriteriaComparison);

            //Cost - Sub Criteria perpective
            IRelation tmp_Relation_Cost_Cr12 = new Relation(tmp_project) { Source = tmp_SubCriteria_PurchasePrice, Target = tmp_SubCriteria_FuelCost };
            IRelation tmp_Relation_Cost_Cr13 = new Relation(tmp_project) { Source = tmp_SubCriteria_PurchasePrice, Target = tmp_SubCriteria_MaintenanceCost };
            IRelation tmp_Relation_Cost_Cr14 = new Relation(tmp_project) { Source = tmp_SubCriteria_PurchasePrice, Target = tmp_SubCriteria_ResaleValue };
            IRelation tmp_Relation_Cost_Cr23 = new Relation(tmp_project) { Source = tmp_SubCriteria_FuelCost, Target = tmp_SubCriteria_MaintenanceCost };
            IRelation tmp_Relation_Cost_Cr24 = new Relation(tmp_project) { Source = tmp_SubCriteria_FuelCost, Target = tmp_SubCriteria_ResaleValue };
            IRelation tmp_Relation_Cost_Cr34 = new Relation(tmp_project) { Source = tmp_SubCriteria_MaintenanceCost, Target = tmp_SubCriteria_ResaleValue };

            // Cost - Sub Criteria Judgment
            IComparisonPerspective tmp_perspective_cost_cr = new ComparisonPerspective(tmp_Criteria_Cost, tmp_CriteriaPerpective);
            ((List<IRelation>)tmp_perspective_cost_cr.Relations).AddRange(new List<IRelation>() { tmp_Relation_Cost_Cr12, tmp_Relation_Cost_Cr13, tmp_Relation_Cost_Cr14, tmp_Relation_Cost_Cr23, tmp_Relation_Cost_Cr24, tmp_Relation_Cost_Cr34 });
            tmp_CriteriaPerpective.SubPerspectives.Add(tmp_perspective_cost_cr);

            IComparison tmp_comparison_cost_cr = new Comparison(tmp_perspective_cost_cr, tmp_Judgment1);
            tmp_comparison_cost_cr.Pairwises.Add(new PairwiseComparison(tmp_comparison_cost_cr, tmp_Relation_Cost_Cr12) { Ratio = 2f});
            tmp_comparison_cost_cr.Pairwises.Add(new PairwiseComparison(tmp_comparison_cost_cr, tmp_Relation_Cost_Cr13) { Ratio = 5f });
            tmp_comparison_cost_cr.Pairwises.Add(new PairwiseComparison(tmp_comparison_cost_cr, tmp_Relation_Cost_Cr14) { Ratio = 3f });
            tmp_comparison_cost_cr.Pairwises.Add(new PairwiseComparison(tmp_comparison_cost_cr, tmp_Relation_Cost_Cr23) { Ratio = 2f });
            tmp_comparison_cost_cr.Pairwises.Add(new PairwiseComparison(tmp_comparison_cost_cr, tmp_Relation_Cost_Cr24) { Ratio = 2f });
            tmp_comparison_cost_cr.Pairwises.Add(new PairwiseComparison(tmp_comparison_cost_cr, tmp_Relation_Cost_Cr34) { Ratio = 1f/2 });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_cost_cr);

            //Capacity - Sub Criteria perpective
            IRelation tmp_Relation_Capacity_Cr12 = new Relation(tmp_project) { Source = tmp_SubCriteria_CargoCapacity, Target = tmp_SubCriteria_PassengerCapacity };

            // Capacity - Sub Criteria Judgment
            IComparisonPerspective tmp_perspective_Capacity_cr = new ComparisonPerspective(tmp_Criteria_Capacity, tmp_CriteriaPerpective);
            ((List<IRelation>)tmp_perspective_Capacity_cr.Relations).AddRange(new List<IRelation>() { tmp_Relation_Capacity_Cr12});
            tmp_CriteriaPerpective.SubPerspectives.Add(tmp_perspective_Capacity_cr);

            IComparison tmp_comparison_Capacity_cr = new Comparison(tmp_perspective_Capacity_cr, tmp_Judgment1);
            tmp_comparison_Capacity_cr.Pairwises.Add(new PairwiseComparison(tmp_comparison_cost_cr, tmp_Relation_Capacity_Cr12) { Ratio = 1f/5 });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_Capacity_cr);

            //Alternative Relations
            IRelation tmp_Relation_A12 = new Relation(tmp_project) { Source = tmp_Alternative_AccordSedan, Target = tmp_Alternative_HybridSedan };
            IRelation tmp_Relation_A13 = new Relation(tmp_project) { Source = tmp_Alternative_AccordSedan, Target = tmp_Alternative_PilotSUV };
            IRelation tmp_Relation_A14 = new Relation(tmp_project) { Source = tmp_Alternative_AccordSedan, Target = tmp_Alternative_CrvSUV };
            IRelation tmp_Relation_A15 = new Relation(tmp_project) { Source = tmp_Alternative_AccordSedan, Target = tmp_Alternative_ElementSUV };
            IRelation tmp_Relation_A16 = new Relation(tmp_project) { Source = tmp_Alternative_AccordSedan, Target = tmp_Alternative_OdysseyMinivan };
            IRelation tmp_Relation_A23 = new Relation(tmp_project) { Source = tmp_Alternative_HybridSedan, Target = tmp_Alternative_PilotSUV };
            IRelation tmp_Relation_A24 = new Relation(tmp_project) { Source = tmp_Alternative_HybridSedan, Target = tmp_Alternative_CrvSUV };
            IRelation tmp_Relation_A25 = new Relation(tmp_project) { Source = tmp_Alternative_HybridSedan, Target = tmp_Alternative_ElementSUV };
            IRelation tmp_Relation_A26 = new Relation(tmp_project) { Source = tmp_Alternative_HybridSedan, Target = tmp_Alternative_OdysseyMinivan };
            IRelation tmp_Relation_A34 = new Relation(tmp_project) { Source = tmp_Alternative_PilotSUV, Target = tmp_Alternative_CrvSUV };
            IRelation tmp_Relation_A35 = new Relation(tmp_project) { Source = tmp_Alternative_PilotSUV, Target = tmp_Alternative_ElementSUV };
            IRelation tmp_Relation_A36 = new Relation(tmp_project) { Source = tmp_Alternative_PilotSUV, Target = tmp_Alternative_OdysseyMinivan };
            IRelation tmp_Relation_A45 = new Relation(tmp_project) { Source = tmp_Alternative_CrvSUV, Target = tmp_Alternative_ElementSUV };
            IRelation tmp_Relation_A46 = new Relation(tmp_project) { Source = tmp_Alternative_CrvSUV, Target = tmp_Alternative_OdysseyMinivan };
            IRelation tmp_Relation_A56 = new Relation(tmp_project) { Source = tmp_Alternative_ElementSUV, Target = tmp_Alternative_OdysseyMinivan };

            //Purchase Price -Alternative perpective
            IComparisonPerspective tmp_perspective_price_al = new ComparisonPerspective(tmp_SubCriteria_PurchasePrice, tmp_perspective_cost_cr);
            ((List<IRelation>)tmp_perspective_price_al.Relations).AddRange(new List<IRelation>() { tmp_Relation_A12, tmp_Relation_A13, tmp_Relation_A14, tmp_Relation_A15, tmp_Relation_A16, tmp_Relation_A23, tmp_Relation_A24, tmp_Relation_A25, tmp_Relation_A26, tmp_Relation_A34, tmp_Relation_A35, tmp_Relation_A36, tmp_Relation_A45, tmp_Relation_A46, tmp_Relation_A56 });
            tmp_perspective_cost_cr.SubPerspectives.Add(tmp_perspective_price_al);
            //Purchase Price -Alternative Judgment
            IComparison tmp_comparison_price_al = new Comparison(tmp_perspective_price_al, tmp_Judgment1);
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A12) { Ratio = 9f });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A13) { Ratio = 9f });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A14) { Ratio = 1f });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A15) { Ratio = 1f/2 });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A16) { Ratio = 5f });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A23) { Ratio = 1f });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A24) { Ratio = 1f/9 });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A25) { Ratio = 1f/9 });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A26) { Ratio = 1f / 7 });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A34) { Ratio = 1f / 9 });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A35) { Ratio = 1f / 9 });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A36) { Ratio = 1f / 7 });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A45) { Ratio = 1f / 2 });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A46) { Ratio = 5f });
            tmp_comparison_price_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_price_al, tmp_Relation_A56) { Ratio = 6f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_price_al);

            //Fuel Cost -Alternative perpective
            IComparisonPerspective tmp_perspective_fuelCost_al = new ComparisonPerspective(tmp_SubCriteria_FuelCost, tmp_perspective_cost_cr);
            ((List<IRelation>)tmp_perspective_fuelCost_al.Relations).AddRange(new List<IRelation>() { tmp_Relation_A12, tmp_Relation_A13, tmp_Relation_A14, tmp_Relation_A15, tmp_Relation_A16, tmp_Relation_A23, tmp_Relation_A24, tmp_Relation_A25, tmp_Relation_A26, tmp_Relation_A34, tmp_Relation_A35, tmp_Relation_A36, tmp_Relation_A45, tmp_Relation_A46, tmp_Relation_A56 });
            tmp_perspective_cost_cr.SubPerspectives.Add(tmp_perspective_fuelCost_al);
            //Purchase Price -Alternative Judgment
            IComparison tmp_comparison_fuelCost_al = new Comparison(tmp_perspective_fuelCost_al, tmp_Judgment1);
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A12) { Ratio = 1f / 1.13f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A13) { Ratio = 1.41f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A14) { Ratio = 1.15f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A15) { Ratio = 1.24f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A16) { Ratio = 1.19f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A23) { Ratio = 1.59f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A24) { Ratio = 1.30f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A25) { Ratio = 1.40f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A26) { Ratio = 1.35f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A34) { Ratio = 1f / 1.23f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A35) { Ratio = 1f / 1.14f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A36) { Ratio = 1f / 1.18f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A45) { Ratio = 1.08f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A46) { Ratio = 1.04f });
            tmp_comparison_fuelCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_fuelCost_al, tmp_Relation_A56) { Ratio = 1f / 1.04f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_fuelCost_al);

            //Maintenance Cost -Alternative perpective
            IComparisonPerspective tmp_perspective_maintenanceCost_al = new ComparisonPerspective(tmp_SubCriteria_MaintenanceCost, tmp_perspective_cost_cr);
            ((List<IRelation>)tmp_perspective_maintenanceCost_al.Relations).AddRange(new List<IRelation>() { tmp_Relation_A12, tmp_Relation_A13, tmp_Relation_A14, tmp_Relation_A15, tmp_Relation_A16, tmp_Relation_A23, tmp_Relation_A24, tmp_Relation_A25, tmp_Relation_A26, tmp_Relation_A34, tmp_Relation_A35, tmp_Relation_A36, tmp_Relation_A45, tmp_Relation_A46, tmp_Relation_A56 });
            tmp_perspective_cost_cr.SubPerspectives.Add(tmp_perspective_maintenanceCost_al);
            //Maintenance Cost -Alternative Judgment
            IComparison tmp_comparison_maintenanceCost_al = new Comparison(tmp_perspective_maintenanceCost_al, tmp_Judgment1);
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A12) { Ratio = 1.5f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A13) { Ratio = 4f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A14) { Ratio = 4f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A15) { Ratio = 4f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A16) { Ratio = 5f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A23) { Ratio = 4f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A24) { Ratio = 4f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A25) { Ratio = 4f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A26) { Ratio = 5f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A34)  { Ratio = 1f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A35) { Ratio = 1.2f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A36) { Ratio = 1f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A45) { Ratio = 1f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A46) { Ratio = 3f });
            tmp_comparison_maintenanceCost_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_maintenanceCost_al, tmp_Relation_A56) { Ratio = 2f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_maintenanceCost_al);

            //Resale Value -Alternative perpective
            IComparisonPerspective tmp_perspective_resaleValue_al = new ComparisonPerspective(tmp_SubCriteria_ResaleValue, tmp_perspective_cost_cr);
            ((List<IRelation>)tmp_perspective_resaleValue_al.Relations).AddRange(new List<IRelation>() { tmp_Relation_A12, tmp_Relation_A13, tmp_Relation_A14, tmp_Relation_A15, tmp_Relation_A16, tmp_Relation_A23, tmp_Relation_A24, tmp_Relation_A25, tmp_Relation_A26, tmp_Relation_A34, tmp_Relation_A35, tmp_Relation_A36, tmp_Relation_A45, tmp_Relation_A46, tmp_Relation_A56 });
            tmp_perspective_cost_cr.SubPerspectives.Add(tmp_perspective_resaleValue_al);
            //Resale Value -Alternative Judgment
            IComparison tmp_comparison_resaleValue_al = new Comparison(tmp_perspective_resaleValue_al, tmp_Judgment1);
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A12) { Ratio = 3f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A13) { Ratio = 4f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A14) { Ratio = 1f/2f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A15) { Ratio = 2f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A16) { Ratio = 2f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A23) { Ratio = 2f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A24) { Ratio = 1f/5f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A25) { Ratio = 1f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A26) { Ratio = 1f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A34) { Ratio = 1f /6f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A35) { Ratio = 1f/2f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A36) { Ratio = 1f/2f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A45) { Ratio = 4f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A46) { Ratio = 4f });
            tmp_comparison_resaleValue_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_resaleValue_al, tmp_Relation_A56) { Ratio = 1f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_resaleValue_al);

            //Safety -Alternative perpective
            IComparisonPerspective tmp_perspective_safety_al = new ComparisonPerspective(tmp_Criteria_Safety, tmp_CriteriaPerpective);
            ((List<IRelation>)tmp_perspective_safety_al.Relations).AddRange(new List<IRelation>() { tmp_Relation_A12, tmp_Relation_A13, tmp_Relation_A14, tmp_Relation_A15, tmp_Relation_A16, tmp_Relation_A23, tmp_Relation_A24, tmp_Relation_A25, tmp_Relation_A26, tmp_Relation_A34, tmp_Relation_A35, tmp_Relation_A36, tmp_Relation_A45, tmp_Relation_A46, tmp_Relation_A56 });
            tmp_CriteriaPerpective.SubPerspectives.Add(tmp_perspective_safety_al);
            //Safety -Alternative Judgment
            IComparison tmp_comparison_safety_al = new Comparison(tmp_perspective_safety_al, tmp_Judgment1);
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A12) { Ratio = 1f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A13) { Ratio = 5f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A14) { Ratio = 7f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A15) { Ratio = 9f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A16) { Ratio = 1f/3f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A23) { Ratio = 5f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A24) { Ratio = 7f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A25) { Ratio = 9f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A26) { Ratio = 1f/3f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A34) { Ratio = 2f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A35) { Ratio = 9f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A36) { Ratio = 1f/8f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A45) { Ratio = 2f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A46) { Ratio = 1f/8f });
            tmp_comparison_safety_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_safety_al, tmp_Relation_A56) { Ratio = 1f/9f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_safety_al);

            //Style -Alternative perpective
            IComparisonPerspective tmp_perspective_style_al = new ComparisonPerspective(tmp_Criteria_Style, tmp_CriteriaPerpective);
            ((List<IRelation>)tmp_perspective_style_al.Relations).AddRange(new List<IRelation>() { tmp_Relation_A12, tmp_Relation_A13, tmp_Relation_A14, tmp_Relation_A15, tmp_Relation_A16, tmp_Relation_A23, tmp_Relation_A24, tmp_Relation_A25, tmp_Relation_A26, tmp_Relation_A34, tmp_Relation_A35, tmp_Relation_A36, tmp_Relation_A45, tmp_Relation_A46, tmp_Relation_A56 });
            tmp_CriteriaPerpective.SubPerspectives.Add(tmp_perspective_style_al);
            //Style -Alternative Judgment
            IComparison tmp_comparison_style_al = new Comparison(tmp_perspective_style_al, tmp_Judgment1);
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A12) { Ratio = 1f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A13) { Ratio = 7f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A14) { Ratio = 5f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A15) { Ratio = 9f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A16) { Ratio = 6f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A23) { Ratio = 7f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A24) { Ratio = 5f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A25) { Ratio = 9f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A26) { Ratio = 6f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A34) { Ratio = 1f/6f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A35) { Ratio = 3f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A36) { Ratio = 1f / 3f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A45) { Ratio = 7f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A46) { Ratio = 5f });
            tmp_comparison_style_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_style_al, tmp_Relation_A56) { Ratio = 1f / 5f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_style_al);

            //Cargo Capacity - Alternative perpective
            IComparisonPerspective tmp_perspective_cargoCapacity_al = new ComparisonPerspective(tmp_SubCriteria_CargoCapacity, tmp_perspective_Capacity_cr);
            ((List<IRelation>)tmp_perspective_cargoCapacity_al.Relations).AddRange(new List<IRelation>() { tmp_Relation_A12, tmp_Relation_A13, tmp_Relation_A14, tmp_Relation_A15, tmp_Relation_A16, tmp_Relation_A23, tmp_Relation_A24, tmp_Relation_A25, tmp_Relation_A26, tmp_Relation_A34, tmp_Relation_A35, tmp_Relation_A36, tmp_Relation_A45, tmp_Relation_A46, tmp_Relation_A56 });
            tmp_perspective_Capacity_cr.SubPerspectives.Add(tmp_perspective_cargoCapacity_al);
            //Cargo Capacity - Alternative Judgment
            IComparison tmp_comparison_cargoCapacity_al = new Comparison(tmp_perspective_cargoCapacity_al, tmp_Judgment1);
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A12) { Ratio = 1f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A13) { Ratio = 1f / 2f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A14) { Ratio = 1f / 2f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A15) { Ratio = 1f / 2f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A16) { Ratio = 1f / 3f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A23) { Ratio = 1f / 2f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A24) { Ratio = 1f / 2f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A25) { Ratio = 1f / 2f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A26) { Ratio = 1f / 3f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A34) { Ratio = 1f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A35) { Ratio = 1f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A36) { Ratio = 1f / 2f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A45) { Ratio = 1f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A46) { Ratio = 1f / 2f });
            tmp_comparison_cargoCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_cargoCapacity_al, tmp_Relation_A56) { Ratio = 1f / 2f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_cargoCapacity_al);

            //Passenger Capacity - Alternative perpective
            IComparisonPerspective tmp_perspective_passengerCapacity_al = new ComparisonPerspective(tmp_SubCriteria_PassengerCapacity, tmp_perspective_Capacity_cr);
            ((List<IRelation>)tmp_perspective_passengerCapacity_al.Relations).AddRange(new List<IRelation>() { tmp_Relation_A12, tmp_Relation_A13, tmp_Relation_A14, tmp_Relation_A15, tmp_Relation_A16, tmp_Relation_A23, tmp_Relation_A24, tmp_Relation_A25, tmp_Relation_A26, tmp_Relation_A34, tmp_Relation_A35, tmp_Relation_A36, tmp_Relation_A45, tmp_Relation_A46, tmp_Relation_A56 });
            tmp_perspective_Capacity_cr.SubPerspectives.Add(tmp_perspective_passengerCapacity_al);
            //Passenger Capacity- Alternative Judgment
            /*
             * ##             - [Accord Sedan, Accord Hybrid, 1]
                ##             - [Accord Sedan, Pilot, 1/2]
                ##             - [Accord Sedan, CR-V, 1]
                ##             - [Accord Sedan, Element, 3]
                ##             - [Accord Sedan, Odyssey, 1/2]
                ##             - [Accord Hybrid, Pilot, 1/2]
                ##             - [Accord Hybrid, CR-V, 1]
                ##             - [Accord Hybrid, Element, 3]
                ##             - [Accord Hybrid, Odyssey, 1/2]
                ##             - [Pilot, CR-V, 2]
                ##             - [Pilot, Element, 6]
                ##             - [Pilot, Odyssey, 1]
                ##             - [CR-V, Element, 3]
                ##             - [CR-V, Odyssey, 1/2]
                ##             - [Element, Odyssey, 1/6]
            */
            IComparison tmp_comparison_passengerCapacity_al = new Comparison(tmp_perspective_passengerCapacity_al, tmp_Judgment1);
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A12) { Ratio = 1f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A13) { Ratio = 1f / 2f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A14) { Ratio = 1f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A15) { Ratio = 3f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A16) { Ratio = 1f / 2f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A23) { Ratio = 1f / 2f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A24) { Ratio = 1f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A25) { Ratio = 3f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A26) { Ratio = 1f / 2f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A34) { Ratio = 2f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A35) { Ratio = 6f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A36) { Ratio = 1f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A45) { Ratio = 3f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A46) { Ratio = 1f / 2f });
            tmp_comparison_passengerCapacity_al.Pairwises.Add(new PairwiseComparison(tmp_comparison_passengerCapacity_al, tmp_Relation_A56) { Ratio = 1f / 6f });
            tmp_Judgment1.Comparisons.Add(tmp_comparison_passengerCapacity_al);

            System.Diagnostics.Debug.WriteLine(tmp_project.ToString());

            AhpSolver tmp_solver = new AhpSolver(tmp_project);
            Matrix resultMatrix = tmp_solver.Solve();

            Assert.IsNotNull(resultMatrix, "success");
        }
    }
}
