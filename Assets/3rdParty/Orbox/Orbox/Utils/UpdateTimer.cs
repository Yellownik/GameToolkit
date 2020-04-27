using System;
using UnityEngine;

namespace Orbox.Utils
{
    public enum TimerType { NOOP, Regular, Once }

    public class UpdateTimer
    {
        public TimerType Type;

        private float Interval;
        private float ElapsedTime;

        private Action NOOP = () => { };
        private Action Action = ()=> { };

        private int InitializeFrameCount;
               

        private class MonoTimer: MonoBehaviour
        {
            public Action UpdateAction = ()=> { };

            public void Update()
            {
                UpdateAction();
            }
        }

        public UpdateTimer(GameObject host)
        {
            var monoTimer = host.AddComponent<MonoTimer>();
            monoTimer.UpdateAction = DoUpdate;
        }


        public void SetNOOP()
        {
            Type = TimerType.NOOP;
            Action = NOOP;
        }

        public void SetOnce(float interval, Action action)
        {
            Type = TimerType.Once;

            InitializeFrameCount = Time.frameCount;
            Interval = interval;
            Action = action;
            ElapsedTime = 0;
        }

        public void SetRegular(float interval, Action action)
        {
            Type = TimerType.Regular;

            InitializeFrameCount = Time.frameCount;
            Interval = interval;
            Action = action;
            ElapsedTime = 0;
        }

        //--- Private ---
        private void DoUpdate()
        {
            switch (Type)
            {
                case TimerType.NOOP: break;
                case TimerType.Once: UpdateOnce(); break;
                case TimerType.Regular: UpdateRegular(); break;
            }

        }

        private void UpdateRegular()
        {
            if (InitializeFrameCount == Time.frameCount)
                return;

            ElapsedTime += Time.deltaTime;

            if (ElapsedTime > Interval)
            {
                ElapsedTime = 0;
                Action();
            }
        }

        private void UpdateOnce()
        {
            if (InitializeFrameCount == Time.frameCount)
                return;

            ElapsedTime += Time.deltaTime;

            if (ElapsedTime > Interval)
            {
                Type = TimerType.NOOP;
                ElapsedTime = 0;         
                Action();                
            }
        }

    }
}
