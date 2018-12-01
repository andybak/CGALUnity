using UnityEngine;
using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Unity.Extensions;

namespace Common.Unity.Drawing
{

    public enum LINE_MODE { LINES, TRIANGLES, TETRAHEDRON  };

    public class DrawLines : DrawBase
    {

        public static LINE_MODE LineMode = LINE_MODE.LINES;

        #region INT

        public static void Draw(Camera camera, IEnumerable<Vector2i> vertices, Color color, Matrix4x4f localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach(var v in vertices)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v.xy01.ToVector4());
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(v.x0y1.ToVector4());
            }

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), indices);
        }

        #endregion

        #region DOUBLE

        public static void Draw(Camera camera, IEnumerable<Vector4d> vertices, Color color, Matrix4x4d localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach(var v in vertices)
                m_vertices.Add(v.ToVector4());

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), indices);
        }

        public static void Draw(Camera camera, IEnumerable<Vector3d> vertices, Color color, Matrix4x4d localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach (var v in vertices)
                m_vertices.Add(v.ToVector4());

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), indices);
        }

        public static void Draw(Camera camera, Vector3d a, Vector3d b, Color color, Matrix4x4d localToWorld)
        {
            if (camera == null) return;
            m_vertices.Clear();

            m_vertices.Add(a.ToVector4());
            m_vertices.Add(b.ToVector4());

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), null);
        }

        public static void Draw(Camera camera, IEnumerable<Vector2d> vertices, Color color, Matrix4x4d localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach (var v in vertices)
            {
                if(Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v.xy01.ToVector4());
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(v.x0y1.ToVector4());
            }

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), indices);
        }

        public static void Draw(Camera camera, Vector2d a, Vector2d b, Color color, Matrix4x4d localToWorld)
        {
            if (camera == null) return;
            m_vertices.Clear();

            if (Orientation == DRAW_ORIENTATION.XY)
            {
                m_vertices.Add(a.xy01.ToVector4());
                m_vertices.Add(b.xy01.ToVector4());
            }
            else if (Orientation == DRAW_ORIENTATION.XZ)
            {
                m_vertices.Add(a.x0y1.ToVector4());
                m_vertices.Add(b.x0y1.ToVector4());
            }

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), null);
        }

        #endregion

        #region FLOAT

        public static void Draw(Camera camera, IEnumerable<Vector4f> vertices, Color color, Matrix4x4f localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach (var v in vertices)
                m_vertices.Add(v.ToVector4());

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), indices);
        }

        public static void Draw(Camera camera, IEnumerable<Vector3f> vertices, Color color, Matrix4x4f localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach (var v in vertices)
                m_vertices.Add(v.ToVector4());

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), indices);
        }

        public static void Draw(Camera camera, Vector3f a, Vector3f b, Color color, Matrix4x4f localToWorld)
        {
            if (camera == null) return;
            m_vertices.Clear();

            m_vertices.Add(a.ToVector4());
            m_vertices.Add(b.ToVector4());

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), null);
        }

        public static void Draw(Camera camera, IEnumerable<Vector2f> vertices, Color color, Matrix4x4f localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach (var v in vertices)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v.xy01.ToVector4());
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(v.x0y1.ToVector4());
            }
                
            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), indices);
        }

        public static void Draw(Camera camera, Vector2f a, Vector2f b, Color color, Matrix4x4f localToWorld)
        {
            if (camera == null) return;
            m_vertices.Clear();

            if (Orientation == DRAW_ORIENTATION.XY)
            {
                m_vertices.Add(a.xy01.ToVector4());
                m_vertices.Add(b.xy01.ToVector4());
            } 
            else if (Orientation == DRAW_ORIENTATION.XZ)
            {
                m_vertices.Add(a.x0y1.ToVector4());
                m_vertices.Add(b.x0y1.ToVector4());
            }

            DrawVertices(camera, m_vertices, color, localToWorld.ToMatrix4x4(), null);
        }

        #endregion

        #region UNITY

        public static void Draw(Camera camera, IEnumerable<Vector4> vertices, Color color, Matrix4x4 localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach (var v in vertices)
                m_vertices.Add(v);

            DrawVertices(camera, m_vertices, color, localToWorld, indices);
        }

        public static void Draw(Camera camera, IEnumerable<Vector3> vertices, Color color, Matrix4x4 localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach (var v in vertices)
                m_vertices.Add(v);

            DrawVertices(camera, m_vertices, color, localToWorld, indices);
        }

        public static void Draw(Camera camera, Vector3 a, Vector3 b, Color color, Matrix4x4 localToWorld)
        {
            if (camera == null) return;
            m_vertices.Clear();

            m_vertices.Add(a);
            m_vertices.Add(b);
        
            DrawVertices(camera, m_vertices, color, localToWorld, null);
        }

        public static void Draw(Camera camera, IEnumerable<Vector2> vertices, Color color, Matrix4x4 localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;

            foreach (var v in vertices)
                m_vertices.Add(v);

            DrawVertices(camera, m_vertices, color, localToWorld, indices);
        }

        public static void Draw(Camera camera, Vector2 a, Vector2 b, Color color, Matrix4x4 localToWorld)
        {
            if (camera == null) return;
            m_vertices.Clear();

            if (Orientation == DRAW_ORIENTATION.XY)
            {
                m_vertices.Add(a);
                m_vertices.Add(b);
            }
            else if (Orientation == DRAW_ORIENTATION.XZ)
            {
                m_vertices.Add(new Vector4(a.x, 0, a.y, 1));
                m_vertices.Add(new Vector4(b.x, 0, b.y, 1));
            }

            DrawVertices(camera, m_vertices, color, localToWorld, null);
        }

        #endregion

        #region DRAW

        private static void DrawVertices(Camera camera, IList<Vector4> vertices, Color color, Matrix4x4 localToWorld, IList<int> indices)
        {
            switch (LineMode)
            {
                case LINE_MODE.LINES:
                    DrawVerticesAsLines(camera, color, vertices, localToWorld, indices);
                    break;

                case LINE_MODE.TRIANGLES:
                    DrawVerticesAsTriangles(camera, color, vertices, localToWorld, indices);
                    break;

                case LINE_MODE.TETRAHEDRON:
                    DrawVerticesAsTetrahedron(camera, color, vertices, localToWorld, indices);
                    break;
            }
        }

        private static void DrawVerticesAsLines(Camera camera, Color color, IList<Vector4> vertices, Matrix4x4 localToWorld, IList<int> indices)
        {
            if (camera == null || vertices == null) return;
            if (vertices.Count < 2) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);
         
            int vertexCount = vertices.Count;

            if(indices != null)
            {
                for (int i = 0; i < indices.Count / 2; i++)
                {
                    int i0 = indices[i * 2 + 0];
                    int i1 = indices[i * 2 + 1];

                    if (i0 < 0 || i0 >= vertexCount) continue;
                    if (i1 < 0 || i1 >= vertexCount) continue;

                    GL.Vertex(vertices[i0]);
                    GL.Vertex(vertices[i1]);
                }
            }
            else
            {
                for (int i = 0; i < vertexCount-1; i++)
                {
                    GL.Vertex(vertices[i ]);
                    GL.Vertex(vertices[i + 1]);
                }
            }

            GL.End();

            GL.PopMatrix();
        }

        private static void DrawVerticesAsTriangles(Camera camera, Color color, IList<Vector4> vertices, Matrix4x4 localToWorld, IList<int> indices)
        {
            if (camera == null || vertices == null) return;
            if (vertices.Count < 3) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);

            int vertexCount = vertices.Count;

            if(indices != null)
            {
                for (int i = 0; i < indices.Count / 3; i++)
                {
                    int i0 = indices[i * 3 + 0];
                    int i1 = indices[i * 3 + 1];
                    int i2 = indices[i * 3 + 2];

                    if (i0 < 0 || i0 >= vertexCount) continue;
                    if (i1 < 0 || i1 >= vertexCount) continue;
                    if (i2 < 0 || i2 >= vertexCount) continue;

                    GL.Vertex(vertices[i0]);
                    GL.Vertex(vertices[i1]);

                    GL.Vertex(vertices[i0]);
                    GL.Vertex(vertices[i2]);

                    GL.Vertex(vertices[i2]);
                    GL.Vertex(vertices[i1]);
                }
            }
            else
            {
                for (int i = 0; i < vertexCount / 3; i++)
                {
                    Vector3 v0 = vertices[i * 3 + 0];
                    Vector3 v1 = vertices[i * 3 + 1];
                    Vector3 v2 = vertices[i * 3 + 2];

                    GL.Vertex(v0);
                    GL.Vertex(v1);

                    GL.Vertex(v0);
                    GL.Vertex(v2);

                    GL.Vertex(v2);
                    GL.Vertex(v1);
                }
            }

            GL.End();

            GL.PopMatrix();
        }

        private static void DrawVerticesAsTetrahedron(Camera camera, Color color, IList<Vector4> vertices, Matrix4x4 localToWorld, IList<int> indices)
        {
            if (camera == null || vertices == null) return;
            if (vertices.Count < 4) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);

            int vertexCount = vertices.Count;

            if(indices != null)
            {
                for (int i = 0; i < indices.Count / 4; i++)
                {
                    int i0 = indices[i * 4 + 0];
                    int i1 = indices[i * 4 + 1];
                    int i2 = indices[i * 4 + 2];
                    int i3 = indices[i * 4 + 3];

                    if (i0 < 0 || i0 >= vertexCount) continue;
                    if (i1 < 0 || i1 >= vertexCount) continue;
                    if (i2 < 0 || i2 >= vertexCount) continue;
                    if (i3 < 0 || i3 >= vertexCount) continue;

                    GL.Vertex(vertices[i0]);
                    GL.Vertex(vertices[i1]);

                    GL.Vertex(vertices[i0]);
                    GL.Vertex(vertices[i2]);

                    GL.Vertex(vertices[i0]);
                    GL.Vertex(vertices[i3]);

                    GL.Vertex(vertices[i1]);
                    GL.Vertex(vertices[i2]);

                    GL.Vertex(vertices[i3]);
                    GL.Vertex(vertices[i2]);

                    GL.Vertex(vertices[i1]);
                    GL.Vertex(vertices[i3]);
                }
            }
            else
            {
                for (int i = 0; i < vertexCount / 4; i++)
                {
                    Vector3 v0 = vertices[i * 4 + 0];
                    Vector3 v1 = vertices[i * 4 + 1];
                    Vector3 v2 = vertices[i * 4 + 2];
                    Vector3 v3 = vertices[i * 4 + 3];


                    GL.Vertex(v0);
                    GL.Vertex(v1);

                    GL.Vertex(v0);
                    GL.Vertex(v2);

                    GL.Vertex(v0);
                    GL.Vertex(v3);

                    GL.Vertex(v1);
                    GL.Vertex(v2);

                    GL.Vertex(v3);
                    GL.Vertex(v2);

                    GL.Vertex(v1);
                    GL.Vertex(v3);
                }
            }

            GL.End();

            GL.PopMatrix();
        }

        #endregion

    }

}