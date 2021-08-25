using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [System.Serializable]
    public struct SerializableVector3
    {
        public float X;
        public float Y;
        public float Z;

        public SerializableVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator Vector3(SerializableVector3 serV3)
        {
            return new Vector3(serV3.X, serV3.Y, serV3.Z);
        }

        public static implicit operator SerializableVector3(Vector3 v3)
        {
            return new SerializableVector3(v3.x, v3.y, v3.z);
        }
    }
}
