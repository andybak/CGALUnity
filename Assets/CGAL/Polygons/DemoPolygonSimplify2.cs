using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Polygons;

namespace CGALDemo
{

    public class DemoPolygonSimplify2 : Polygon2Input
    {

        Polygon2f polygon, simplified;

        protected override void OnPolygonComplete(Polygon2f input)
        {
            LineColor = Color.green;
            polygon = input;
            polygon.MakeCCW();
            polygon.BuildIndices();
            polygon.BuildHoleIndices();

            simplified = PolygonSimplify2.Simplify(polygon, 0.5f, SIMPLIFY_METHOD.SQUARE_DIST);
            simplified.BuildIndices();

        }

        protected override void OnPolygonCleared()
        {
            LineColor = Color.red;
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
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon is Simple = " + polygon.IsSimple);
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Polygon is Convex = " + polygon.IsConvex);
                GUI.Label(new Rect(10, 70, textLen, textHeight), "Polygon area = " + polygon.Area);
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Polygon Orientation = " + polygon.Orientation);
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

                Matrix4x4f m = Matrix4x4f.Identity;

                DrawLines.LineMode = LINE_MODE.LINES;
                DrawLines.Draw(cam, simplified.Positions, LineColor, m, simplified.Indices);

                DrawVertices.Orientation = DRAW_ORIENTATION.XY;
                DrawVertices.Draw(cam, 0.02f, simplified.Positions, Color.yellow, m);
            }

        }

    }
}
