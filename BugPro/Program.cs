using System;
using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State { Created, InProgress, UnderReview, Postponed, Resolved, Reopened }
        private enum Trigger { StartWork, Postpone, Complete, BeginReview, RestoreWork }
        private StateMachine<State, Trigger> sm;

        public Bug(State state)
        {
            sm = new StateMachine<State, Trigger>(state);
            sm.Configure(State.Created)
                .Permit(Trigger.StartWork, State.InProgress);
            sm.Configure(State.InProgress)
                .Permit(Trigger.Complete, State.Resolved)
                .Permit(Trigger.Postpone, State.Postponed)
                .Permit(Trigger.BeginReview, State.UnderReview)
                .Ignore(Trigger.StartWork);
            sm.Configure(State.UnderReview)
                .Permit(Trigger.Complete, State.Resolved)
                .Permit(Trigger.RestoreWork, State.Reopened);
            sm.Configure(State.Resolved)
                .Permit(Trigger.StartWork, State.InProgress)
                .Permit(Trigger.RestoreWork, State.Reopened);
            sm.Configure(State.Postponed)
                .Permit(Trigger.StartWork, State.InProgress);
            sm.Configure(State.Reopened)
                .Permit(Trigger.StartWork, State.InProgress);
        }

        public void Complete()
        {
            sm.Fire(Trigger.Complete);
            Console.WriteLine("Complete");
        }

        public void StartWork()
        {
            sm.Fire(Trigger.StartWork);
            Console.WriteLine("StartWork");
        }

        public void Postpone()
        {
            sm.Fire(Trigger.Postpone);
            Console.WriteLine("Postpone");
        }

        public void BeginReview()
        {
            sm.Fire(Trigger.BeginReview);
            Console.WriteLine("BeginReview");
        }

        public void RestoreWork()
        {
            sm.Fire(Trigger.RestoreWork);
            Console.WriteLine("RestoreWork");
        }

        public State GetState()
        {
            return sm.State;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var bug = new Bug(Bug.State.Created);
            Console.WriteLine("Initial State: " + bug.GetState());
            bug.StartWork();
            Console.WriteLine("State: " + bug.GetState());
            bug.BeginReview();
            Console.WriteLine("State: " + bug.GetState());
            bug.Complete();
            Console.WriteLine("State: " + bug.GetState());
            bug.RestoreWork();
            Console.WriteLine("State: " + bug.GetState());
            bug.StartWork();
            Console.WriteLine("State: " + bug.GetState());
            bug.Postpone();
            Console.WriteLine("State: " + bug.GetState());
            bug.StartWork();
            Console.WriteLine("State: " + bug.GetState());
        }
    }
}