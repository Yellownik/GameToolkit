using Orbox.Async;
using UnityEngine;
using Utils;

namespace UIWrappers
{
    public abstract class BaseUIWrapper : MonoBehaviour
    {
        public abstract IPromise Show();
        public abstract IPromise Hide();

        protected IPromise BaseScale(bool isShow, float duration = -1)
        {
            var promise = new Promise();

            transform.DoScale(isShow, duration)
                .onComplete += () => promise.Resolve();

            return promise;
        }
    }
}
