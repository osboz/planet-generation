//https://github.com/SebLague/Procedural-Planets

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{

    Planet planet;
    Editor shapeEditor;
    Editor colourEditor;

    // Metoden OnInspectorGUI() bliver kaldt hver gang inspektoren for en Planet-objekt er åben.
    // Denne metode viser indstillingerne for farver og form, og giver mulighed for at generere en planet.
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            // Kald OnInspectorGUI() fra base-klassen for at vise standardindstillingerne.
            base.OnInspectorGUI();
            // Generer planeten, hvis der er blevet lavet ændringer.
            if (check.changed) planet.GeneratePlanet();
        }

        // Laver en knap for at updatere planeten
        if (GUILayout.Button("Updater planet")) planet.GeneratePlanet();
        // Laver en knap for at generere en ny planeten.
        if (GUILayout.Button("Generér med samme farver planet")) planet.GeneraterRandomShape();

        // Vis indstillingerne for farver og form.
        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.colourSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref planet.shapeSettingsFoldout, ref colourEditor);
    }

    // Metoden DrawSettingsEditor() viser indstillingerne for farver og form.
    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        // Hvis indstillingerne er null, så returneres.
        if (settings == null) return;

        // Vis en dropdown-menu for indstillingerne.
        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

        using (var check = new EditorGUI.ChangeCheckScope())
        {
            // Hvis dropdown-menuen ikke er åben, så returneres.
            if (foldout == false) return;

            // Opret en editor til at vise indstillingerne.
            CreateCachedEditor(settings, null, ref editor);
            editor.OnInspectorGUI();

            // Hvis der ikke er blevet lavet ændringer, så returneres.
            if (check.changed == false) return;

            // Kald callback-funktionen for når indstillingerne er blevet opdateret.
            if (onSettingsUpdated == null) return;
            onSettingsUpdated();
        }
    }
    // Metoden OnEnable() bliver kaldt når editoren er åben for en Planet-objekt.
    // Denne metode sætter en reference til Planet-objektet.
    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
