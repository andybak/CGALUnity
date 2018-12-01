using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Meshes.IndexBased;
using CGAL.Polygons;
using CGAL.Triangulation.Constrainted;
using CGAL.Triangulation.Conforming;

namespace CGALDemo
{

    public enum TRIANGULATION_MODE { CONSTRAINTED, CONFORMING };

    public class DemoPolygonTriangulation2 : Polygon2Input
    {

        public TRIANGULATION_MODE mode = TRIANGULATION_MODE.CONSTRAINTED;

        Polygon2f polygon;

        Mesh2f mesh;

        int iterations = 0;

        protected override void OnPolygonComplete(Polygon2f input)
        {
            LineColor = Color.green;
            polygon = input;

            polygon.MakeCCW();
            polygon.BuildIndices();
            polygon.BuildHoleIndices();

            if (polygon.IsSimple)
            {
                if (mode == TRIANGULATION_MODE.CONSTRAINTED)
                {
                    mesh = ConstraintedTriangulation2.Triangulate(polygon);
                }
                else
                {
                    ConformingCriteria criteria = new ConformingCriteria();
                    criteria.angBounds = 0.125f;
                    criteria.lenBounds = 0.2f;
                    criteria.iterations = iterations;
                    mesh = ConformingTriangulation2.Triangulate(polygon, criteria);
                }
            }
            else
            {
                mesh = new Mesh2f(0, 0);
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

            if (mode == TRIANGULATION_MODE.CONFORMING)
            {
                if (Input.GetKeyDown(KeyCode.KeypadPlus))
                    iterations++;

                if (Input.GetKeyDown(KeyCode.KeypadMinus))
                    iterations--;

                GUI.Label(new Rect(10, 90, textLen, textHeight), "iterations = " + iterations);
            }

            if (!MadePolygon)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
                GUI.Label(new Rect(10, 70, textLen, textHeight), "Mode = " + mode);
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");

                if (!polygon.IsSimple)
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must be simple to triangulate. Clear and start again.");
                }
                else if (polygon.Orientation != ORIENTATION.COUNTERCLOCKWISE)
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Polygon must have ccw orientation. Clear and start again.");
                }
                else
                {
                    GUI.Label(new Rect(10, 30, textLen, textHeight), "Points = " + mesh.Positions.Length);
                    GUI.Label(new Rect(10, 50, textLen, textHeight), "Edges = " + (mesh.Indices.Length / 3));
                }
            }

        }

        protected override void OnPostRender()
        {

            if (!MadePolygon)
                base.OnPostRender();
            else
            {
                Camera cam = Camera.current;
                if (cam == null) return;

                DrawLines.LineMode = LINE_MODE.TRIANGLES;
                DrawBase.Orientation = DRAW_ORIENTATION.XY;
                Matrix4x4f m = Matrix4x4f.Identity;

                DrawLines.Draw(cam, mesh.Positions, Color.blue, m, mesh.Indices);
                DrawVertices.Draw(cam, 0.02f, mesh.Positions, Color.yellow, m);
            }

        }

    }
}
