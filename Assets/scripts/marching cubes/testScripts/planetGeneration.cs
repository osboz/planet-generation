using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEditor;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class planetGeneration : MonoBehaviour
{

    [SerializeField][Min(0.1f)] private float isolevel = 0.5f; // The isolevel threshold for the marching cubes algorithm
    [SerializeField] private float voxelSize = 1f; // The size of each voxel

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;


    // Definitionen af en struktur "point"
    struct point
    {
        public Vector3Int position; // Vektor som angiver positionen for "pointet"
        public float value; // En værdi for "pointet"
    }

    // De variabler som er synlige i editoren
    [SerializeField][Range(0,10)] private int planetRadius = 1; // The radius of the planet

    [SerializeField] private float sphereSize = 0.1f; // Størrelsen på sfæren som repræsenterer "pointet"

    [SerializeField] private float pointMinValue = 0f, pointMaxValue = 1f; // Højeste og laveste mulige værdier for "pointet"

    [Space]
    [SerializeField] private bool drawGizmos = true; // Boolean som indikerer om Gizmos skal tegnes eller ej

    [SerializeField] private point[] grid; // Array som indeholder alle "point" på planeten

    // Calculate the number of points in each dimension based on the planet radius
    private int gridWidth { get { return Mathf.CeilToInt(planetRadius * 2); } }
    private int gridHeight { get { return Mathf.CeilToInt(planetRadius * 2); } }
    private int gridDepth { get { return Mathf.CeilToInt(planetRadius * 2); } }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateMesh();
    }


    /// <summary>
    /// OnGUI kaldes for at tegne og håndtere GUI-events.
    /// Denne funktion kan kaldes flere gange per frame (en gang per event).
    /// </summary>
    void OnValidate()
    {
        generateGrid(); // Hvis editoren er blevet ændret, genereres planeten på ny
    }


    void generateGrid()
    {
        grid = new point[gridWidth * gridHeight * gridDepth]; // Opretter et nyt array med størrelsen "gridSize"
        // Get the position of the game object this script is attached to
        Vector3 center = transform.position;

        // Iterates through all the points and sets their position relative to the center
        for (int x = 0; x < gridWidth; x++) for (int y = 0; y < gridHeight; y++) for (int z = 0; z < gridDepth; z++)
                {
                    int i = x * gridHeight * gridDepth + y * gridDepth + z;

                    // Set the position of the point relative to the center
                    grid[i].position = new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, z - gridDepth / 2);
                    grid[i].value = Vector3.Distance(grid[i].position, center);
                }
    }

    void GenerateMesh()
    {
        // Initialize the mesh vertices and triangles
        vertices = new Vector3[0];
        triangles = new int[0];

        // Generate the mesh using the marching cubes algorithm
        for (int x = 0; x < planetRadius * 2; x++)
        {
            for (int y = 0; y < planetRadius * 2; y++)
            {
                for (int z = 0; z < planetRadius * 2; z++)
                {
                    int i = x * gridHeight * gridDepth + y * gridDepth + z;
                    float[] values = new float[8];

                    // Get the values of the 8 vertices of the cube
                    values[0] = grid[i].value;
                    values[1] = grid[i + 1].value;
                    values[2] = grid[i + gridHeight].value;
                    values[3] = grid[i + gridHeight + 1].value;
                    values[4] = grid[i + gridHeight * gridDepth].value;
                    values[5] = grid[i + gridHeight * gridDepth + 1].value;
                    values[6] = grid[i + gridHeight * gridDepth + gridHeight].value;
                    values[7] = grid[i + gridHeight * gridDepth + gridHeight + 1].value;


                }
            }
        }

        // Set the mesh vertices and triangles
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate the mesh normals and bounds
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }


    /// <summary>
    /// OnDrawGizmos bliver kaldt for at tegne og opdatere Gizmos i editoren.
    /// </summary>
    void OnDrawGizmos()
    {
        if (grid == null) return; // Går ud af OnDrawGizmos, hvis "grid" ikke er instatieret

        if (drawGizmos == false) return; // Tjekker om vi skal tegne Gizmos eller ej

        // Itererer gennem hvert "point" i arrayet "grid"
        for (int i = 0; i < grid.Length; i++)
        {
            float value = grid[i].value; // Henter værdien for det pågældende "point"

            // Sætter farven for Gizmoet til at være hvid eller sort

            Color color = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(pointMinValue, pointMaxValue, value));
            color.a = Mathf.InverseLerp(planetRadius * pointMaxValue, pointMinValue, value) * 2;

            Gizmos.color = color;
            sphereSize =  Mathf.Lerp(planetRadius * pointMaxValue, pointMinValue, value);

            // Tegner en sfære i positionen for det pågældende "point"
            Gizmos.DrawSphere(new Vector3(grid[i].position.x, grid[i].position.y, grid[i].position.z), sphereSize);
        }

    }
}
