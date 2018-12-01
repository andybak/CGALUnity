using UnityEngine;
using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Unity.Extensions;

namespace Common.Unity.Drawing
{

    public class DrawNormals : DrawBase
    {

        private static List<Vector4> m_normals = new List<Vector4>();

        #region DOUBLE

        public static void Draw(Camera camera, IEnumerable<Vector3d> vertices, IEnumerable<Vector3d> normals, Color color, double length, Matrix4x4d localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_normals.Clear();

            foreach (var v in vertices)
                m_vertices.Add(v.ToVector4());

            foreach (var n in normals)
                m_normals.Add(n.ToVector4());
            
            Draw(camera, color, m_vertices, m_normals, (float)length, localToWorld.ToMatrix4x4());
        }

        #endregion

        #region FLOAT

        public static void Draw(Camera camera, IEnumerable<Vector3f> vertices, IEnumerable<Vector3f> normals, Color color, float length, Matrix4x4f localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_normals.Clear();

            foreach (var v in vertices)
                m_vertices.Add(v.ToVector4());

            foreach (var n in normals)
                m_normals.Add(n.ToVector4());

            Draw(camera, color, m_vertices, m_normals, length, localToWorld.ToMatrix4x4());
        }

        public static void Draw(Camera camera, Vector3f vertex, Vector3f normal, Color color, float length, Matrix4x4f localToWorld)
        {
            if (camera == null) return;
            m_vertices.Clear();
            m_normals.Clear();

            m_vertices.Add(vertex.ToVector4());
            m_normals.Add(normal.ToVector4());

            Draw(camera, color, m_vertices, m_normals, length, localToWorld.ToMatrix4x4());
        }

        public static void Draw(Camera camera, IEnumerable<Vector2f> vertices, IEnumerable<Vector2f> normals, Color color, float length, Matrix4x4f localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_normals.Clear();

            foreach(var v in vertices)
            {
                if(Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v.xy01.ToVector4());
                else
                    m_vertices.Add(v.x0y1.ToVector4());
            }

            foreach (var n in normals)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    m_normals.Add(n.xy01.ToVector4());
                else
                    m_normals.Add(n.x0y1.ToVector4());
            }

            Draw(camera, color, m_vertices, m_normals, length, localToWorld.ToMatrix4x4());
        }
        #endregion

        #region UNITY

        public static void Draw(Camera camera, IEnumerable<Vector3> vertices, IEnumerable<Vector3> normals, Color color, float length, Matrix4x4 localToWorld)
        {
            if (camera == null || vertices == null) return;
            m_vertices.Clear();
            m_normals.Clear();

            foreach (var v in vertices)
                m_vertices.Add(v);

            foreach (var n in normals)
                m_normals.Add(n);

            Draw(camera, color, m_vertices, m_normals, length, localToWorld);
        }

        #endregion

        #region DRAW

        private static void Draw(Camera camera, Color color, IList<Vector4> vertices, IList<Vector4> normals, float length, Matrix4x4 localToWorld)
        {
            if (camera == null || vertices == null) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);

            int vertexCount = vertices.Count;
            for (int i = 0; i < vertexCount; i++)
            {
                GL.Vertex(vertices[i]);
                GL.Vertex(vertices[i] + normals[i] * length);
            }

            GL.End();

            GL.PopMatrix();
        }

        #endregion
    }

}