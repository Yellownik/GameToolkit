using System;
using UnityEngine;

namespace Orbox.Utils
{
    public class MonoProxy : MonoBehaviour
    {
        public Action OnMonoStart = () => { };
        public Action OnMonoUpdate = () => { };
        public Action OnMonoDestroy = () => { };

        private void Start()
        {
            OnMonoStart();
        }

        private void Update()
        {
            OnMonoUpdate();
        }

        private void OnDestroy()
        {
            OnMonoDestroy();
        }

    }
}