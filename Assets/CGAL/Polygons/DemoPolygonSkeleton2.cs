using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Meshes.IndexBased;
using CGAL.Meshes.HalfEdgeBased;
using CGAL.Polygons;

namespace CGALDemo
{

    public class DemoPolygonSkeleton2 : Polygon2Input
    {

        Polygon2f polygon;

        Mesh2f line;

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

            if (polygon.IsSimple)
            {

                var constructor = new HBMeshConstructor<HBVertex2f, HBEdge, HBFace>();
                PolygonSkeleton2.CreateInteriorSkeleton(polygon, constructor);
                var mesh = constructor.PopMesh();

                line = HBMeshConversion.ToIndexableMesh2f(mesh);
            }
            else
            {
                line = null;
            }
            
        }

        protected override void OnPolygonCleared()
        {
            line = null;
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
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must be simple to make skeleton. Clear and start again.");
                }
                else if (polygon.Orientation != ORIENTATION.COUNTERCLOCKWISE)
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must have ccw orientation. Clear and start again.");
                }
                else
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Points = " + line.Positions.Length);
                    GUI.Label(new Rect(10, 50, textLen, textHeight), "Edges = " + (line.Indices.Length / 2));
                }
            }

        }

        protected override void OnPostRender()
        {
            Camera cam = Camera.current;
            if (cam == null) return;

            DrawLines.LineMode = LINE_MODE.LINES;
            DrawVertices.Orientation = DRAW_ORIENTATION.XY;
            Matrix4x4f m = Matrix4x4f.Identity;

            if (!MadePolygon)
            {
                base.OnPostRender();
            }
            else
            {
                DrawPolygon(cam, polygon, Color.green, Color.yellow);

                if (line != null)
                {
                    DrawLines.Draw(cam, line.Positions, Color.blue, m, line.Indices);
                    DrawVertices.Draw(cam, 0.02f, line.Positions, Color.yellow, m);
                }

            }

        }

    }
}
