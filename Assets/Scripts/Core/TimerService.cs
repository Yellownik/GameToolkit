﻿using Orbox.Async;
using Orbox.Signals;
using Orbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface ITimerService : IEventPublisher
    {
        void SubscribeForUpdate(Action action);
        void SubscribeForHalfSecond(Action action);

        IPromise Wait(float seconds);
        IPromise WaitForCondition(Func<bool> condition);
    }

    public class TimerService : MonoBehaviour, ITimerService
    {
        private readonly List<Action> UpdateSubscribers = new List<Action>();
        private readonly List<Action> HalfSecondSubscribers = new List<Action>();

        private readonly WaitForSeconds HalfSecondWaiter = new WaitForSeconds(0.5f);

        private ITimers Timers;

        private void Awake()
        {
            Timers = MonoExtensions.MakeComponent<Timers>(transform);
            StartCoroutine(HalfSecondCoroutine());
        }

        private void Update()
        {
            foreach (var sub in UpdateSubscribers)
                sub.Invoke();
        }

        private IEnumerator HalfSecondCoroutine()
        {
            while (true)
            {
                foreach (var sub in HalfSecondSubscribers)
                    sub.Invoke();

                yield return HalfSecondWaiter;
            }
        }

        #region Public methods
        void IEventPublisher.Add(IEventSubscriber subscriber)
        {
            if (subscriber is IUpdatable updatable)
                UpdateSubscribers.Add(updatable.Update);
        }

        public void SubscribeForUpdate(Action action)
        {
            UpdateSubscribers.Add(action);
        }

        public void SubscribeForHalfSecond(Action action)
        {
            HalfSecondSubscribers.Add(action);
        }

        public IPromise Wait(float seconds)
        {
            return Timers.Wait(seconds);
        }

        public IPromise WaitForCondition(Func<bool> condition)
        {
            IEnumerator WaitForConditionEnumerator(Func<bool> cond, Promise prom)
            {
                yield return new WaitUntil(cond);
                prom.Resolve();
            }

            var promise = new Promise();
            StartCoroutine(WaitForConditionEnumerator(condition, promise));

            return promise;
        }
        #endregion
    }
}
