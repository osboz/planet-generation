using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class gridVisualizer : MonoBehaviour
{
    [SerializeField] private bool reGenerateGrid = false;
    [Min(1)][SerializeField] private float gridSize = 1; // Størrelsen af grid'en
    [Min(1)][SerializeField] private int resolution = 10; // Størrelsen af grid'en
    [SerializeField] private float planetSize = 0.5f; // Værdien for de tilføjede punkter

    grid mainGrid; // Reference til grid-objektet

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        createGrid(resolution);
    }

    void OnDrawGizmos()
    {
        if (mainGrid == null || mainGrid.points == null) return; // Sørg for, at grid'en er oprettet og har nogle punkter

        // Loop igennem alle punkterne i grid'en
        foreach (grid.point p in mainGrid.points)
        {

            Gizmos.color = Color.Lerp(Color.white, Color.black, p.value);

            Gizmos.DrawCube(p.position, Vector3.one * 0.2f); // Tegn en kugle-Gizmo på punktets position
        }
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        if (reGenerateGrid)
            createGrid(resolution);
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

