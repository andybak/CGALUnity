using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;

namespace Common.Unity.Extensions
{

    public static class VectorExtension
    {

        public static Vector4 ToVector4(this Vector4f v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }

        public static Vector3 ToVector3(this Vector3f v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector4 ToVector4(this Vector3f v)
        {
            return new Vector4(v.x, v.y, v.z, 1);
        }

        public static Vector2 ToVector2(this Vector3f v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 ToVector2(this Vector2f v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 ToVector3(this Vector2f v)
        {
            return new Vector3(v.x, v.y, 0);
        }

        public static Vector4 ToVector4(this Vector2f v)
        {
            return new Vector4(v.x, v.y, 0, 1);
        }

        public static Vector4 ToVector4(this Vector4d v)
        {
            return new Vector4((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        }

        public static Vector3 ToVector3(this Vector3d v)
        {
            return new Vector3((float)v.x, (float)v.y, (float)v.z);
        }

        public static Vector4 ToVector4(this Vector3d v)
        {
            return new Vector4((float)v.x, (float)v.y, (float)v.z, 1);
        }

        public static Vector2 ToVector2(this Vector2d v)
        {
            return new Vector2((float)v.x, (float)v.y);
        }

        public static Vector3 ToVector3(this Vector2d v)
        {
            return new Vector3((float)v.x, (float)v.y, 0);
        }

        public static Vector4 ToVector4(this Vector2d v)
        {
            return new Vector4((float)v.x, (float)v.y, 0, 1);
        }

        public static Vector3f ToVector3f(this Vector3 v)
        {
            return new Vector3f(v.x, v.y, v.z);
        }

        public static Vector2f ToVector2f(this Vector3 v)
        {
            return new Vector2f(v.x, v.y);
        }

        public static Vector2i ToVector2i(this Vector3 v)
        {
            return new Vector2i((int)v.x, (int)v.y);
        }

        public static Vector4f ToVector4f(this Vector3 v)
        {
            return new Vector4f(v.x, v.y, v.z, 1);
        }

        public static Vector2f ToVector2f(this Vector2 v)
        {
            return new Vector2f(v.x, v.y);
        }

        public static Vector4 ToVector4(this Vector4i v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }

        public static Vector3 ToVector3(this Vector3i v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector3 ToVector3(this Vector2i v)
        {
            return new Vector3(v.x, v.y, 0);
        }

        public static Vector2 ToVector2(this Vector2i v)
        {
            return new Vector2(v.x, v.y);
        }

    }

}
