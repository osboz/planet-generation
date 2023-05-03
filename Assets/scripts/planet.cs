// Planet-klassen er ansvarlig for at generere planeten ud fra de givne ShapeSettings og ColourSettings.
// Klassen opretter mesh-filtre og TerrainFace-objekter til hver side af planeten.
// Klassen indeholder også metoder til at generere planetens mesh og farver.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    // Angiver antallet af quads i meshet
    [Range(2, 256)]
    public int resolution = 10;

    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    // Angiver, om planeten automatisk skal opdateres, når en indstilling ændres
    public bool autoUpdate = false;

    // Referencer til ColourSettings- og ShapeSettings-objekterne
    public ColourSettings colourSettings;
    public ShapeSettings shapeSettings;

    // Angiver, om farve- og form-indstillingerne er foldet ud i Unity-editoren
    [HideInInspector]
    public bool shapeSettingsFoldout, colourSettingsFoldout;

    // Objektet, der genererer planetens form
    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColourGenerator colourGenerator = new ColourGenerator();

    // Mesh-filtrene og TerrainFace-objekterne til hver side af planeten
    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    // Opretter ShapeGenerator- og TerrainFace-objekterne til hver side af planeten
    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        // Opret meshFilters-arrayet, hvis det ikke allerede er gjort
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        terrainFaces = new TerrainFace[6];

        // Vector3-arrayet med retningsvektorer for hver side af planeten
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        // Opretter mesh-filtre og TerrainFace-objekter til hver side af planeten
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

    // Genererer en helt ny planet med mesh og farver
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    // Opdaterer planetens form, når ShapeSettings ændres
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate == false) return;
        Initialize();
        GenerateMesh();
    }

    // Opdaterer planetens farver, når ColourSettings ændres
    public void OnColourSettingsUpdated()
    {
        if (autoUpdate == false) return;
        Initialize();
        GenerateColours();
    }

    // Genererer meshet til hver side af planeten
    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colourGenerator.updateElevation(shapeGenerator.elevationMinMax);

    }

    // Genererer farverne på planeten ud fra de nuværende farveindstillinger

    void GenerateColours()
    {
        colourGenerator.updateColours();

    }
}