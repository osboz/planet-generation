    // void GenerateMesh()
    // {
    //     // Initialize the mesh vertices and triangles
    //     vertices = new Vector3[0];
    //     triangles = new int[0];

    //     // Generate the mesh using the marching cubes algorithm
    //     for (int x = 0; x < planetRadius * 2; x++)
    //     {
    //         for (int y = 0; y < planetRadius * 2; y++)
    //         {
    //             for (int z = 0; z < planetRadius * 2; z++)
    //             {
    //                 int i = x * gridHeight * gridDepth + y * gridDepth + z;
    //                 float[] values = new float[8];

    //                 // Get the values of the 8 vertices of the cube
    //                 values[0] = grid[i].value;
    //                 values[1] = grid[i + 1].value;
    //                 values[2] = grid[i + gridHeight].value;
    //                 values[3] = grid[i + gridHeight + 1].value;
    //                 values[4] = grid[i + gridHeight * gridDepth].value;
    //                 values[5] = grid[i + gridHeight * gridDepth + 1].value;
    //                 values[6] = grid[i + gridHeight * gridDepth + gridHeight].value;
    //                 values[7] = grid[i + gridHeight * gridDepth + gridHeight + 1].value;


    //             }
    //         }
    //     }

    //     // Set the mesh vertices and triangles
    //     mesh.vertices = vertices;
    //     mesh.triangles = triangles;

    //     // Recalculate the mesh normals and bounds
    //     mesh.RecalculateNormals();
    //     mesh.RecalculateBounds();
    // }
