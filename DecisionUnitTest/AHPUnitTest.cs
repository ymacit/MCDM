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
            IDecision tmp_project = TestFactory.SchoolSelectionProblem();
            System.Diagnostics.Debug.WriteLine(tmp_project.ToString());

            AhpSolver tmp_solver = new AhpSolver(tmp_project);
            IDecisionResult decisionResult = tmp_solver.Solve();

            Assert.IsNotNull(decisionResult, "success");

        }

        [TestMethod]
        public void Test_CarSelection1()
        {
            IDecision tmp_project = TestFactory.CarSelectionProblem();
            System.Diagnostics.Debug.WriteLine(tmp_project.ToString());

            AhpSolver tmp_solver = new AhpSolver(tmp_project);
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

            AhpSolver tmp_solver = new AhpSolver(tmp_project);
            IDecisionResult decisionResult = tmp_solver.Solve();

            Assert.IsNotNull(decisionResult, "success");
        }
    }
}

