using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Colors;

namespace Common.Unity.Extensions
{

    public static class ColorExtension
    {

        public static Color ToColor(this ColorRGBA c)
        {
            return new Color(c.r, c.g, c.b, c.a);
        }

        public static Color ToColor(this ColorRGB c)
        {
            return new Color(c.r, c.g, c.b, 1.0f);
        }

    }

}
