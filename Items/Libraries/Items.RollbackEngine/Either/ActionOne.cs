﻿using System;
using Items.Common.Logging;

namespace Items.RollbackEngine.Either
{
    internal sealed class ActionOne : IRollbackAction<int, int>
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLoggerFor<ActionOne>();


        public ActionOne()
        {
        }

        #region IRollbackAction<int, int> Implementation

        public int Execute(int parameter)
        {
            if (parameter != 0)
            {
                throw new ArgumentException($"Expected 0 argument, got {parameter.ToString()}.", nameof(parameter));
            }

            Logger.Message("Action one done.");
            return 1;
        }

        public bool TryRollbackSafe()
        {
            Logger.Message("Rollback action one.");
            return true;
        }

        #endregion
    }
}
