using System;

namespace Orbox.Async
{

    public interface IPromise
    {
        IPromise Clone();
        IPromise Done(Action callback);        
        IPromise Always(Action callback);

        IPromise Fail(Action callback);
        IPromise Fail(Action<RejectReason> callback);
        
        IPromise Then(Func<IPromise> next);
        IPromise<TNext> Then<TNext>(Func<IPromise<TNext>> next);
        
        IPromise Catch();
        IPromise Catch(Predicate<RejectReason> match, Func<RejectReason, IPromise> recover);
        IPromise AddDisposer(IDisposer disposer);
    }

    public interface IPromise<T> : IPromise
    {
        new IPromise<T> Clone();
        IPromise<T> Done(Action<T> callback);

        IPromise Then(Func<T, IPromise> next);
        IPromise<TNext> Then<TNext>(Func<T, IPromise<TNext>> next);

        IPromise<T> Catch(Predicate<RejectReason> match, Func<RejectReason, IPromise<T>> recover);
    }

}