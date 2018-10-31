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
    public class ANPUnitTest
    {
        [TestMethod]
        public void Test_SchoolSelection1()
        {
            IDecision tmp_project = TestFactory.SchoolSelectionProblem();
            System.Diagnostics.Debug.WriteLine(tmp_project.ToString());

            AnpSolver tmp_solver = new AnpSolver(tmp_project);
            IDecisionResult decisionResult = tmp_solver.Solve();

            Assert.IsNotNull(decisionResult, "success");

        }

        [TestMethod]
        public void Test_CarSelection1()
        {
            IDecision tmp_project = TestFactory.CarSelectionProblem();
            System.Diagnostics.Debug.WriteLine(tmp_project.ToString());

            AnpSolver tmp_solver = new AnpSolver(tmp_project);
            IDecisionResult decisionResult = tmp_solver.Solve();

            Assert.IsNotNull(decisionResult, "success");
        }

        [TestMethod]
        public void Test_CarSelection2()
        {
            //http://en.wikipedia.org/wiki/Analytic_hierarchy_process_%E2%80%94_Car_example
            //https://rpubs.com/gluc/ahp

            IDecision tmp_project = TestFactory.ComplexCarSelectionProblem();

            System.Diagnostics.Debug.WriteLine(tmp_project.ToString());

            AnpSolver tmp_solver = new AnpSolver(tmp_project);
            IDecisionResult decisionResult = tmp_solver.Solve();

            Assert.IsNotNull(decisionResult, "success");
        }
    }
}
