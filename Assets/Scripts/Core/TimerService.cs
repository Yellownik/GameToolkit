using Orbox.Async;
using Orbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TimerService : MonoBehaviour
    {
        private ITimers Timers;

        private void Awake()
        {
            Timers = MonoExtensions.MakeComponent<Timers>(transform);
        }

        #region Public methods
        public IPromise WaitForCondition(Func<bool> condition)
        {
            IEnumerator WaitForConditionEnumerator(Func<bool> cond, Deferred deferred)
            {
                yield return new WaitUntil(cond);
                deferred.Resolve();
            }

            var promise = new Deferred();
            StartCoroutine(WaitForConditionEnumerator(condition, promise));

            return promise;
        }

        public IPromise Wait(float seconds)
        {
            return Timers.Wait(seconds);
        }
        #endregion
    }
}
