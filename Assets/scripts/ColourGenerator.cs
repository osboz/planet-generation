using UnityEngine;

// Generates and updates the colors of a planet's texture based on its settings and elevation
public class ColourGenerator
{
    ColourSettings settings;    // The color settings to use for generating the texture
    Texture2D texture;          // The texture to generate and update
    const int textureResolution = 256; // The resolution of the texture

    // Updates the color settings and creates the texture if it doesn't already exist
    public void UpdateSettings(ColourSettings settings)
    {
        this.settings = settings;

        // Create the texture if it doesn't already exist
        if (texture == null) 
        {
            texture = new Texture2D(textureResolution, 1);
        }
    }

    // Updates the planet material with the elevation min and max values
    public void UpdateElevation(minMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.min, elevationMinMax.max));
    }

    // Generates the color gradient and applies it to the texture and planet material
    public void UpdateColours()
    {
        Color[] colours = new Color[textureResolution];

        // Evaluate the color gradient at each point and store the resulting color in the array
        for (int i = 0; i < textureResolution; i++)
        {
            colours[i] = settings.planetGradient.Evaluate((float)i / (textureResolution - 1f));
        }

        // Set the texture pixels to the generated colors and apply the changes
        texture.SetPixels(colours);
        texture.Apply();

        // Update the planet material with the generated texture
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
