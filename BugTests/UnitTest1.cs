using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInitialState()
        {
            var bug = new Bug(Bug.State.Created);
            Assert.AreEqual(Bug.State.Created, bug.GetState());
        }

        [TestMethod]
        public void TestStartWorkFromCreated()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void TestBeginReviewFromInProgress()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.BeginReview();
            Assert.AreEqual(Bug.State.UnderReview, bug.GetState());
        }

        [TestMethod]
        public void TestCompleteFromInProgress()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.Complete();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        [TestMethod]
        public void TestCompleteFromUnderReview()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.BeginReview();
            bug.Complete();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        [TestMethod]
        public void TestPostponeFromInProgress()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.Postpone();
            Assert.AreEqual(Bug.State.Postponed, bug.GetState());
        }

        [TestMethod]
        public void TestStartWorkFromPostponed()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.Postpone();
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void TestRestoreWorkFromResolved()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.Complete();
            bug.RestoreWork();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void TestStartWorkFromReopened()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.Complete();
            bug.RestoreWork();
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void TestRestoreWorkFromUnderReview()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.BeginReview();
            bug.RestoreWork();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidCompleteFromCreated()
        {
            var bug = new Bug(Bug.State.Created);
            bug.Complete();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidPostponeFromCreated()
        {
            var bug = new Bug(Bug.State.Created);
            bug.Postpone();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidBeginReviewFromCreated()
        {
            var bug = new Bug(Bug.State.Created);
            bug.BeginReview();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidRestoreWorkFromInProgress()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.RestoreWork();
        }

        [TestMethod]
        public void TestFullCycle_Created_StartWork_BeginReview_Complete_RestoreWork_StartWork_Postpone_StartWork()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
            bug.BeginReview();
            Assert.AreEqual(Bug.State.UnderReview, bug.GetState());
            bug.Complete();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
            bug.RestoreWork();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
            bug.Postpone();
            Assert.AreEqual(Bug.State.Postponed, bug.GetState());
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void TestIgnoreStartWorkWhenInProgress()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDoubleTransitionError()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.BeginReview();
            bug.BeginReview();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidTransitionFromResolved()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.Complete();
            bug.Postpone();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidTransitionFromPostponed()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.Postpone();
            bug.Complete();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidPostponeFromReopened()
        {
            var bug = new Bug(Bug.State.Created);
            bug.StartWork();
            bug.Complete();
            bug.RestoreWork();
            bug.Postpone();
        }
    }
}