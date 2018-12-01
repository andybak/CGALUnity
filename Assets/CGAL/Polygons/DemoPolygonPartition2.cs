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
            LineColor = Color.green;
            polygon = input;

            polygon.MakeCCW();
            polygon.BuildIndices();
            polygon.BuildHoleIndices();

            if (polygon.IsSimple)
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

                if(!polygon.IsSimple)
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must be simple to partition. Clear and start again.");
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
            if (!MadePolygon || !polygon.IsSimple || polygon.Orientation != ORIENTATION.COUNTERCLOCKWISE)
            {
                base.OnPostRender();
            }
            else
            {
                Camera cam = Camera.current;
                if (cam == null) return;

                Matrix4x4f m = Matrix4x4f.Identity;

                int num = partition.Count;

                for (int i = 0; i < num; i++)
                {
                    var poly = partition[i];

                    DrawLines.LineMode = LINE_MODE.LINES;
                    DrawLines.Draw(cam, poly.Positions, LineColor, m, poly.Indices);

                    DrawVertices.Orientation = DRAW_ORIENTATION.XY;
                    DrawVertices.Draw(cam, 0.02f, poly.Positions, Color.yellow, m);
                }

            }
        }

    }
}
