//using RSG;
using Orbox.Async;

namespace Utils
{
    public static class PromiseUtils
    {
        //public static void RecreateAndResolve(ref Promise promise)
        //{
        //    var oldPromise = promise;
        //    promise = new Promise();
        //    oldPromise.Resolve();
        //}

        //public static void RecreateAndResolve<T>(ref Promise<T> promise, T t)
        //{
        //    var oldPromise = promise;
        //    promise = new Promise<T>();
        //    oldPromise.Resolve(t);
        //}

        public static void RecreateAndResolve(ref Deferred promise)
        {
            var oldPromise = promise;
            promise = new Deferred();
            oldPromise.Resolve();
        }

        public static void RecreateAndResolve<T>(ref Deferred<T> promise, T t)
        {
            var oldPromise = promise;
            promise = new Deferred<T>();
            oldPromise.Resolve(t);
        }
    }
}
