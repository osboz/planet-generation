using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid
{
    // Definer en struktur kaldet 'point', der indeholder positionen af et punkt i 3D-rummet og en værdi
    public struct point
    {
        public Vector3 position; // Positionen af punktet i 3D-rummet
        public float value; // En tilfældig værdi for punktet (bare for at illustrere brugen af en struktur med flere felter)
    }

    public point[,,] points; // Definer en array af 'point'-strukturer

    // Metode til at oprette en ny grid med en given størrelse
    public void createGrid(int size)
    {
        points = new point[size, size, size];

    }

    // Metode til at tilføje et punkt til grid'en på en given position med en given værdi
    public void addPoint(Vector3Int point, Vector3 position, float value)
    {
        point p = new point(); // Opret en ny 'point'-struktur
        p.position = position; // Sæt positionen på punktet
        p.value = value; // Sæt værdien på punktet
        points[point.x, point.y, point.z] = p; // Tilføj punktet til array'en
    }


}

