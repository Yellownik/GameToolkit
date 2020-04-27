using UnityEngine;
using System.Collections;

namespace Orbox.Utils
{
    public struct Awaker 
    {
        private bool FIsAwaked;
        public bool IsAwaked
        {
            get
            {
                var awaked = FIsAwaked;
                FIsAwaked = true;

                return awaked;
            }
        }

    }
}