using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WH.Saving
{
    [System.Serializable]
    public struct SerializableQuaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public SerializableQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static implicit operator Quaternion(SerializableQuaternion serQu)
        {
            return new Quaternion(serQu.X, serQu.Y, serQu.Z, serQu.W);
        }

        public static implicit operator SerializableQuaternion(Quaternion qu)
        {
            return new SerializableQuaternion(qu.x, qu.y, qu.z, qu.w);
        }
    }
}
