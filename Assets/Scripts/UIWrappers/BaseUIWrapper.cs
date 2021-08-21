using Orbox.Async;
using UnityEngine;
using Utils;

namespace UIWrappers
{
    public abstract class BaseUIWrapper : MonoBehaviour
    {
        public abstract IPromise Show();
        public abstract IPromise Hide();

        protected IPromise BaseScale(bool isShow)
        {
            var promise = new Deferred();

            transform.DoScale(isShow)
                .onComplete += () => promise.Resolve();

            return promise;
        }
    }
}
