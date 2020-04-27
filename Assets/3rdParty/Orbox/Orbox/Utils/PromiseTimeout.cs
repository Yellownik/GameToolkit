using Orbox.Async;


namespace Orbox.Utils
{
    // Usage as extension example
    //
    //public static IPromise AddTimeout(this IPromise promise, float timeout)
    //{
    //    var timers = CompositionRoot.GetTimers();
    //    var promiseWithTimeout = PromiseTimeout.AddTimeout(promise, timers, timeout);
    //
    //    return promiseWithTimeout;
    //}

    public static class PromiseTimeout
    {
        protected enum EStatus
        {
            Pending,
            Resolved,
            Rejected
        }

        public static IPromise AddTimeout(IPromise promise, ITimers timers, float timeout)
        {
            var result = new Deferred();
            var status = EStatus.Pending;

            var timeoutPromise = timers.Wait(timeout);

            promise.Done(() =>
            {
                if (status == EStatus.Pending)
                {
                    status = EStatus.Resolved;
                    result.Resolve();
                }
            });

            promise.Fail(() =>
            {
                if (status == EStatus.Pending)
                {
                    status = EStatus.Rejected;
                    result.Reject();
                }
            });

            timeoutPromise.Done(() =>
            {
                if (status == EStatus.Pending)
                {
                    status = EStatus.Rejected;
                    result.Reject();
                }
            });

            return result;
        }

        public static IPromise<T> AddTimeout<T>(IPromise<T> promise, ITimers timers, float timeout)
        {
            var result = new Deferred<T>();
            var status = EStatus.Pending;

            var timeoutPromise = timers.Wait(timeout);

            promise.Done(r =>
            {
                if (status == EStatus.Pending)
                {
                    status = EStatus.Resolved;
                    result.Resolve(r);
                }
            });

            promise.Fail(() =>
            {
                if (status == EStatus.Pending)
                {
                    status = EStatus.Rejected;
                    result.Reject();
                }
            });

            timeoutPromise.Done(() =>
            {
                if (status == EStatus.Pending)
                {
                    status = EStatus.Rejected;
                    result.Reject();
                }
            });

            return result;
        }

    }
}