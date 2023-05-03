using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This scriptable object holds the color settings for the planet
[CreateAssetMenu()]
public class ColourSettings : ScriptableObject
{
    // The gradient used for coloring the planet
    public Gradient planetGradient;

    // The material used for rendering the planet
    public Material planetMaterial;
}
