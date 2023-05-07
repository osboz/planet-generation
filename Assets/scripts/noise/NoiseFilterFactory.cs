using UnityEngine;

// Define a static class for creating different types of noise filters
public static class NoiseFilterFactory
{
    // Define a static method for creating noise filters based on settings
    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        // Use a switch statement to select the correct filter type based on settings
        switch (settings.filterType)
        {
            // If the filter type is "Simple", create a new SimpleNoiseFilter object and return it
            case NoiseSettings.FilterType.Simple: return new SimpleNoiseFilter(settings.simpleNoiseSettings);

            // If the filter type is "Ridgid", create a new RidgeNoiseFilter object and return it
            case NoiseSettings.FilterType.Ridgid: return new RidgeNoiseFilter(settings.ridgidNoiseSettings);
        }

        // If the filter type is not recognized, return null
        return null;
    }
}
