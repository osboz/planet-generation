using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This attribute allows this class to be created as an asset in the Unity editor.
[CreateAssetMenu()]

// This class defines the shape settings of a planet.
public class ShapeSettings : ScriptableObject
{
    // The radius of the planet.
    public float planetRadius = 1;

    // An array of noise layers to apply to the planet's surface.
    public NoiseLayer[] noiseLayers;

    // This class defines a noise layer.
    [System.Serializable]
    public class NoiseLayer
    {
        // Whether this noise layer is enabled or not.
        public bool enabled = true;

        // Whether to use the first noise layer as a mask for subsequent layers.
        public bool useFirstLayerAsMask;

        // The settings for the noise to be applied to this layer.
        public NoiseSettings noiseSettings;
    }

    // This method randomly changes the centre point of each noise layer.
    public void randomShape()
    {
        // Iterate over each noise layer in the noiseLayers array.
        foreach (NoiseLayer NL in noiseLayers)
        {
            // Set the centre point of the noise to a random position.
            NL.noiseSettings.simpleNoiseSettings.centre = new Vector3(Random.Range(-10000, 10000), Random.Range(-10000, 10000), Random.Range(-10000, 10000));
        }
    }
}
