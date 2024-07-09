using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MotherStar.Platform.Application.SEO.Lighthouse.Jobs
{
    /// <summary>
    /// Use this helper class to manage hangfire background tasks inside of transaction scope. This should run fine inside of RCommon.DataServices.Transactions.UnitOfWorkManager
    /// </summary>
    /// <remarks>Dependency note: 12/15/2021 HttpContext will not be available during execution of tasks in Hangfire. Dependencies that resolve as web applications when tasks is created (such as RCommon.EnvironmentAccessor) will resolve as web applications when task is fired. Need to use another container: https://docs.hangfire.io/en/latest/background-methods/using-ioc-containers.html 
    /// Transaction note: 2/14/2017 running in transaction produces an exception: https://discuss.hangfire.io/t/msdtc-error-on-hangfire-1-5-3-and-1-4-6/1553 </remarks>
    public static class BackgroundJobHelper
    {
        public static string Enqueue(Expression<Func<Task>> methodCall)
        {
            string jobIdCreated;
            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                jobIdCreated = BackgroundJob.Enqueue(methodCall);
                ts.Complete();
            }

            return jobIdCreated;
        }



        public static string Enqueue(Expression<Action> methodCall)
        {
            string jobIdCreated;
            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                jobIdCreated = BackgroundJob.Enqueue(methodCall);
                ts.Complete();
            }

            return jobIdCreated;
        }



        public static string Enqueue<T>(Expression<Func<T, Task>> methodCall)
        {
            string jobIdCreated;

            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                jobIdCreated = BackgroundJob.Enqueue(methodCall);
                ts.Complete();
            }

            return jobIdCreated;
        }



        public static string Enqueue<T>(Expression<Action<T>> methodCall)
        {
            string jobIdCreated;
            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                jobIdCreated = BackgroundJob.Enqueue(methodCall);
                ts.Complete();
            }

            return jobIdCreated;
        }



        public static bool Delete(string jobId, List<string> fromStates = null)
        {
            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                var jobDeleted = false;

                if (fromStates != null)
                {
                    foreach (var fromState in fromStates)
                    {
                        jobDeleted = BackgroundJob.Delete(jobId, fromState);
                        if (jobDeleted)
                            break;
                    }
                }
                else
                {
                    jobDeleted = BackgroundJob.Delete(jobId);
                }
                ts.Complete();
                return jobDeleted;
            }
        }



        public static List<string> Delete(List<string> jobIds, List<string> fromStates = null)
        {
            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                var deletedJobIds = new List<string>();

                foreach (var jobId in jobIds)
                {
                    var jobDeleted = false;

                    if (fromStates != null)
                    {
                        foreach (var fromState in fromStates)
                        {
                            jobDeleted = BackgroundJob.Delete(jobId, fromState);
                            if (jobDeleted)
                                break;
                        }
                    }
                    else
                    {
                        jobDeleted = BackgroundJob.Delete(jobId);
                    }

                    if (jobDeleted)
                    {
                        deletedJobIds.Add(jobId);
                    }
                }

                ts.Complete();
                return deletedJobIds;
            }
        }



        public static string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt)
        {
            string jobIdCreated;

            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                jobIdCreated = BackgroundJob.Schedule(methodCall, enqueueAt);
                ts.Complete();
            }

            return jobIdCreated;
        }



        public static string Schedule(Expression<Action> methodCall, TimeSpan delay)
            => Schedule(methodCall, DateTimeOffset.UtcNow + delay);



        internal static string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan taskDelay)
        {
            string jobIdCreated;

            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                jobIdCreated = BackgroundJob.Schedule(methodCall, DateTimeOffset.UtcNow + taskDelay);
                ts.Complete();
            }

            return jobIdCreated;
        }



        public static string ContinueWith(string parentId, Expression<Func<Task>> methodCall)
        {
            string jobIdCreated;

            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                jobIdCreated = BackgroundJob.ContinueJobWith(parentId, methodCall);
                ts.Complete();
            }

            return jobIdCreated;
        }



        public static string ContinueWithIfParentIdSet(string parentId, Expression<Func<Task>> methodCall, JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            string jobIdCreated;

            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (string.IsNullOrWhiteSpace(parentId))
                {
                    jobIdCreated = BackgroundJob.Enqueue(methodCall);
                }
                else
                {
                    jobIdCreated = BackgroundJob.ContinueJobWith(parentId, methodCall, options);
                }

                ts.Complete();
            }

            return jobIdCreated;
        }



        public static string ContinueWithIfParentIdSet(string parentId, Expression<Action> methodCall, JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            string jobIdCreated;

            using (var ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (string.IsNullOrWhiteSpace(parentId))
                {
                    jobIdCreated = BackgroundJob.Enqueue(methodCall);
                }
                else
                {
                    jobIdCreated = BackgroundJob.ContinueJobWith(parentId, methodCall, options);
                }

                ts.Complete();
            }



            return jobIdCreated;
        }
    }
}
