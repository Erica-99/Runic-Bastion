using System.Collections.Generic;
using UnityEngine;

namespace RunicBastion.Utils
{
    public static class CustomMeshGenerator
    {
        public static Mesh GenerateRingMesh(int segments = 360, float outerRadius = 1.0f, float thickness = 0.1f, float verticalThickness = 0.1f)
        {
            Mesh mesh = new Mesh();

            float innerRadius = outerRadius - thickness;
            float halfHeight = verticalThickness / 2f;

            int ringVertexCount = segments * 4; // 2 rings
            int sideVertexCount = segments * 4; // 2 sides
            int totalVertexCount = ringVertexCount + sideVertexCount;

            Vector3[] vertices = new Vector3[totalVertexCount];
            Vector3[] normals = new Vector3[totalVertexCount];
            List<int> triangles = new List<int>();

            int vert = 0;

            // Top surface vertices
            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);

                Vector3 innerTop = new Vector3(cos * innerRadius, halfHeight, sin * innerRadius);
                Vector3 outerTop = new Vector3(cos * outerRadius, halfHeight, sin * outerRadius);

                vertices[vert] = innerTop;
                vertices[vert + 1] = outerTop;

                normals[vert] = Vector3.up;
                normals[vert + 1] = Vector3.up;

                vert += 2;
            }

            // Bottom surface vertices
            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);

                Vector3 innerBottom = new Vector3(cos * innerRadius, -halfHeight, sin * innerRadius);
                Vector3 outerBottom = new Vector3(cos * outerRadius, -halfHeight, sin * outerRadius);

                vertices[vert] = innerBottom;
                vertices[vert + 1] = outerBottom;

                normals[vert] = Vector3.down;
                normals[vert + 1] = Vector3.down;

                vert += 2;
            }

            int topStart = 0;
            int bottomStart = segments * 2;

            // Top face triangles
            for (int i = 0; i < segments; i++)
            {
                int nextI = (i + 1) % segments;
                int i0 = topStart + i * 2;
                int i1 = i0 + 1;
                int i2 = topStart + nextI * 2;
                int i3 = i2 + 1;

                triangles.Add(i0);
                triangles.Add(i2);
                triangles.Add(i1);

                triangles.Add(i1);
                triangles.Add(i2);
                triangles.Add(i3);
            }

            // Bottom face triangles
            for (int i = 0; i < segments; i++)
            {
                int nextI = (i + 1) % segments;
                int i0 = bottomStart + i * 2;
                int i1 = i0 + 1;
                int i2 = bottomStart + nextI * 2;
                int i3 = i2 + 1;

                triangles.Add(i0);
                triangles.Add(i1);
                triangles.Add(i2);

                triangles.Add(i1);
                triangles.Add(i3);
                triangles.Add(i2);
            }

            // Sides vertices
            int sideStart = vert;

            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);

                Vector3 dir = new Vector3(cos, 0, sin).normalized;

                // Inner wall
                vertices[vert] = new Vector3(cos * innerRadius, halfHeight, sin * innerRadius);
                normals[vert] = -dir;
                vert++;
                vertices[vert] = new Vector3(cos * innerRadius, -halfHeight, sin * innerRadius);
                normals[vert] = -dir;
                vert++;

                // Outer wall
                vertices[vert] = new Vector3(cos * outerRadius, halfHeight, sin * outerRadius);
                normals[vert] = dir;
                vert++;
                vertices[vert] = new Vector3(cos * outerRadius, -halfHeight, sin * outerRadius);
                normals[vert] = dir;
                vert++;
            }

            // Sides faces
            for (int i = 0; i < segments; i++)
            {
                int next = (i + 1) % segments;

                int innerA_top = sideStart + i * 4 + 0;
                int innerA_bottom = sideStart + i * 4 + 1;
                int outerA_top = sideStart + i * 4 + 2;
                int outerA_bottom = sideStart + i * 4 + 3;

                int innerB_top = sideStart + next * 4 + 0;
                int innerB_bottom = sideStart + next * 4 + 1;
                int outerB_top = sideStart + next * 4 + 2;
                int outerB_bottom = sideStart + next * 4 + 3;

                // Inner wall
                triangles.Add(innerA_top);
                triangles.Add(innerA_bottom);
                triangles.Add(innerB_bottom);

                triangles.Add(innerA_top);
                triangles.Add(innerB_bottom);
                triangles.Add(innerB_top);

                // Outer wall
                triangles.Add(outerA_top);
                triangles.Add(outerB_bottom);
                triangles.Add(outerA_bottom);

                triangles.Add(outerA_top);
                triangles.Add(outerB_top);
                triangles.Add(outerB_bottom);
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateBounds();
            return mesh;
        }

        public static Mesh GenerateSphereMesh(int segments = 20, float radius = 1f)
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[(segments + 1) * (segments + 1)];
            int[] triangles = new int[segments * segments * 6];
            Vector2[] uv = new Vector2[vertices.Length];

            // Generate Vertices and UVs
            for (int i = 0; i <= segments; i++)
            {
                float phi = Mathf.PI * i / segments;
                for (int j = 0; j <= segments; j++)
                {
                    float theta = 2 * Mathf.PI * j / segments;

                    float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = radius * Mathf.Cos(phi);
                    float z = radius * Mathf.Sin(phi) * Mathf.Sin(theta);

                    int index = i * (segments + 1) + j;
                    vertices[index] = new Vector3(x, y, z);
                    uv[index] = new Vector2((float)j / segments, (float)i / segments);
                }
            }

            // Generate Triangles
            int triIndex = 0;
            for (int i = 0; i < segments; i++)
            {
                for (int j = 0; j < segments; j++)
                {
                    int current = i * (segments + 1) + j;
                    int next = current + 1;
                    int down = (i + 1) * (segments + 1) + j;
                    int downNext = down + 1;

                    triangles[triIndex++] = current;
                    triangles[triIndex++] = downNext;
                    triangles[triIndex++] = down;

                    triangles[triIndex++] = current;
                    triangles[triIndex++] = next;
                    triangles[triIndex++] = downNext;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;

            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
