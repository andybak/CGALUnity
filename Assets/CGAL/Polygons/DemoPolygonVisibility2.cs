using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Polygons;

namespace CGALDemo
{

    public class DemoPolygonVisibility2 : Polygon2Input
    {

        Polygon2f polygon;

        List<Polygon2f> visibility = new List<Polygon2f>();

        protected override void OnPolygonComplete(Polygon2f input)
        {
            polygon = input;
            polygon.BuildIndices();
            polygon.BuildHoleIndices();
            visibility.Clear();
        }

        protected override void OnPolygonCleared()
        {
            polygon = null;
            visibility.Clear();
        }

        protected override void OnLeftClick(Vector2f point)
        {
            if (MadePolygon)
            {
                visibility.Clear();
                PolygonVisibility2.Compute(polygon, point, visibility);

                foreach (var polygon in visibility)
                {
                    polygon.BuildIndices();
                    polygon.BuildHoleIndices();
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

                DrawPolygon(cam, polygon, Color.green, Color.yellow);

                foreach(var polygon in visibility)
                    DrawPolygon(cam, polygon, Color.blue, Color.yellow);
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

                GUI.Label(new Rect(10, 120, textLen, textHeight), "Left click to compute visibility.");

            }

        }


    }
}
