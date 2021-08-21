using System;
using System.Linq;

namespace Orbox.Async
{
    public class Promise : BaseDeferred, IPromise
    {

        public new IPromise Resolve()
        {
            return base.Resolve();
        }

        public static IPromise All(params IPromise[] collection)
        {
            var deferred = new Promise();
            var promises = Array.ConvertAll(collection, promice => promice as BaseDeferred);
           
            if (collection.Length == 0)
                deferred.Resolve();

            for(int i = 0; i < promises.Length; i++)
            {
                var promise = promises[i];
                promise.Done(() =>
                {
                    if (deferred.State == Status.Pending && promises.All(p => IsResolved(p)))
                        deferred.Resolve();
                });

                promise.Fail(() =>
                {
                    if(deferred.State == Status.Pending)
                        deferred.Reject();
                });
            }

            return deferred;

        }

        //First will be Resolved, others well be Reseted
        public static IPromise Race(params IPromise[] collection)
        {
            var race = new Promise();
            var promises = Array.ConvertAll(collection, promice => promice as BaseDeferred);

            BaseDeferred last = null;

            for(int i = 0; i < promises.Length; i++)
            {
                var promise = promises[i];

                last = promise;
                var self = promise;

                if (IsResolved(last))
                    break;

                promise.Done(() =>
                {
                    for(int j = 0; j < promises.Length; j++ )
                    {
                        var item = promises[j];

                        if (IsPending(item) && item != self) // TODO: item != self is always true because done
                            Reset(item);
                    }

                    race.Resolve();
                });

                promise.Fail(() =>
                {
                    if (promises.All(p => IsRejected(p)))
                        race.Reject();
                });
            }

            if (IsResolved(last))
            {
                for(int i = 0; i < promises.Length; i++)
                {
                    var item = promises[i];

                    if (IsPending(item) && item != last)
                        Reset(item);
                }

                race.Resolve();
            }

            return race;
        }

    }
   
    public class Promise<T> : BaseDeferred ,  IPromise<T>
    {
        protected T Result;

        IPromise<T> IPromise<T>.Clone()
        {
            var clone = new Promise<T>();

            AssignDisposer(clone, Disposer);

            Done(result => clone.Resolve(result));
            Fail(() => clone.Reject());

            return clone;
        }

        public IPromise<T> Resolve(T result)
        {
            Result = result;
            Resolve();

            return this;
        }

        public new IPromise<T> Reject()
        {
            base.Reject();

            return this;
        }

        public new IPromise<T> Reject(RejectReason reason)
        {
            base.Reject(reason);

            return this;
        }

        public IPromise<T> Done(Action<T> callback)
        {            
            if (State == Status.Resolved)
                callback(Result);

            if (State == Status.Pending)
                DoneCallbacks.Add(() => callback(Result));
                
            return this;
        }

        public IPromise Then(Func<T, IPromise> next)
        {
            var deferred = new Promise();

            AssignDisposer(deferred, Disposer);

            Done(result =>
            {
                var promise = next(result);

                AssignDisposer((BaseDeferred)promise, Disposer);

                promise.Done(() => deferred.Resolve());
                promise.Fail(() => deferred.Reject());

            });

            Fail(() => deferred.Reject());

            return deferred;
        }

        public IPromise<TNext> Then<TNext>(Func<T,IPromise<TNext>> next)
        {
            var deferred = new Promise<TNext>();

            AssignDisposer(deferred, Disposer);

            Done(result =>
            {
                var promise = next(result);

                AssignDisposer((BaseDeferred)promise, Disposer);

                promise.Done(r => deferred.Resolve(r));
                promise.Fail(() => deferred.Reject());

            });

            Fail(() => deferred.Reject());

            return deferred;
        }

        public IPromise<T> Catch(Predicate<RejectReason> condition, Func<RejectReason, IPromise<T>> next)
        {
            var deferred = new Promise<T>();
            
            AssignDisposer(deferred, Disposer);

            Done(result =>
            {
                deferred.Resolve(result);
            });

            Fail(error =>
            {
                if (condition(RejectReason))
                {
                    var promise = next(RejectReason);
                    
                    AssignDisposer((BaseDeferred)promise, Disposer);                    

                    promise.Done(r => deferred.Resolve(r));
                    promise.Fail(e => deferred.Reject(e));                    
                }
                else
                {
                    deferred.Reject(error);
                }
            });

            return deferred;
        }


    }

}

