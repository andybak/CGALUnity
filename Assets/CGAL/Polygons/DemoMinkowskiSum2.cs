using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Polygons;

namespace CGALDemo
{

    public class DemoMinkowskiSum2 : Polygon2Input
    {

        Polygon2f polygon, sum, shape;

        private void Start()
        {
            float scale = 0.25f;
            //Vector2f a = new Vector2f(-1, -1) * scale;
            //Vector2f b = new Vector2f(1, -1) * scale;
            //Vector2f c = new Vector2f(0, 1) * scale;

            //shape = CreatePolygon2.FromTriangle(a, b, c);
            shape = CreatePolygon2.FromStar4(Vector2f.Zero, scale);
            shape.BuildIndices();
        }

        protected override void OnPolygonComplete(Polygon2f input)
        {
            LineColor = Color.green;
            polygon = input;

            polygon.MakeCCW();
            polygon.BuildIndices();
            polygon.BuildHoleIndices();

            if (polygon.IsSimple)
            {
                Polygon2f A = shape;
                Polygon2f B = polygon;

                sum = MinkowskiSums2.ComputeSum(A, B);
                sum.BuildIndices();
                sum.BuildHoleIndices();

            }
            else
            {
                sum = null;
            }

        }

        protected override void OnPolygonCleared()
        {
            LineColor = Color.red;
            sum = null;
        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;

            if (!MadePolygon)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");

                if (!polygon.IsSimple)
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must be simple. Clear and start again.");
                }
                else if (polygon.Orientation != ORIENTATION.COUNTERCLOCKWISE)
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must have ccw orientation. Clear and start again.");
                }

            }

        }

        protected override void OnPostRender()
        {

            if (!MadePolygon)
            {
                base.OnPostRender();
            }
            else
            {
                Camera cam = Camera.current;
                if (cam == null) return;

                Matrix4x4f m;

                foreach (var p in polygon.Positions)
                {
                    m = Matrix4x4f.Translate(p.xy0);
                    DrawPolygon(cam, shape, Color.red, m);
                }

                m = Matrix4x4f.Identity;

                DrawPolygon(cam, polygon, Color.green, m);
                DrawPolygon(cam, sum, Color.blue, m);
            }

        }

        private void DrawPolygon(Camera cam, Polygon2f polygon, Color col, Matrix4x4f m)
        {
            if (polygon == null) return;

            DrawLines.LineMode = LINE_MODE.LINES;
            DrawVertices.Orientation = DRAW_ORIENTATION.XY;

            DrawLines.Draw(cam, polygon.Positions, col, m, polygon.Indices);
            DrawVertices.Draw(cam, 0.02f, polygon.Positions, Color.yellow, m);

            if (!polygon.HasHoles) return;

            foreach (var hole in polygon.Holes)
            {
                DrawLines.Draw(cam, hole.Positions, Color.green, m, hole.Indices);
                DrawVertices.Draw(cam, 0.02f, hole.Positions, Color.yellow, m);
            }
        }

    }
}
