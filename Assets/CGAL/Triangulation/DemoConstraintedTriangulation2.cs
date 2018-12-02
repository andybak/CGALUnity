using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Triangulation.Constrainted;
using CGAL.Meshes.IndexBased;

namespace CGALDemo
{

    public class DemoConstraintedTriangulation2 : MonoBehaviour
    {

        Vector2f[] points;

        List<Vector2f[]> lines;

        Mesh2f mesh;

        void Start()
        {

            points = new Vector2f[100];
            FillWithRandom(points);

            lines = CreateLines();

            mesh = ConstraintedTriangulation2.Triangulate(lines, points);

        }

        private void OnPostRender()
        {

            Camera cam = Camera.current;
            if (cam == null) return;

            DrawLines.LineMode = LINE_MODE.TRIANGLES;
            DrawBase.Orientation = DRAW_ORIENTATION.XY;
            Matrix4x4f m = Matrix4x4f.Identity;

            DrawLines.Draw(cam, mesh.Positions, Color.green, m, mesh.Indices);
            DrawVertices.Draw(cam, 0.02f, mesh.Positions, Color.yellow, m);

        }

        private static void FillWithRandom(Vector2f[] points, int seed = 0)
        {

            Random.InitState(seed);
            float min = -0.9f;
            float max = 0.9f;

            for (int i = 0; i < points.Length; i++)
            {
                float x = Random.Range(min, max);
                float y = Random.Range(min, max);

                points[i] = new Vector2f(x, y);
            }

        }

        private static List<Vector2f[]> CreateLines()
        {
            Vector2f[] lineA = new Vector2f[]
            {
                new Vector2f(-1,-1), new Vector2f(-1,1)
            };

            Vector2f[] lineB = new Vector2f[]
            {
                new Vector2f(1,-1), new Vector2f(1,1)
            };

            var lines = new List<Vector2f[]>();
            lines.Add(lineA);
            lines.Add(lineB);

            return lines;
        }

    }

}
