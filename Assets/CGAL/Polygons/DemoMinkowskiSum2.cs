using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
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
            polygon = input;

            polygon.MakeCCW();
            polygon.BuildIndices();
            polygon.BuildHoleIndices();

            if (polygon.IsSimple && !polygon.HasHoles)
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

                if (!polygon.IsSimple || polygon.HasHoles)
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must be simple with no holes. Clear and start again.");
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

                foreach (var p in polygon.Positions)
                {
                    Matrix4x4f m = Matrix4x4f.Translate(p.xy0);
                    DrawPolygon(cam, shape, Color.red, Color.yellow, m);
                }

                DrawPolygon(cam, polygon, Color.green, Color.yellow);
                DrawPolygon(cam, sum, Color.blue, Color.yellow);
            }

        }
    }
}
