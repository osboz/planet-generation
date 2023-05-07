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
       
    // The settings for generating ridgid noise
    public RidgidNoiseSettings ridgidNoiseSettings;

    // A class to hold the settings for generating simple noise
    [System.Serializable]
    public class SimpleNoiseSettings
    {
        // variables to change for the noise
        public float strength = 1;
        [Range(1, 8)]
        public int numLayers = 1;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = .5f;
        public Vector3 centre;
        public float minValue;
    }

    // A class to hold the settings for generating ridgid noise
    [System.Serializable]
    public class RidgidNoiseSettings : SimpleNoiseSettings
    {
        // settings special to the RidgidNoiseSettings setting class
        public float weightMultiplier = .8f;
    }
}
