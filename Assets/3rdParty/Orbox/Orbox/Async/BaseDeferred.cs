using System;
using System.Collections.Generic;

namespace Orbox.Async
{

    public abstract class BaseDeferred: IPromise
    {
        protected enum Status
        {
            Pending,
            Resolved,
            Rejected
        }

        protected Status State;
        protected IDisposer Disposer;
        protected RejectReason RejectReason;

        protected List<Action> DoneCallbacks = new List<Action>();
        protected List<Action> FailCallbacks = new List<Action>();


        public BaseDeferred()
        {
            State = Status.Pending;
            RejectReason = new RejectReason();
        }

        public IPromise Clone()
        {
            var clone = new Promise();

            AssignDisposer(clone, Disposer);

            Done(() => clone.Resolve());
            Fail(() => clone.Reject());

            return clone;
        }

        public IPromise Reject()
        {
            if (State != Status.Pending)
                throw new InvalidOperationException();

            State = Status.Rejected;

            for (int i = 0; i < FailCallbacks.Count; i++)
            {
                var fail = FailCallbacks[i];
                fail();
            }

            ClearCallbacks();

            return this;
        }

        public IPromise Reject(RejectReason reason)
        {
            RejectReason = reason;
            Reject();

            return this;
        }

        public IPromise Done(Action callback)
        {
            if (State == Status.Resolved)
                callback();

            if (State == Status.Pending)
                DoneCallbacks.Add(callback);
                          
            return this;
        }

        public IPromise Fail(Action callback)
        {
            if (State == Status.Rejected)
                callback();

            if (State == Status.Pending)
                FailCallbacks.Add(callback);

            return this;
        }

        public IPromise Fail(Action<RejectReason> callback)
        {
            if (State == Status.Rejected)
                callback(RejectReason);

            if (State == Status.Pending)
                FailCallbacks.Add(()=> callback(RejectReason));

            return this;
        }

        public IPromise Catch()
        {
            var deferred = new Promise();

            AssignDisposer(deferred, Disposer);

            Done(() => deferred.Resolve());
            Fail(() => deferred.Resolve());
            
            return deferred;
        }

        public IPromise Catch(Predicate<RejectReason> match, Func<RejectReason, IPromise> recover)
        {
            var deferred = new Promise();

            AssignDisposer(deferred, Disposer);

            Done(() =>
            {
                deferred.Resolve();
            });

            Fail(error =>
            {
                if (match(error))
                {
                    var promise = recover(error);

                    AssignDisposer((BaseDeferred)promise, Disposer);

                    promise.Done(() => deferred.Resolve());
                    promise.Fail(e => deferred.Reject(e));
                }
                else
                {
                    deferred.Reject(error);
                }
            });

            return deferred;
        }


        public IPromise Always(Action callback)
        {            
            if (State == Status.Rejected || State == Status.Resolved)
                callback();

            if (State == Status.Pending)
            {
                DoneCallbacks.Add(callback);
                FailCallbacks.Add(callback);
            }
                
            return this;
        }

        public IPromise Then(Func<IPromise> next)
        {
            var deferred = new Promise();

            AssignDisposer(deferred, Disposer);

            Done(() =>
            {
                var promise = next();

                AssignDisposer((BaseDeferred)promise, Disposer);

                promise.Done(() => deferred.Resolve());
                promise.Fail(() => deferred.Reject());

            });

            Fail(() => deferred.Reject());

            return deferred;
        }

        public IPromise<TNext> Then<TNext>(Func<IPromise<TNext>> next)
        {
            var deferred = new Promise<TNext>();

            AssignDisposer(deferred, Disposer);

            Done(() =>
            {
                var promise = next();

                AssignDisposer((BaseDeferred)promise, Disposer);

                promise.Done(res => deferred.Resolve(res));
                promise.Fail(() => deferred.Reject());
            });

            Fail(() => deferred.Reject());

            return deferred;
        }

        public IPromise AddDisposer(IDisposer disposer)
        {
            var deferred = new Promise();

            AssignDisposer(deferred, disposer);

            Done(() => deferred.Resolve());
            Fail(() => deferred.Reject());            

            return deferred;
        }

        // --- protected ---

        protected IPromise Resolve()
        {
            if (State != Status.Pending)
                throw new InvalidOperationException();

            State = Status.Resolved;

            for (int i = 0; i < DoneCallbacks.Count; i++)
            {
                var done = DoneCallbacks[i];
                done();
            }

            ClearCallbacks();

            return this;
        }

        protected static void AssignDisposer(BaseDeferred deferred, IDisposer disposer)
        {
            if (disposer != null)
            {
                deferred.Disposer = disposer;
                deferred.Disposer.Add(deferred.ClearCallbacks);
            }
        }

        protected virtual void ClearCallbacks()
        {
            DoneCallbacks.Clear();
            FailCallbacks.Clear();
        }

        protected static void Reset(BaseDeferred baseDeferred)
        {
            baseDeferred.ClearCallbacks();
        }

        protected static bool IsResolved(BaseDeferred baseDeferred)
        {
            return baseDeferred.State == Status.Resolved;
        }

        protected static bool IsPending(BaseDeferred baseDeferred)
        {
            return baseDeferred.State == Status.Pending;

        }

        protected static bool IsRejected(BaseDeferred baseDeferred)
        {
            return baseDeferred.State == Status.Rejected;
        }


    }



}

