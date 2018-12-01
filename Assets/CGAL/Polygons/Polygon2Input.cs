using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Unity.Drawing;
using CGAL.Polygons;

namespace CGALDemo
{

    public abstract class Polygon2Input : MonoBehaviour
    {

        private const float SNAP_DIST = 0.1f;

        private const int MIN_POINTS = 3;

        protected List<Vector2f> Points { get; private set; }

        private List<int> Indices { get; set; }

        protected Color LineColor = Color.red;

        protected bool MadePolygon { get; set; }

        protected float SnapPoint { get; set; }

        protected virtual void OnPolygonComplete(Polygon2f input)
        {

        }

        protected virtual void OnPolygonCleared()
        {

        }

        protected virtual void OnLeftClick(Vector2f point)
        {

        }

        protected virtual void Update()
        {

            if (Input.GetKeyDown(KeyCode.F1))
            {
                ResetInput();
                MadePolygon = true;
                OnPolygonComplete(CreatePolygon2.FromBox(new Vector2f(-1), new Vector2f(1)));
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                ResetInput();
                Polygon2f box = CreatePolygon2.FromBox(new Vector2f(-1), new Vector2f(1));
                Polygon2f hole = CreatePolygon2.FromBox(new Vector2f(-0.5f), new Vector2f(0.5f));
                hole.MakeCW();
                box.AddHole(hole);
                MadePolygon = true;
                OnPolygonComplete(box);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                ResetInput();
                Polygon2f circle = CreatePolygon2.FromCircle(new Vector2f(-1), 1, 16);
                MadePolygon = true;
                OnPolygonComplete(circle);
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                ResetInput();
                Polygon2f circle = CreatePolygon2.FromCircle(new Vector2f(0), 1, 16);
                Polygon2f hole = CreatePolygon2.FromCircle(new Vector2f(0), 0.5f, 16);
                hole.MakeCW();
                circle.AddHole(hole);
                MadePolygon = true;
                OnPolygonComplete(circle);
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                ResetInput();
                Polygon2f capsule = CreatePolygon2.FromCapsule(new Vector2f(0), 1, 1, 16);
                MadePolygon = true;
                OnPolygonComplete(capsule);
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                ResetInput();
                Polygon2f cathedral = CreatePolygon2.FromCapsule(new Vector2f(0), 1, 1, 16);

                for(float x = -0.5f; x <= 0.5f; x+= 1.0f)
                {
                    for (float y = -0.75f; y <= 0.75f; y += 0.5f)
                    {
                        Polygon2f pillar = CreatePolygon2.FromCircle(new Vector2f(x,y), 0.1f, 16);
                        pillar.MakeCW();
                        cathedral.AddHole(pillar);
                    }
                }

                MadePolygon = true;
                OnPolygonComplete(cathedral);
            }
            else
            {

                bool leftMouseClicked = Input.GetMouseButtonDown(0);

                if (leftMouseClicked)
                {
                    Vector2f point = GetMousePosition();
                    OnLeftClick(point);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ResetInput();
                    OnPolygonCleared();
                }
                else if (!MadePolygon)
                {
                    Vector2f point = GetMousePosition();
                    point = SnapToPolygon(point);

                    if (leftMouseClicked)
                    {
                        if (Points == null)
                        {
                            CreatePoints();
                            AddPoint(point);
                            AddPoint(point);
                        }
                        else
                        {
                            if (PolygonClosed())
                            {
                                ClosePolygon();
                                MadePolygon = true;

                                OnPolygonComplete(new Polygon2f(Points.ToArray()));
                            }
                            else
                            {
                                AddPoint(point);
                            }
                        }
                    }
                    else
                    {
                        MoveLastPoint(point);
                    }

                }

            }

        }

        protected virtual void OnPostRender()
        {

            Camera cam = Camera.current;
            if (cam == null) return;
            if (Points == null) return;
            if (Indices == null) return;

            Matrix4x4f m = Matrix4x4f.Identity;

            DrawLines.LineMode = LINE_MODE.LINES;
            DrawVertices.Orientation = DRAW_ORIENTATION.XY;

            DrawLines.Draw(cam, Points, LineColor, m, Indices);
            DrawVertices.Draw(cam, 0.02f, Points, Color.yellow, m);

        }

        protected void DrawPolygon(Camera cam, Polygon2f polygon, Color lineColor, Color vertColor)
        {
            if (polygon == null) return;

            Matrix4x4f m = Matrix4x4f.Identity;

            DrawLines.LineMode = LINE_MODE.LINES;
            DrawVertices.Orientation = DRAW_ORIENTATION.XY;

            DrawLines.Draw(cam, polygon.Positions, lineColor, m, polygon.Indices);
            DrawVertices.Draw(cam, 0.02f, polygon.Positions, vertColor, m);

            if (!polygon.HasHoles) return;

            foreach (var hole in polygon.Holes)
            {
                DrawLines.Draw(cam, hole.Positions, lineColor, m, hole.Indices);
                DrawVertices.Draw(cam, 0.02f, hole.Positions, vertColor, m);
            }
        }

        protected void ResetInput()
        {
            MadePolygon = false;
            Points = null;
            Indices = null;
        }

        private Vector2f GetMousePosition()
        {
            Vector3 p = Input.mousePosition;

            Camera cam = GetComponent<Camera>();
            p = cam.ScreenToWorldPoint(p);

            if (SnapPoint > 0.0f)
            {
                p.x = Mathf.Round(p.x * SnapPoint) / SnapPoint;
                p.y = Mathf.Round(p.y * SnapPoint) / SnapPoint;
            }

            return new Vector2f(p.x, p.y);
        }

        private void CreatePoints()
        {
            Points = new List<Vector2f>();
            Indices = new List<int>();
        }

        private void AddPoint(Vector2f point)
        {
            if (Points == null) return;

            Points.Add(point);

            int count = Points.Count;
            if (count == 1) return;

            Indices.Add(count - 2);
            Indices.Add(count - 1);
        }

        private void MoveLastPoint(Vector2f point)
        {
            if (Points == null) return;

            int count = Points.Count;
            if (count == 0) return;

            Points[count - 1] = point;
        }

        private Vector2f SnapToPolygon(Vector2f point)
        {
            if (Points == null) return point;

            int count = Points.Count;
            if (count < 4) return point;

            float x = Points[0].x - point.x;
            float y = Points[0].y - point.y;

            float dist = Mathf.Sqrt(x * x + y * y);

            if (dist <= SNAP_DIST)
            {
                point.x = Points[0].x;
                point.y = Points[0].y;
            }

            return point;
        }

        private bool PolygonClosed()
        {
            if (Points == null) return false;

            int count = Points.Count;
            if (count < MIN_POINTS) return false;

            float x = Points[0].x - Points[count - 1].x;
            float y = Points[0].y - Points[count - 1].y;

            return Mathf.Sqrt(x * x + y * y) <= SNAP_DIST;
        }

        private void ClosePolygon()
        {
            int count = Points.Count;
            if (count < MIN_POINTS) return;

            Points.RemoveAt(count - 1);

            Indices.Add(count - 2);
            Indices.Add(0);
        }

    }
}
