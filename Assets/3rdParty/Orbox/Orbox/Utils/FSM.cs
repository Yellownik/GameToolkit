
using Orbox.Async;
using System;
using System.Collections.Generic;

namespace Orbox.Utils
{
    public class FSM<TEnum> : IFSM<TEnum> where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        private TEnum Current;
        private Dictionary<TransitionKey, Func<IPromise>> Transitions;

        private bool TransitionInProgress;

        public FSM(TEnum initial)
        {
            Current = initial;
            Transitions = new Dictionary<TransitionKey, Func<IPromise>>(new TransitionEqualityComparer());
        }

        public void AddTransition(TEnum from, TEnum to, Func<IPromise> transiton)
        {
            var key = GetKey(from, to);

            Transitions.Add(key, transiton);
        }

        public IPromise SetState(TEnum next)
        {
            if (TransitionInProgress)
                throw new InvalidOperationException("Transition is in progress");


            var key = GetKey(Current, next);

            // return already resolved promise when transition is absent
            if(Transitions.ContainsKey(key) == false)
            {
                return new Deferred().Resolve();
            }


            TransitionInProgress = true;
            var transition = Transitions[key];


            var complete = transition().Done(() =>
            {
                Current = next;
                TransitionInProgress = false;
            });

            return complete;
        }


        //private data classes

        private struct TransitionKey
        {
            public readonly int From;
            public readonly int To;

            public TransitionKey(int from, int to)
            {
                From = from;
                To = to;
            }
        }

        private class TransitionEqualityComparer : IEqualityComparer<TransitionKey>
        {
            public bool Equals(TransitionKey x, TransitionKey y)
            {
                return x.From == y.From && x.To == y.To;
            }

            public int GetHashCode(TransitionKey key)
            {
                return key.From.GetHashCode() ^ key.To.GetHashCode();
            }
        }

        // private methods

        private TransitionKey GetKey(TEnum from, TEnum to)
        {
            int ifrom = EnumInt32ToInt.Convert(from);
            int ito = EnumInt32ToInt.Convert(to);

            var key = new TransitionKey(ifrom, ito);

            return key;
        }
    }
}