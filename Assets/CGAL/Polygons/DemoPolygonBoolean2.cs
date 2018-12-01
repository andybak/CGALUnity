using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Polygons;

namespace CGALDemo
{
    public enum BOOL_MODE { UNION, INTERSECTION, DIFFERENCE, SYM_DIFFERENCE };

    public class DemoPolygonBoolean2 : Polygon2Input
    {

        List<Polygon2f> polygons = new List<Polygon2f>();

        Polygon2f input;

        BOOL_MODE mode = BOOL_MODE.UNION;

        private void Start()
        {
            SnapPoint = 10.0f;
        }

        protected override void OnPolygonComplete(Polygon2f _input)
        {
            LineColor = Color.red;

            input = _input;

            input.MakeCCW();
            input.BuildIndices();
            input.BuildHoleIndices();

            ResetInput();

            if (!input.IsSimple) return;

            if (polygons.Count == 0)
            {
                polygons.Add(input);
            }
            else if(polygons[0].IsSimple)
            {
                Polygon2f A = polygons[0];
                Polygon2f B = input;
                polygons.Clear();
                List<Polygon2f> output = new List<Polygon2f>();

                bool b = false;

                switch (mode)
                {
                    case BOOL_MODE.UNION:
                        b = PolygonBoolean2.Union(A, B, output);
                        break;

                    case BOOL_MODE.INTERSECTION:
                        b = PolygonBoolean2.Intersection(A, B, output);
                        break;

                    case BOOL_MODE.DIFFERENCE:
                        b = PolygonBoolean2.Difference(A, B, output);
                        break;

                    case BOOL_MODE.SYM_DIFFERENCE:
                        b = PolygonBoolean2.SymmetricDifference(A, B, output);
                        break;
                }

                if (b)
                {
                    foreach (var polygon in output)
                    {
                        polygons.Add(polygon);
                        polygon.BuildIndices();
                        polygon.BuildHoleIndices();
                    }
                }

            }

        }

        protected override void OnPolygonCleared()
        {
            LineColor = Color.red;
            polygons.Clear();
        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            GUI.Label(new Rect(10, 70, textLen, textHeight), "Mode = " + mode);

            if (input != null)
            {
                if (!input.IsSimple)
                {
                    GUI.Label(new Rect(10, 90, textLen, textHeight), "Input must be simple. Start again.");
                }
                else if (!input.IsCCW)
                {
                    GUI.Label(new Rect(10, 90, textLen, textHeight), "Input must have ccw orientation. Start again.");
                }
            }

        }

        protected override void OnPostRender()
        {
            base.OnPostRender();

            Camera cam = Camera.current;
            if (cam == null) return;

            for (int i = 0; i < polygons.Count; i++)
            {
                DrawPolygon(cam, polygons[i], Color.green);
            }

        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                uint i = (uint)mode + 1;
                mode = (BOOL_MODE)(i % 4);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                uint i = (uint)mode - 1;
                mode = (BOOL_MODE)(i % 4);
            }
        }

        private void DrawPolygon(Camera cam, Polygon2f polygon, Color col)
        {
            DrawLines.LineMode = LINE_MODE.LINES;
            DrawVertices.Orientation = DRAW_ORIENTATION.XY;
            Matrix4x4f m = Matrix4x4f.Identity;

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
