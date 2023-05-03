using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class to hold the settings for generating noise
[System.Serializable]
public class NoiseSettings
{
    // An enumeration for the type of filter
    public enum FilterType { Simple, Ridgid };
    
    // The selected filter type
    public FilterType filterType;
    
    // The settings for generating simple noise
    public SimpleNoiseSettings simpleNoiseSettings;
    
    // The settings for generating ridgid noise
    public RidgidNoiseSettings ridgidNoiseSettings;

    // A class to hold the settings for generating simple noise
    [System.Serializable]
    public class SimpleNoiseSettings
    {
        // The strength of the noise
        public float strength = 1;

        // The number of noise layers
        [Range(1, 8)]
        public int numLayers = 1;

        // The base roughness of the noise
        public float baseRoughness = 1;

        // The roughness of the noise
        public float roughness = 2;

        // The persistence of the noise
        public float persistence = .5f;

        // The centre point of the noise
        public Vector3 centre;

        // The minimum value of the noise
        public float minValue;
    }

    // A class to hold the settings for generating ridgid noise
    [System.Serializable]
    public class RidgidNoiseSettings : SimpleNoiseSettings
    {
        // The weight multiplier of the noise
        public float weightMultiplier = .8f;
    }
}
