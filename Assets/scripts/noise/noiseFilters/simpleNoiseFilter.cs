using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    // definerer og instantierer en ny Noise klasse
    Noise noise = new Noise();

    // definerer en NoiseSetting som settings
    NoiseSettings.SimpleNoiseSettings settings;

    // contructor
    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    // Evaluerer noisegeneratoren på et givent punkt i rummet.
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0; // noiseværdien ved det givne punkt.
        float frequency = settings.baseRoughness; // Grundfrekvensen for noisen.
        float amplitude = 1; // Amplituden for noisen.

        // Gennemløb de enkelte lag af noise.
        for (int i = 0; i < settings.numLayers; i++)
        {
            // Evaluer noiseværdien ved det givne punkt for det aktuelle lag.
            float v = noise.Evaluate(point * frequency + settings.centre);
            // Tilføj noiseværdien til den samlede noiseværdi, vægtet efter amplituden.
            noiseValue += (v + 1) * 0.5f * amplitude;
            // Opdater frekvensen og amplituden for det næste lag af noise.
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }
        // Anvend en minimumsværdi og juster noiseværdien efter det.
        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        // Anvend styrken på noiseværdien og returnér den.
        return noiseValue * settings.strength;

    }
}