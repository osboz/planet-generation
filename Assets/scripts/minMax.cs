// This class represents a minimum and maximum value pair.
public class minMax
{

    // The minimum value of the pair.
    public float min { get; private set; }

    // The maximum value of the pair.
    public float max { get; private set; }

    // Constructor for the class.
    public minMax()
    {
        // Set the minimum value to the maximum possible value of a float.
        min = float.MaxValue;

        // Set the maximum value to the minimum possible value of a float.
        max = float.MinValue;
    }

    // Add a new value to the min-max pair.
    // The minimum and maximum values will be updated accordingly.
    public void AddValue(float v)
    {
        // Update the maximum value if the new value is greater than the current maximum.
        if (v > max) max = v;

        // Update the minimum value if the new value is less than the current minimum.
        if (v < min) min = v;
    }

}
/*
Summary, the minMax class represents a pair of values, where the min property stores the minimum value of the pair, and the max property stores the maximum value of the pair. 
The class has a constructor that initializes the min property to the maximum possible value of a float and the max property to the minimum possible value of a float. 
The AddValue method allows you to add a new value to the pair, and it updates the min and max properties accordingly.
*/