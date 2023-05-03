using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    // The shape settings used to generate the planet
    ShapeSettings settings;

    // An array of noise filters used to generate the planet's shape
    INoiseFilter[] noiseFilters;

    // The minimum and maximum elevation of the planet
    public minMax elevationMinMax;


    // Update the settings used to generate the planet's shape
    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;

        // Create an array of noise filters using the noise settings specified in the shape settings
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }

        // Reset the elevation min and max values
        elevationMinMax = new minMax();
    }

    // Calculate the point on the planet's surface for a given point on the unit sphere
    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        // Calculate the value of the first noise layer
        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        // Calculate the value of the remaining noise layers
        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                // Use the value of the first layer as a mask if specified in the noise layer settings
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        // Scale the elevation based on the planet's radius and add it to the point on the unit sphere
        elevation = settings.planetRadius * (1 + elevation);
        elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;
    }
}
