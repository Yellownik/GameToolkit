using System;
using System.Collections.Generic;

namespace Orbox.Async
{

    public class ConditionTracker
    {
        private List<TrackedItem> Items = new List<TrackedItem>();

        // --- public ---

        public IPromise Track(Func<bool> condition)
        {
            var deferred = new Deferred();
            var item = new TrackedItem(deferred, condition);

            Items.Add(item);

            return deferred;
        }

        public void CheckAll()
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                var item = Items[i];

                if (item.Condition() == true)
                {
                    item.Deferred.Resolve();
                    Items.RemoveAt(i);
                }
            }
        }

        public void RejectAll()
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                var item = Items[i];

                item.Deferred.Reject();
                Items.RemoveAt(i);
            }
        }

        // --- private ---

        private class TrackedItem
        {
            public readonly Deferred Deferred;
            public readonly Func<bool> Condition;

            public TrackedItem(Deferred deferred, Func<bool> condition)
            {
                Deferred = deferred;
                Condition = condition;
            }
        }
    }

    /* Usage example 
    public class UnitGroup: MonoBehaviour
    {
        ConditionTracker ConditionTracker = new ConditionTracker();

        public IPromise WaitForDestinationReached(IPointMarker point)
        {
            var promise = ConditionTracker.Track(() => { return IsDestinationReached(point); });
            return promise;
        }

        // --- unity ---
        private void Update()
        {
            ConditionTracker.CheckAll();
        }

        private bool IsDestinationReached(IPointMarker marker)
        {
            ...
        }
    }*/
}