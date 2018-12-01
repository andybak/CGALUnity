using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Polygons;

namespace CGALDemo
{

    public class DemoPolygon2 : Polygon2Input
    {

        private class IntersectionResult
        {
            public bool contains;
            public Vector2f point;
        }

        Polygon2f polygon;

        IntersectionResult result;

        protected override void OnPolygonComplete(Polygon2f input)
        {
            LineColor = Color.green;
            polygon = input;
            polygon.BuildIndices();
            polygon.BuildHoleIndices();
            result = null;
        }

        protected override void OnPolygonCleared()
        {
            LineColor = Color.red;
            result = null;
        }

        protected override void OnLeftClick(Vector2f point)
        {
            if (MadePolygon)
            {
                PolygonIntersection2.PushPolygon(polygon);

                result = new IntersectionResult();
                result.contains = PolygonIntersection2.ContainsPoint(point);
                result.point = point;

                PolygonIntersection2.PopPolygon();

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
                if (polygon == null) return;

                Matrix4x4f m = Matrix4x4f.Identity;

                DrawLines.LineMode = LINE_MODE.LINES;
                DrawVertices.Orientation = DRAW_ORIENTATION.XY;

                DrawLines.Draw(cam, polygon.Positions, Color.green, m, polygon.Indices);
                DrawVertices.Draw(cam, 0.02f, polygon.Positions, Color.yellow, m);

                if (!polygon.HasHoles) return;

                foreach (var hole in polygon.Holes)
                {
                    DrawLines.Draw(cam, hole.Positions, Color.green, m, hole.Indices);
                    DrawVertices.Draw(cam, 0.02f, hole.Positions, Color.yellow, m);
                }
            }

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

                GUI.Label(new Rect(10, 120, textLen, textHeight), "Left click to test intersection.");

                if(result != null)
                {
                    GUI.Label(new Rect(10, 140, textLen, textHeight), string.Format("Point {0},{1} is {2}.", 
                        result.point.x, result.point.y, result.contains));
                }
            }
            
        }


    }
}
