using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Triangulation.ConvexHull;
using CGAL.Polygons;

namespace CGALDemo
{

    public class DemoConvexHull2 : MonoBehaviour
    {

        Vector2f[] points;

        Polygon2f convex;

        void Start()
        {
  
            points = new Vector2f[100];
            FillWithRandom(points);

            convex = ConvexHull2.FindHull(points);
            convex.BuildIndices();

        }

        private void OnPostRender()
        {

            Camera cam = Camera.current;
            if (cam == null) return;

            Matrix4x4f m = Matrix4x4f.Identity;

            DrawLines.LineMode = LINE_MODE.LINES;
            DrawLines.Draw(cam, convex.Positions, Color.red, m, convex.Indices);

            DrawVertices.Orientation = DRAW_ORIENTATION.XY;
            DrawVertices.Draw(cam, 0.02f, points, Color.yellow, m);

        }

        private static void FillWithRandom(Vector2f[] points, int seed = 0)
        {

            Random.InitState(seed);

            for(int i = 0; i < points.Length; i++)
            {
                float x = Random.Range(-1.0f, 1.0f);
                float y = Random.Range(-1.0f, 1.0f);

                points[i] = new Vector2f(x, y);
            }

        }

    }

}
