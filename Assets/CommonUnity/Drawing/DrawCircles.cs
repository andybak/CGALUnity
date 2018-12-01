using UnityEngine;
using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Unity.Extensions;

namespace Common.Unity.Drawing
{

    public class DrawCircles : DrawBase
    {

        private static List<float> m_radii = new List<float>();

        public static void Draw(Camera camera, Vector2f position, float radius, Color color, Matrix4x4f localToWorld, int segments = 16)
        {
            if (camera == null) return;
            m_vertices.Clear();
            m_radii.Clear();

            m_radii.Add(radius);

            if (Orientation == DRAW_ORIENTATION.XY)
                m_vertices.Add(position.xy01.ToVector4());
            else if (Orientation == DRAW_ORIENTATION.XZ)
                m_vertices.Add(position.x0y1.ToVector4());

            DrawCirclesAsLines(camera, color, m_vertices, m_radii, localToWorld.ToMatrix4x4(), segments);
        }

        public static void Draw(Camera camera, Vector3f position, float radius, Color color, Matrix4x4f localToWorld, int segments = 16)
        {
            if (camera == null) return;
            m_vertices.Clear();
            m_radii.Clear();

            m_radii.Add(radius);
            m_vertices.Add(position.ToVector4());

            DrawCirclesAsLines(camera, color, m_vertices, m_radii, localToWorld.ToMatrix4x4(), segments);
        }

        private static void DrawCirclesAsLines(Camera camera, Color color, IList<Vector4> positions, IList<float> radii, Matrix4x4 localToWorld, int segments)
        {
            if (camera == null) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);

            double theta;
            float x, y;
            Vector4 a, b;

            for (int j = 0; j < positions.Count; j++)
            {
                Vector4 center = positions[j];
                float radius = radii[j];
                if (radius <= 0) continue;

                for (int i = 0; i < segments; i++)
                {
                    if(Orientation == DRAW_ORIENTATION.XY)
                    {
                        theta = 2.0 * Math.PI * i / segments;
                        x = radius * (float)Math.Cos(theta);
                        y = radius * (float)Math.Sin(theta);
                        a = center + new Vector4(x, y, 0, 1);

                        theta = 2.0 * Math.PI * (i + 1) / segments;
                        x = radius * (float)Math.Cos(theta);
                        y = radius * (float)Math.Sin(theta);
                        b = center + new Vector4(x, y, 0, 1);
                    }
                    else
                    {
                        theta = 2.0 * Math.PI * i / segments;
                        x = radius * (float)Math.Cos(theta);
                        y = radius * (float)Math.Sin(theta);
                        a = center + new Vector4(x, 0, y, 1);

                        theta = 2.0 * Math.PI * (i + 1) / segments;
                        x = radius * (float)Math.Cos(theta);
                        y = radius * (float)Math.Sin(theta);
                        b = center + new Vector4(x, 0, y, 1);
                    }

                    GL.Vertex(a);
                    GL.Vertex(b);
                }
            }

            GL.End();

            GL.PopMatrix();
        }

    }

}