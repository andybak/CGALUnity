using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Geometry.Shapes;

namespace Common.Unity.Extensions
{

    public static class BoundsExtension 
    {

        public static Box2f ToBox2f(this Bounds bounds)
        {
            return new Box2f(bounds.min.ToVector2f(), bounds.max.ToVector2f());
        }

        public static Box2i ToBox2i(this Bounds bounds)
        {
            return new Box2i(bounds.min.ToVector2i(), bounds.max.ToVector2i());
        }

        public static Box3f ToBox3f(this Bounds bounds)
        {
            return new Box3f(bounds.min.ToVector3f(), bounds.max.ToVector3f());
        }

    }

}
