using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Core.Colors;
using Common.Unity.Extensions;

namespace Common.Unity.Drawing
{

    public class DrawVertices : DrawBase
    {

        #region INT
        public static void Draw(Camera camera, float size, IEnumerable<Vector2i> vertices, Color color, Matrix4x4f localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            foreach(var v in vertices)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v.xy01.ToVector4());
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(v.x0y1.ToVector4());

                m_colors.Add(color);
            }

            Draw(camera, size, m_vertices, m_colors, localToWorld.ToMatrix4x4());
        }

        public static void Draw(Camera camera, float size, IEnumerable<Vector2i> vertices, IEnumerable<ColorRGBA> colors, Matrix4x4f localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            foreach (var v in vertices)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v.xy01.ToVector4());
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(v.x0y1.ToVector4());
            }

            foreach(var c in colors)
                m_colors.Add(c.ToColor());

            Draw(camera, size, m_vertices, m_colors, localToWorld.ToMatrix4x4());
        }
        #endregion

        #region FLOAT
        public static void Draw(Camera camera, float size, IEnumerable<Vector3f> vertices, IEnumerable<ColorRGBA> colors, Matrix4x4f localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            foreach(var v in vertices)
                m_vertices.Add(v.ToVector4());

            foreach (var c in colors)
                m_colors.Add(c.ToColor());

            Draw(camera, size, m_vertices, m_colors, localToWorld.ToMatrix4x4());
        }

        public static void Draw(Camera camera, float size, IEnumerable<Vector3f> vertices, Color color, Matrix4x4f localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            foreach (var v in vertices)
            {
                m_vertices.Add(v.ToVector4());
                m_colors.Add(color);
            }

            Draw(camera, size, m_vertices, m_colors, localToWorld.ToMatrix4x4());
        }

        public static void Draw(Camera camera, float size, Vector3f vertex, Color color, Matrix4x4f localToWorld)
        {
            if (camera == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            m_vertices.Add(vertex.ToVector4());
            m_colors.Add(color);

            Draw(camera, size, m_vertices, m_colors, localToWorld.ToMatrix4x4());
        }

        public static void Draw(Camera camera, float size, IEnumerable<Vector2f> vertices, IEnumerable<ColorRGBA> colors, Matrix4x4f localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            foreach(var v in vertices)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v.xy01.ToVector4());
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(v.x0y1.ToVector4());
            }

            foreach (var c in colors)
                m_colors.Add(c.ToColor());

            Draw(camera, size, m_vertices, m_colors, localToWorld.ToMatrix4x4());
        }

        public static void Draw(Camera camera, float size, IEnumerable<Vector2f> vertices, Color color, Matrix4x4f localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            foreach (var v in vertices)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v.xy01.ToVector4());
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(v.x0y1.ToVector4());

                m_colors.Add(color);
            }

            Draw(camera, size, m_vertices, m_colors, localToWorld.ToMatrix4x4());
        }

        public static void Draw(Camera camera, float size, Vector2f vertex, Color color, Matrix4x4f localToWorld)
        {
            if (camera == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            if (Orientation == DRAW_ORIENTATION.XY)
                m_vertices.Add(vertex.xy01.ToVector4());
            else if (Orientation == DRAW_ORIENTATION.XZ)
                m_vertices.Add(vertex.x0y1.ToVector4());

            m_colors.Add(color);

            Draw(camera, size, m_vertices, m_colors, localToWorld.ToMatrix4x4());
        }
        #endregion

        #region UNITY
        public static void Draw(Camera camera, float size, IEnumerable<Vector3> vertices, IEnumerable<Color> colors, Matrix4x4 localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();

            foreach(var v in vertices)
                m_vertices.Add(v);

            foreach (var c in colors)
                m_colors.Add(c);

            Draw(camera, size, m_vertices, m_colors, localToWorld);
        }

        public static void Draw(Camera camera, float size, IEnumerable<Vector3> vertices, Color color, Matrix4x4 localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            foreach (var v in vertices)
            {
                m_vertices.Add(v);
                m_colors.Add(color);
            }

            Draw(camera, size, m_vertices, m_colors, localToWorld);
        }

        public static void Draw(Camera camera, float size, IEnumerable<Vector2> vertices, Color color, Matrix4x4 localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            foreach (var v in vertices)
            {
                m_vertices.Add(v);
                m_colors.Add(color);
            }

            Draw(camera, size, m_vertices, m_colors, localToWorld);
        }
        #endregion

        #region DRAW
        private static void Draw(Camera camera, float size, IList<Vector4> vertices, IList<Color> colors, Matrix4x4 localToWorld)
        {
            if (camera == null || vertices == null) return;
            if (vertices.Count != colors.Count) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.QUADS);

            switch (Orientation)
            {
                case DRAW_ORIENTATION.XY:
                    DrawXY(size, vertices, colors);
                    break;

                case DRAW_ORIENTATION.XZ:
                    DrawXZ(size, vertices, colors);
                    break;
            }

            GL.End();

            GL.PopMatrix();
        }

        private static void DrawXY(float size, IList<Vector4> vertices, IList<Color> colors)
        {
            float half = size * 0.5f;
            for (int i = 0; i < vertices.Count; i++)
            {
                float x = vertices[i].x;
                float y = vertices[i].y;
                float z = vertices[i].z;

                GL.Color(colors[i]);
                GL.Vertex3(x + half, y + half, z);
                GL.Vertex3(x + half, y - half, z);
                GL.Vertex3(x - half, y - half, z);
                GL.Vertex3(x - half, y + half, z);
            }
        }

        private static void DrawXZ(float size, IList<Vector4> vertices, IList<Color> colors)
        {
            float half = size * 0.5f;
            for (int i = 0; i < vertices.Count; i++)
            {
                float x = vertices[i].x;
                float y = vertices[i].y;
                float z = vertices[i].z;

                GL.Color(colors[i]);
                GL.Vertex3(x + half, y, z + half);
                GL.Vertex3(x + half, y, z - half);
                GL.Vertex3(x - half, y, z - half);
                GL.Vertex3(x - half, y, z + half);
            }
        }
        #endregion
    }

}
