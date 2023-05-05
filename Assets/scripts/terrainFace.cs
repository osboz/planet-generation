using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a face of a terrain mesh.
public class TerrainFace
{

    // The shape generator used to generate the terrain.
    ShapeGenerator shapeGenerator;

    // The mesh used to represent the terrain.
    Mesh mesh;

    // The resolution of the terrain mesh.
    int resolution;

    // The local up direction of the terrain face.
    Vector3 localUp;

    // The first axis used to calculate the terrain mesh vertices.
    Vector3 axisA;

    // The second axis used to calculate the terrain mesh vertices.
    Vector3 axisB;

    // Constructor for the class.
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        // Initialize the shape generator, mesh, resolution, and local up direction of the terrain face.
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        // Calculate the first axis of the terrain face.
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);

        // Calculate the second axis of the terrain face.
        axisB = Vector3.Cross(localUp, axisA);
    }

    // Construct the mesh for the terrain face.
    public void ConstructMesh()
    {
        // Create an array to hold the vertices of the terrain mesh.
        Vector3[] vertices = new Vector3[resolution * resolution];

        // Create an array to hold the triangles of the terrain mesh.
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        // Initialize the triangle index to 0.
        int triIndex = 0;

        // Loop over the vertices of the terrain mesh.
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // Calculate the index of the current vertex.
                int i = x + y * resolution;

                // Calculate the percentage of the x and y values of the vertex.
                Vector2 percent = new Vector2(x, y) / (resolution - 1);

                // Calculate the point on the unit cube corresponding to the current vertex.
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;

                // Calculate the point on the unit sphere corresponding to the current vertex.
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                // Calculate the point on the terrain corresponding to the current vertex.
                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                // Add triangles to the mesh if the current vertex is not on the edge of the terrain.
                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }

        // Clear the mesh and set its vertices and triangles.
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate the normals of the mesh.
        mesh.RecalculateNormals();
    }

    // Converts a point on the surface of a unit cube to a point on the surface of a unit sphere.
    public static Vector3 pointOnUnitCubeToPointOnUnitSphere(Vector3 p)
    {
        // Calculate the squares of the components of the input vector
        float x2 = p.x * p.x;
        float y2 = p.y * p.y;
        float z2 = p.z * p.z;

        // Calculate the three components of the output vector by multiplying
        // the corresponding component of the input vector with a scaling factor
        // that depends on the other two components
        float x = p.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 * z2) / 3);
        float y = p.y * Mathf.Sqrt(1 - (z2 + x2) / 2 + (z2 * x2) / 3);
        float z = p.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 * y2) / 3);

        // Return the output vector
        return new Vector3(x, y, z);
    }


}