﻿using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using Acolyte.Exceptions;

namespace Items.RollbackEngine.Either
{
    public static class RollbackActionExtensions
    {
        public static bool TryRollbackSafe(this IEnumerable<IRollbackAction> rollbackActions)
        {
            rollbackActions.ThrowIfNull(nameof(rollbackActions));

            using var rollbackScope = new RollbackScope(rollbackActions);
            return rollbackScope.TryRollbackSafe();
        }

        public static IRollbackActionResult<TOut> Bind<TIn, TOut>(
            this IRollbackAction<TIn, TOut> action, TIn parameter)
        {
            return Bind(
                action, new SuccessfulResult<TIn>(parameter, Enumerable.Empty<IRollbackAction>())
            );
        }

        public static IRollbackActionResult<TOut> Bind<TIn, TOut>(
            this IRollbackAction<TIn, TOut> action, IRollbackActionResult<TIn> parameter)
        {
            action.ThrowIfNull(nameof(action));
            parameter.ThrowIfNull(nameof(parameter));

            switch (parameter)
            {
                case FailedResult failedResult:
                {
                    throw failedResult.Exception.Rethrow();
                }

                case SuccessfulResult<TIn> successfulResult:
                {
                    try
                    {

                        TOut result = action.Execute(successfulResult.Result);
                        return new SuccessfulResult<TOut>(
                            result, successfulResult.RollbackList.Prepend(action)
                        );
                    }
                    catch (Exception)
                    {
                        successfulResult.RollbackList.TryRollbackSafe();
                        throw;
                    }
                }

                default:
                {
                    throw new ArgumentException("Invalid parameter.", nameof(parameter));
                }
            }
        }

        public static IRollbackActionResult<TOut> Bind<TIn, TOut>(
            this IRollbackActionResult<TIn> parameter, IRollbackAction<TIn, TOut> action)
        {
            return Bind(action, parameter);
        }
    }
}
