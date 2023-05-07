using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgeNoiseFilter : INoiseFilter
{
    // definerer en NoiseSetting som settings
    NoiseSettings.RidgidNoiseSettings settings;

    // definerer og instantierer en ny Noise klasse
    Noise noise = new Noise();

    public RidgeNoiseFilter(NoiseSettings.RidgidNoiseSettings settings)
    {
        this.settings = settings;
    }

    // Evaluerer støjgeneratoren på et givent punkt i rummet.
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0; // Støjværdien ved det givne punkt.
        float frequency = settings.baseRoughness; // Grundfrekvensen for støjen.
        float amplitude = 1; // Amplituden for støjen.
        float weight = 1;

        // Gennemløb de enkelte lag af støj.
        for (int i = 0; i < settings.numLayers; i++)
        {
            // Evaluer støjværdien ved det givne punkt for det aktuelle lag.
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            // Tilføj støjværdien til den samlede støjværdi, vægtet efter amplituden.
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);
            noiseValue += v * amplitude;
            
            // Opdater frekvensen og amplituden for det næste lag af støj.
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        // Anvend en minimumsværdi og juster støjværdien efter det.
        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        // Anvend styrken på støjværdien og returnér den.
        return noiseValue * settings.strength;
    }
}
