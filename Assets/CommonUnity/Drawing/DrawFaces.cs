using UnityEngine;
using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Colors;
using Common.Unity.Extensions;

namespace Common.Unity.Drawing
{

    public enum FACE_MODE { TRIANGLES, QUADS };

    public class DrawFaces : DrawBase
    {

        public static FACE_MODE FaceMode = FACE_MODE.TRIANGLES;

        public static void Draw(Camera camera, IList<Vector2f> vertices, IList<ColorRGBA> colors, Matrix4x4f localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            int count = vertices.Count;
            for (int i = 0; i < count; i++)
            {
                m_vertices.Add(vertices[i].xy01.ToVector4());
                m_colors.Add(colors[i].ToColor());
            }

            DrawVertices(camera, m_vertices, m_colors, localToWorld.ToMatrix4x4(), indices);
        }

        public static void Draw(Camera camera, IList<Vector2f> vertices, Color color, Matrix4x4f localToWorld, IList<int> indices = null)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_colors.Clear();

            int count = vertices.Count;
            for (int i = 0; i < count; i++)
            {
                m_vertices.Add(vertices[i].xy01.ToVector4());
                m_colors.Add(color);
            }

            DrawVertices(camera, m_vertices, m_colors, localToWorld.ToMatrix4x4(), indices);
        }

        private static void DrawVertices(Camera camera, IList<Vector4> vertices, List<Color> colors, Matrix4x4 localToWorld, IList<int> indices)
        {

            switch (FaceMode)
            {
                case FACE_MODE.QUADS:
                    DrawVerticesAsQuads(camera, colors, vertices, localToWorld, indices);
                    break;

                case FACE_MODE.TRIANGLES:
                    DrawVerticesAsTriangles(camera, colors, vertices, localToWorld, indices);
                    break;
            }

        }

        private static void DrawVerticesAsQuads(Camera camera, List<Color> colors, IList<Vector4> vertices, Matrix4x4 localToWorld, IList<int> indices)
        {
            const int stride = 4;

            if (camera == null || vertices == null) return;
            if (vertices.Count < stride) return;
            if (m_colors.Count != vertices.Count) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.QUADS);

            int vertexCount = vertices.Count;

            if (indices != null)
            {
                for (int i = 0; i < indices.Count / stride; i++)
                {
                    int i0 = indices[i * stride + 0];
                    int i1 = indices[i * stride + 1];
                    int i2 = indices[i * stride + 2];
                    int i3 = indices[i * stride + 3];

                    if (i0 < 0 || i0 >= vertexCount) continue;
                    if (i1 < 0 || i1 >= vertexCount) continue;
                    if (i2 < 0 || i2 >= vertexCount) continue;
                    if (i3 < 0 || i3 >= vertexCount) continue;

                    GL.Vertex(vertices[i0]);
                    GL.Color(colors[i0]);

                    GL.Vertex(vertices[i1]);
                    GL.Color(colors[i1]);

                    GL.Vertex(vertices[i2]);
                    GL.Color(colors[i2]);

                    GL.Vertex(vertices[i3]);
                    GL.Color(colors[i3]);
                }
            }
            else
            {
                for (int i = 0; i < vertexCount / stride; i++)
                {
                    int i0 = i * stride + 0;
                    int i1 = i * stride + 1;
                    int i2 = i * stride + 2;
                    int i3 = i * stride + 3;

                    GL.Vertex(vertices[i0]);
                    GL.Color(colors[i0]);

                    GL.Vertex(vertices[i1]);
                    GL.Color(colors[i1]);

                    GL.Vertex(vertices[i2]);
                    GL.Color(colors[i2]);

                    GL.Vertex(vertices[i3]);
                    GL.Color(colors[i3]);
                }
            }

            GL.End();

            GL.PopMatrix();
        }

        private static void DrawVerticesAsTriangles(Camera camera, List<Color> colors, IList<Vector4> vertices, Matrix4x4 localToWorld, IList<int> indices)
        {
            const int stride = 3;

            if (camera == null || vertices == null) return;
            if (vertices.Count < stride) return;
            if (m_colors.Count != vertices.Count) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.TRIANGLES);

            int vertexCount = vertices.Count;

            if (indices != null)
            {
                for (int i = 0; i < indices.Count / stride; i++)
                {
                    int i0 = indices[i * stride + 0];
                    int i1 = indices[i * stride + 1];
                    int i2 = indices[i * stride + 2];

                    if (i0 < 0 || i0 >= vertexCount) continue;
                    if (i1 < 0 || i1 >= vertexCount) continue;
                    if (i2 < 0 || i2 >= vertexCount) continue;

                    GL.Vertex(vertices[i0]);
                    GL.Color(colors[i0]);

                    GL.Vertex(vertices[i1]);
                    GL.Color(colors[i1]);

                    GL.Vertex(vertices[i2]);
                    GL.Color(colors[i2]);
                }
            }
            else
            {
                for (int i = 0; i < vertexCount / stride; i++)
                {
                    int i0 = i * stride + 0;
                    int i1 = i * stride + 1;
                    int i2 = i * stride + 2;

                    GL.Vertex(vertices[i0]);
                    GL.Color(colors[i0]);

                    GL.Vertex(vertices[i1]);
                    GL.Color(colors[i1]);

                    GL.Vertex(vertices[i2]);
                    GL.Color(colors[i2]);
                }
            }

            GL.End();

            GL.PopMatrix();
        }

    }

}