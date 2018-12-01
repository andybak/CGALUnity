using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using Common.Core.LinearAlgebra;
using Common.Core.Colors;
using Common.Unity.Extensions;

namespace Common.Unity.Drawing
{

    public enum DRAW_ORIENTATION { XY, XZ };

    public class DrawBase
    {
        public static DRAW_ORIENTATION Orientation = DRAW_ORIENTATION.XY;

        protected static List<Vector4> m_vertices = new List<Vector4>();
        protected static List<Color> m_colors = new List<Color>();

        public static IList<int> CUBE_INDICES = new int[]
        {
            0, 1, 1, 2, 2, 3, 3, 0,
            4, 5, 5, 6, 6, 7, 7, 4,
            0, 4, 1, 5, 2, 6, 3, 7
        };

        public static IList<int> SQUARE_INDICES = new int[]
        {
            0, 1, 1, 2, 2, 3, 3, 0
        };

        public CompareFunction ZTest
        {
            get { return (CompareFunction)Material.GetInt("_ZTest"); }
            set { Material.SetInt("_ZTest", (int)CompareFunction.Always); }
        }

        private static Material m_material;
        protected static Material Material
        {
            get
            {
                if (m_material != null)
                    return m_material;

                m_material = new Material(Shader.Find("Hidden/Internal-Colored"));

                return m_material;
            }
        }



    }

}
