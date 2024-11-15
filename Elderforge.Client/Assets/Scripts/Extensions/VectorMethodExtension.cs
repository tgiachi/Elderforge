using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elderforge.Network.Serialization.Numerics;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class VectorMethodExtension
    {

        public static Vector3Int ToUnityVector3(this SerializableVector3Int vector)
        {
            return new Vector3Int(vector.X, vector.Y, vector.Z);
        }

        public static Vector3 ToUnityVector3(this SerializableVector3 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}
