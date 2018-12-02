using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.LinearAlgebra;
using Common.Core.Colors;
using Common.Unity.Drawing;

using CGAL.Meshes.FaceBased;
using CGAL.Meshes.HalfEdgeBased;
using CGAL.Polygons;
using CGAL.Triangulation.Conforming;

namespace CGALDemo
{

    public class DemoConformingTriangulation2 : MonoBehaviour
    {

        //HBMesh<HBVertex2f, HBEdge, HBFace> mesh;

        FBMesh<FBVertex2f, FBFace> mesh;

        List<Vector2f> positions;

        List<int> indices;

        List<Vector2f> linePositions;

        List<int> lineIndices;

       private void Start()
       {

            Polygon2f polygon = CreatePolygon2.FromCircle(new Vector2f(), 1.8f, 32);

            ConformingCriteria criteria = new ConformingCriteria();
            criteria.angBounds = 0.125f;
            criteria.lenBounds = 0.2f;
            criteria.iterations = 100;

            var constructor = new FBMeshConstructor<FBVertex2f, FBFace>();

            ConformingTriangulation2.Triangulate(polygon, constructor, criteria);
            mesh = constructor.PopMesh();

            int count = mesh.Vertices.Count;

            positions = new List<Vector2f>();
            for(int i = 0; i < mesh.Vertices.Count; i++)
                positions.Add(mesh.Vertices[i].Position);

            indices = mesh.CreateFaceIndices(3);

            CreateConnectionLines(mesh);

        }

        private void OnPostRender()
        {

            Camera cam = Camera.current;
            if (cam == null) return;
            if (mesh == null) return;

            DrawFaces.FaceMode = FACE_MODE.TRIANGLES;
            DrawLines.LineMode = LINE_MODE.TRIANGLES;
            DrawBase.Orientation = DRAW_ORIENTATION.XY;

            Matrix4x4f m = Matrix4x4f.Identity;

            //DrawFaces.Draw(cam, positions, Color.green, m, indices);
            DrawLines.Draw(cam, positions, Color.blue, m, indices);
            //DrawVertices.Draw(cam, 0.02f, positions, Color.yellow, m);

            DrawLines.LineMode = LINE_MODE.LINES;
            DrawLines.Draw(cam, linePositions, Color.red, m, lineIndices);

        }

        /// <summary>
        /// Create the lines to show the connections between faces
        /// for a face based mesh.
        /// </summary>
        void CreateConnectionLines(FBMesh<FBVertex2f, FBFace> mesh)
        {
            linePositions = new List<Vector2f>();
            lineIndices = new List<int>();

            foreach(var face in mesh.Faces)
            {
                Vector2f center = Vector2f.Zero;

                for(int i = 0; i < 3; i++)
                    center += face.GetVertex<FBVertex2f>(i).Position;

                center /= 3.0f;

                for (int i = 0; i < 3; i++)
                {
                    var neighbor = face.Neighbors[i];
                    if (neighbor == null) continue;

                    Vector2f c = Vector2f.Zero;

                    for (int j = 0; j < 3; j++)
                        c += neighbor.GetVertex<FBVertex2f>(j).Position;

                    c /= 3.0f;

                    int count = linePositions.Count;
                    linePositions.Add(center);
                    linePositions.Add(c);

                    lineIndices.Add(count);
                    lineIndices.Add(count+1);
                }
            }

        }

        /// <summary>
        /// Create the lines to show the connections between faces
        /// for a half edge based mesh.
        /// </summary>
        void CreateConnectionLines(HBMesh<HBVertex2f, HBEdge, HBFace> mesh)
        {
            linePositions = new List<Vector2f>();
            lineIndices = new List<int>();

            foreach (var face in mesh.Faces)
            {
                Vector2f center = Vector2f.Zero;
                foreach (HBVertex2f v in face.Edge.EnumerateVertices())
                    center += v.Position;

                center /= 3.0f;

                foreach(var edge in face.Edge.EnumerateEdges())
                {
                    if (edge.Opposite == null) continue;

                    var neighbor = edge.Opposite.Face;

                    Vector2f c = Vector2f.Zero;
                    foreach (HBVertex2f v in neighbor.Edge.EnumerateVertices())
                        c += v.Position;

                    c /= 3.0f;

                    int count = linePositions.Count;
                    linePositions.Add(center);
                    linePositions.Add(c);

                    lineIndices.Add(count);
                    lineIndices.Add(count + 1);
                }
            }

        }
    }
}
