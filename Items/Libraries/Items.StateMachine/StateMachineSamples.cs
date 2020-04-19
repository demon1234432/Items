﻿using Items.Common;
using Items.Common.Logging;
using Items.StateMachine.V1.Common;
using Items.StateMachine.V1.States;

namespace Items.StateMachine
{
    public sealed class StateMachineSamples : ISamplesModule
    {
        private static readonly ILogger Logger =
            LoggerFactory.CreateLoggerFor<StateMachineSamples>();

        public string ModuleName { get; } = nameof(StateMachineSamples);


        public StateMachineSamples()
        {
        }

        #region ISamplesModule Implementation

        public SampleCollection ProvideSamples()
        {
            return new SampleCollection
            {
                { "StateMachine.SimpleStateMachine", RunSimpleStateMachineSample },
                { "StateMachine.UntilFinishEnumerator", RunStateMachineUntilFinishEnumeratorSample }
            };
        }

        #endregion

        public static void RunSimpleStateMachineSample()
        {
            var initialState = new State(42, 1337);
            var initialAction = new TaskC();

            State finalState = StateMachineHelper.PerformStraightforward(initialState, initialAction);

            Logger.SkipLine();
            var initialAction2 = new TaskA();

            State finalState2 = StateMachineHelper.PerformStraightforward(initialState, initialAction2);
        }

        public static void RunStateMachineUntilFinishEnumeratorSample()
        {
            var initialState = new State(42, 1337);
            var initialAction = new TaskC();

            Logger.Message($"Initial state: {initialState}");
            Logger.Message("Starting performing.");
            State finalState = initialAction
                .PerformUntilFinalState(initialState)
                .Execute();

            Logger.Message($"Final state: {finalState}");

            Logger.SkipLine();
            var initialAction2 = new TaskA();

            Logger.Message($"Initial state: {initialState}");
            Logger.Message("Starting performing.");
            State finalState2 = initialAction2
                .PerformUntilFinalState(initialState)
                .CatchExceptions()
                .Execute();

            Logger.Message($"Final state: {finalState2}");
        }
    }
}
