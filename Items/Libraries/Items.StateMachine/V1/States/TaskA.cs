﻿using System;

namespace Items.StateMachine.V1.States
{
    public sealed class TaskA : IStatefulTask<State>
    {
        public bool IsFinal { get; } = false;


        public TaskA()
        {
        }

        #region IStatefullTask<State> Implementation

        IStatefulTask<State> IStatefulTask<State>.DoAction(State state)
        {
            if (state.A > 10)
            {
                state.A *= 10;
                state.B = 42;
            }

            throw new Exception("Something goes wrong.");
            //return new TaskC();
        }

        #endregion
    }
}
