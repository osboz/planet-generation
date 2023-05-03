// The Planet class is responsible for generating the planet based on the given ShapeSettings and ColourSettings.
// The class creates MeshFilters and TerrainFace objects for each side of the planet.
// The class also contains methods to generate the planet's mesh and colors.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    // Specifies the number of quads in the mesh
    [Range(2, 256)]
    public int resolution = 64;

    // Specifies which faces of the planet to render
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    // Specifies whether the planet should be automatically updated when a setting is changed
    public bool autoUpdate = false;

    // References to the ColourSettings and ShapeSettings objects
    public ColourSettings colourSettings;
    public ShapeSettings shapeSettings;

    // Specifies whether the color and shape settings are expanded in the Unity editor
    [HideInInspector]
    public bool shapeSettingsFoldout, colourSettingsFoldout;

    // The object that generates the planet's shape
    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColourGenerator colourGenerator = new ColourGenerator();

    // MeshFilters and TerrainFace objects for each side of the planet
    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    // Creates ShapeGenerator and TerrainFace objects for each side of the planet
    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        // Create the meshFilters array if it hasn't already been created
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        terrainFaces = new TerrainFace[6];

        // Vector3 array of direction vectors for each side of the planet
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        // Create MeshFilters and TerrainFace objects for each side of the planet
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    // Generates a brand new planet with mesh and colors
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    // Updates the planet's shape when ShapeSettings are changed
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate == false) return;
        Initialize();
        GenerateMesh();
    }

    // Updates the planet's colors when ColorSettings are changed
    public void OnColourSettingsUpdated()
    {
        if (autoUpdate == false) return;
        Initialize();
        GenerateColours();
    }

    // Generates the mesh for each face of the planet
    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            // Only construct mesh for active faces
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }// Update colour generator with elevation data
        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    // Generates the colours for the planet based on the current colour settings
    void GenerateColours()
    {
        colourGenerator.UpdateColours();
    }

    // Generates a new random shape for the planet and regenerates the mesh and colours
    public void GeneraterRandomShape()
    {
        shapeSettings.randomShape();
        GeneratePlanet();
    }

}