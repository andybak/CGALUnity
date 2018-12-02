using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Polygons;

namespace CGALDemo
{

    public class DemoPolygonPartition2 : Polygon2Input
    {

        List<Polygon2f> partition;

        Polygon2f polygon;

        private void Start()
        {
            SnapPoint = 10.0f;
        }

        protected override void OnPolygonComplete(Polygon2f input)
        {
            polygon = input;

            polygon.MakeCCW();
            polygon.BuildIndices();
            polygon.BuildHoleIndices();

            if (polygon.IsSimple && !polygon.HasHoles)
            {
                partition = PolygonPartition2.Partition(polygon, PARTITION_METHOD.OPTIMAL);
            }
            else
            {
                partition = new List<Polygon2f>();
            }

            foreach(var poly in partition)
            {
                poly.MakeCCW();
                poly.BuildIndices();
                poly.BuildHoleIndices();
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

                if(!polygon.IsSimple || polygon.HasHoles)
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must be simple with no holes. Clear and start again.");
                }
                else if (polygon.Orientation != ORIENTATION.COUNTERCLOCKWISE)
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must have ccw orientation. Clear and start again.");
                }
                else
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Partitions = " + partition.Count);
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

                foreach (var polygon in partition)
                    DrawPolygon(cam, polygon, Color.green, Color.yellow);
            }
        }

    }
}
