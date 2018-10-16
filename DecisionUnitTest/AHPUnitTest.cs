using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;

namespace DecisionUnitTest
{
    [TestClass]
    public class AHPUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            IDecisionItem tmp_root = new DecisionItem(ItemType.Model, "test1");
            //Goal
            IDecisionItem tmp_ClusterGoal = tmp_root.Add(ItemType.Goal, "Goal");
            IDecisionItem tmp_GoalNode = tmp_ClusterGoal.Add(ItemType.Goal, "Satisfaction with School");
            //Criteria
            IDecisionItem tmp_ClusterCriteria = tmp_root.Add(ItemType.Criteria, "Criteria");
            IDecisionItem tmp_CriteriaLearning = tmp_ClusterCriteria.Add(ItemType.Criteria, "Learning");
            IDecisionItem tmp_CriteriaFriends = tmp_ClusterCriteria.Add(ItemType.Criteria, "Friends");
            IDecisionItem tmp_CriteriaSchoolLife = tmp_ClusterCriteria.Add(ItemType.Criteria, "School Life");
            IDecisionItem tmp_CriteriaVocationalTraining = tmp_ClusterCriteria.Add(ItemType.Criteria, "Vocational Training");
            IDecisionItem tmp_CriteriaCollegePrep = tmp_ClusterCriteria.Add(ItemType.Criteria, "College Prep");
            IDecisionItem tmp_CriteriaMusicClasses = tmp_ClusterCriteria.Add(ItemType.Criteria, "Music Classes");
            //Alternative
            IDecisionItem tmp_ClusterAlternative = tmp_root.Add(ItemType.Alternative, "Alternative");
            IDecisionItem tmp_AlternativeSchoolA = tmp_ClusterAlternative.Add(ItemType.Alternative, "School A");
            IDecisionItem tmp_AlternativeSchoolB = tmp_ClusterAlternative.Add(ItemType.Alternative, "School B");
            IDecisionItem tmp_AlternativeSchoolC = tmp_ClusterAlternative.Add(ItemType.Alternative, "School C");

            ((DecisionItem)tmp_root).Print();

            Assert.AreEqual(tmp_root.ItemType, ItemType.Model, "success");

        }
    }
}
