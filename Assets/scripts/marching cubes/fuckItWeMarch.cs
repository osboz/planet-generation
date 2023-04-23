using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class fuckItWeMarch : MonoBehaviour
{
    public struct Triangle
    {
        public Vector3 vertexC;
        public Vector3 vertexB;
        public Vector3 vertexA;
    };

    List<Triangle> triangles = new List<Triangle>();

    [SerializeField] private bool reGenerateGrid = false;
    [Min(1)][SerializeField] private float gridSize = 10; // Størrelsen af grid'en
    [Min(1)][SerializeField] private int resolution = 10; // Størrelsen af grid'en
    [SerializeField] private float planetSize = 5f; // Værdien for de tilføjede punkter

    grid mainGrid;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        createGrid(resolution);
        march();
        createMesh();
    }


    void march()
    {
        // Calculate coordinates of each corner of the current cube

        for (int x = 0; x < mainGrid.points.GetLength(0) - 1; x++)
            for (int y = 0; y < mainGrid.points.GetLength(1) - 1; y++)
                for (int z = 0; z < mainGrid.points.GetLength(2) - 1; z++)
                {

                    Vector3[] cornerCoords = new Vector3[8];
                    cornerCoords[0] = mainGrid.points[x, y, z].position;
                    cornerCoords[1] = mainGrid.points[x + 1, y, z].position;
                    cornerCoords[2] = mainGrid.points[x, y + 1, z].position;
                    cornerCoords[3] = mainGrid.points[x, y, z + 1].position;
                    cornerCoords[4] = mainGrid.points[x + 1, y + 1, z].position;
                    cornerCoords[5] = mainGrid.points[x + 1, y, z + 1].position;
                    cornerCoords[6] = mainGrid.points[x, y + 1, z + 1].position;
                    cornerCoords[7] = mainGrid.points[x + 1, y + 1, z + 1].position;


                    Debug.Log(cornerCoords[0]);

                    Debug.Log(cornerCoords[1]);
                    // Calculate unique index for each cube configuration.
                    // There are 256 possible values (cube has 8 corners, so 2^8 possibilites).
                    // A value of 0 means cube is entirely inside the surface; 255 entirely outside.
                    // The value is used to look up the edge table, which indicates which edges of the cube the surface passes through.
                    int cubeConfiguration = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        // Think of the configuration as an 8-bit binary number (each bit represents the state of a corner point).
                        // The state of each corner point is either 0: above the surface, or 1: below the surface.
                        // The code below sets the corresponding bit to 1, if the point is below the surface.
                        if (Vector3.Distance(this.transform.position, mainGrid.points[x, y, z].position) < planetSize)
                        {
                            cubeConfiguration |= (1 << i);
                        }
                    }

                    // Get array of the edges of the cube that the surface passes through.
                    int[] edgeIndices = new int[16];

                    for (int i = 0; i < 16; i++)
                    {
                        edgeIndices[i] = MarchingTable.triangulation[cubeConfiguration, i];
                    }


                    // Create triangles for the current cube configuration
                    for (int i = 0; i < 16; i += 3)
                    {
                        // If edge index is -1, then no further vertices exist in this configuration
                        if (edgeIndices[i] == -1) { break; }
                        // Get indices of the two corner points defining the edge that the surface passes through.
                        // (Do this for each of the three edges we're currently looking at).
                        int edgeIndexA = edgeIndices[i];
                        int a0 = MarchingTable.cornerIndexAFromEdge[edgeIndexA];
                        int a1 = MarchingTable.cornerIndexBFromEdge[edgeIndexA];

                        int edgeIndexB = edgeIndices[i + 1];
                        int b0 = MarchingTable.cornerIndexAFromEdge[edgeIndexB];
                        int b1 = MarchingTable.cornerIndexBFromEdge[edgeIndexB];

                        int edgeIndexC = edgeIndices[i + 2];
                        int c0 = MarchingTable.cornerIndexAFromEdge[edgeIndexC];
                        int c1 = MarchingTable.cornerIndexBFromEdge[edgeIndexC];

                        // Calculate positions of each vertex.
                        Vector3 vertexA = createVertex(cornerCoords[a0], cornerCoords[a1]);
                        Vector3 vertexB = createVertex(cornerCoords[b0], cornerCoords[b1]);
                        Vector3 vertexC = createVertex(cornerCoords[c0], cornerCoords[c1]);

                        // Create triangle
                        Triangle tri = new Triangle();
                        tri.vertexA = vertexC;
                        tri.vertexB = vertexB;
                        tri.vertexC = vertexA;
                        triangles.Add(tri);
                    }
                }
    }


    Vector3 createVertex(Vector3 p1p, Vector3 p2p)
    {
        Vector3 returnthing = p1p + p2p;
        returnthing = returnthing * 0.5f;
        return returnthing;
    }


    void createMesh()
    {
        Mesh mesh = new Mesh();

        // Create an array of vertices from the triangles
        Vector3[] vertices = new Vector3[triangles.Count * 3];
        for (int i = 0; i < triangles.Count; i++)
        {
            vertices[i * 3] = triangles[i].vertexA;
            vertices[i * 3 + 1] = triangles[i].vertexB;
            vertices[i * 3 + 2] = triangles[i].vertexC;
        }
        mesh.vertices = vertices;

        // Create an array of indices for the triangles
        int[] indices = new int[triangles.Count * 3];
        for (int i = 0; i < triangles.Count; i++)
        {
            indices[i * 3] = i * 3;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;
        }
        mesh.triangles = indices;

        // Calculate normals
        mesh.RecalculateNormals();

        // Assign the mesh to the mesh filter component
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }



    // Metode til at oprette en grid med den angivne størrelse
    public void createGrid(int size)
    {
        mainGrid = new grid(); // Opret et nyt grid-objekt
        mainGrid.createGrid(resolution); // Opret grid'en med den angivne størrelse

        // Tilføj nogle tilfældige punkter til grid'en
        for (int x = 0; x < resolution; x++) for (int y = 0; y < resolution; y++) for (int z = 0; z < resolution; z++)
                {

                    Vector3 percent = new Vector3(x, y, z) / (resolution - 1);

                    // Beregn positionen af punktet baseret på x-, y- og z-koordinaterne og planetens størrelse
                    Vector3 position = (percent - Vector3.one * 0.5f) * gridSize;

                    // Beregn en værdi for punktet baseret på dets afstand til planetens center og en tilfældig Perlin-støjværdi
                    float value = planetSize - Vector3.Distance(this.gameObject.transform.position, position) + Mathf.PerlinNoise(x * z, y * z);

                    // Tilføj punktet med dets position og værdi til grid'en
                    mainGrid.addPoint(new Vector3Int(x, y, z), position, value);
                }
    }

}
