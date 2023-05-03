using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{
    ColourSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    public void UpdateSettings(ColourSettings settings)
    {
        this.settings = settings;
        if (texture == null) texture = new Texture2D(textureResolution, 1);
    }

    public void updateElevation(minMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.min, elevationMinMax.max));
    }

    public void updateColours()
    {
        Color[] colours = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            colours[i] = settings.planetGradient.Evaluate((float)i / (textureResolution - 1f));

        }

        texture.SetPixels(colours);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }


}